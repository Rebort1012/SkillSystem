using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkillDeployer : MonoBehaviour
{
    private SkillData m_skillData;

    ///<summary>敌人选区，选择目标的算法</summary>
    public IAttackSelector attackTargetSelector;

    private DamageMode damageMode;

    //发出者
    private CharacterStatus status;

    /// <summary> 要释放的技能 </summary>
    public SkillData skillData
    {
        set
        {
            m_skillData = value;
            damageMode = 0;
            if ((skillData.skill.damageType & DamageType.Sector) == DamageType.Sector)
                damageMode = DamageMode.Sector;
            else if ((skillData.skill.damageType & DamageType.Circle) == DamageType.Circle)
                damageMode = DamageMode.Circle;
            else if ((skillData.skill.damageType & DamageType.Line) == DamageType.Line)
                damageMode = DamageMode.Line;

            if (damageMode != 0)
                attackTargetSelector = SelectorFactory.CreateSelector(damageMode);

            status = value.Owner.GetComponent<CharacterStatus>();
        }
        get { return m_skillData; }
    }


    /// <summary>技能释放</summary>
    public virtual void DeploySkill()
    {
        if (m_skillData == null) return;
        //对自身的影响
        SelfImpact(m_skillData.Owner);

        //执行伤害的计算
        if (damageMode != 0) 
            StartCoroutine(ExecuteDamage());
    }

    //执行伤害的计算
    protected virtual IEnumerator ExecuteDamage()
    {
        //按持续时间及，两次伤害间隔，
        float attackTimer = 0; //已持续攻击的时间
        
        ResetTargets();
        if (skillData.attackTargets != null && skillData.attackTargets.Length > 0)
        {
            //Debug.Log(skillData.attackTargets[0].name);
            foreach (var item in skillData.attackTargets)
            {
                //刷新敌人头像显示
                CharacterStatus targetStatus = item.GetComponent<CharacterStatus>();
                MonsterMgr.I.HideAllEnemyPortraits();
                targetStatus.uiPortrait.ShowPortrait();

                //加buff
                foreach (var buff in skillData.skill.buffType)
                {
                    //加bufficon
                    targetStatus.uiPortrait.AddBuffIcon(buff, skillData.skill.buffDuration);
                    
                    //已有该buff刷新
                    bool exist = false;
                    var buffs = item.GetComponents<BuffRun>();
                    
                    foreach (var it in buffs)
                    {
                        if (it.bufftype == buff)
                        {
                            it.Reset();
                            exist = true;
                            break;
                        }
                    }

                    if (exist)
                    {
                        continue;
                    }

                    //添加新buff
                    var buffRun = item.AddComponent<BuffRun>();
                    buffRun.InitBuff(buff, skillData.skill.buffDuration, skillData.skill.buffValue,
                        skillData.skill.buffInterval);
                }
            }
        }

        do
        {
            //通过选择器选好攻击目标
            ResetTargets();
            if (skillData.attackTargets != null && skillData.attackTargets.Length > 0)
            {
                //Debug.Log(skillData.attackTargets[0].name);
                foreach (var item in skillData.attackTargets)
                {
                    //对敌人的影响
                    TargetImpact(item);
                }
            }

            yield return new WaitForSeconds(skillData.skill.damageInterval);
            attackTimer += skillData.skill.damageInterval;
            //做伤害数值的计算
        } while (skillData.skill.durationTime > attackTimer);
    }

    private void ResetTargets()
    {
        if (m_skillData == null)
            return;

        m_skillData.attackTargets = attackTargetSelector.SelectTarget(m_skillData, transform);
    }

    private float CirculateDamage(GameObject goTarget)
    {
        CharacterStatus goStatus = goTarget.GetComponent<CharacterStatus>();

        //是否命中计算
        float rate = status.hitRate / (float) goStatus.dodgeRate;
        if (rate < 1)
        {
            int max = (int) (rate * 100);
            int val = Random.Range(0, 100);
            if (val < max)
            {
                //Debug.Log("Miss");
                return 0;
            }
        }

        //普攻的技能伤害为0; 技能有固定伤害*等级加成 + 普攻伤害
        var damageVal = status.damage * (1000 / (1000 + goStatus.defence)) +
                        skillData.skill.damage * (1 + skillData.level * skillData.skill.damageRatio);
        return damageVal;
    }

    ///对敌人的影响nag
    public virtual void TargetImpact(GameObject goTarget)
    {
        //出受伤特效
        if (skillData.hitFxPrefab != null)
        {
            //找到受击特效的挂点
            Transform hitFxPos = goTarget.GetComponent<CharacterStatus>().HitFxPos;

            var go = GameObjectPool.I.CreateObject(
                skillData.skill.hitFxName,
                skillData.hitFxPrefab,
                hitFxPos.position,
                hitFxPos.rotation);
            go.transform.SetParent(hitFxPos);
            GameObjectPool.I.Destory(go, 2f);
        }

        //受伤
        var damageVal = CirculateDamage(goTarget);
        var targetStatus = goTarget.GetComponent<CharacterStatus>();
        targetStatus.OnDamage((int) damageVal, skillData.Owner);
    }

	//碰撞触发目标影响
    public virtual void TargetImpact(GameObject goTarget, Collider collider)
    {
        //刷新敌人头像显示
        CharacterStatus targetStatus = goTarget.GetComponent<CharacterStatus>();
        MonsterMgr.I.HideAllEnemyPortraits();
        targetStatus.uiPortrait.ShowPortrait();

        //加buff
        foreach (var buff in skillData.skill.buffType)
        {
            //加bufficon
            targetStatus.uiPortrait.AddBuffIcon(buff, skillData.skill.buffDuration);
        }
        
        //敌人buff
        foreach (var buff in skillData.skill.buffType)
        {
            //已有该buff刷新
            bool exist = false;
            var buffs = goTarget.GetComponents<BuffRun>();
            foreach (var it in buffs)
            {
                if (it.bufftype == buff)
                {
                    it.Reset();
                    exist = true;
                    break;
                }
            }

            if (exist)
                continue;

            //添加新buff
            var buffRun = goTarget.AddComponent<BuffRun>();
            buffRun.InitBuff(buff, skillData.skill.buffDuration,
                skillData.skill.buffValue, skillData.skill.buffInterval);
        }

        //出受伤特效
        if (skillData.hitFxPrefab != null)
        {
            //找到受击特效的挂点，碰撞但未检测到射线点，生成受击特效在hitFxPos处
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            Physics.Raycast((Ray) ray, out hit, 1000);
            if (hit.collider == collider)
            {
                var go = GameObjectPool.I.CreateObject(
                    skillData.skill.hitFxName,
                    skillData.hitFxPrefab,
                    hit.point,
                    transform.rotation);
                GameObjectPool.I.Destory(go, 2f);
            }
            else
            {
                Transform hitFxPos = goTarget.GetComponent<CharacterStatus>().HitFxPos;
                var go = GameObjectPool.I.CreateObject(
                    skillData.skill.hitFxName,
                    skillData.hitFxPrefab,
                    hitFxPos.position,
                    hitFxPos.rotation);
                GameObjectPool.I.Destory(go, 2f);
            }
        }

        //受伤
        var damageVal = CirculateDamage(goTarget);
        targetStatus.OnDamage((int) damageVal, skillData.Owner);
    }

    ///对自身的影响
    public virtual void SelfImpact(GameObject goSelf)
    {
        //释放者: 消耗SP
        var chStaus = goSelf.GetComponent<CharacterStatus>();
        if (chStaus.SP != 0)
        {
            chStaus.SP -= m_skillData.skill.costSP;
            chStaus.uiPortrait.RefreshHpMp();
            //add+2 魔法条更新
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((skillData.skill.damageType & DamageType.Bullet) == DamageType.Bullet)
        {
            if (skillData.skill.attckTargetTags.Contains(other.tag))
            {
                if (skillData.skill.attackNum == 1)
                {
                    TargetImpact(other.gameObject, other);
                }
                else
                {
                    //通过选择器选好攻击目标
                    IAttackSelector selector = new CircleAttackSelector();
                    selector.SelectTarget(m_skillData, transform);
                    if (skillData.attackTargets != null && skillData.attackTargets.Length > 0)
                    {
                        foreach (var item in skillData.attackTargets)
                        {
                            //对敌人的影响
                            TargetImpact(item, other);
                        }
                    }
                }

                GameObjectPool.I.Destory(gameObject);
            }
            else if (other.CompareTag("Wall"))
            {
                if (skillData.hitFxPrefab != null)
                {
                    Ray ray = new Ray(transform.position, transform.forward);
                    RaycastHit hit;
                    Physics.Raycast((Ray) ray, out hit, 1000);

                    if (hit.collider != other)
                        return;

                    //找到受击特效的挂点
                    var go = GameObjectPool.I.CreateObject(
                        skillData.skill.hitFxName,
                        skillData.hitFxPrefab,
                        hit.point,
                        other.transform.rotation);
                    //go.transform.SetParent(hitFxPos);
                    GameObjectPool.I.Destory(go, 2f);
                }

                GameObjectPool.I.Destory(gameObject);
            }
        }
    }
    
    
    public static Dictionary<BuffType, string> buffIconName = new Dictionary<BuffType, string>();
    
    public static void InitBuffIconName()
    {
        buffIconName.Add(BuffType.Burn,"Buff_13");
        buffIconName.Add(BuffType.Slow,"Buff_15");
        buffIconName.Add(BuffType.Stun,"Buff_12");
        buffIconName.Add(BuffType.Poison,"Buff_14");
        buffIconName.Add(BuffType.BeatBack,"Buff_5");
        buffIconName.Add(BuffType.BeatUp,"Buff_4");
        buffIconName.Add(BuffType.Pull,"Buff_6");
        buffIconName.Add(BuffType.AddDefence,"Buff_3");
        buffIconName.Add(BuffType.RecoverHp,"Buff_7");
        buffIconName.Add(BuffType.Light,"Buff_8");
    }
}