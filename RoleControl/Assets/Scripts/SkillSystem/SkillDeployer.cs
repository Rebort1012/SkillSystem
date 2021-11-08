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
            if((skillData.skill.damageType & DamageType.Sector) ==  DamageType.Sector)
                damageMode = DamageMode.Sector;
            else if((skillData.skill.damageType & DamageType.Circle) ==  DamageType.Circle)
                damageMode = DamageMode.Circle;
            else if((skillData.skill.damageType & DamageType.Line) ==  DamageType.Line)
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
        float rate = status.hitRate / (float)goStatus.dodgeRate;
        if (rate < 1)
        {
            int max = (int)(rate * 100);
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
        //敌人buff
        
        //受伤
        var damageVal =  CirculateDamage(goTarget);
        var targetStatus = goTarget.GetComponent<CharacterStatus>();
        targetStatus.normalAtt = damageVal;
        
        targetStatus.OnDamage((int)damageVal, skillData.Owner);
       
        //出受伤特效
        if (skillData.hitFxPrefab != null)
        {
            //找到受击特效的挂点
            Transform hitFxPos = goTarget.GetComponent<CharacterStatus>().HitFxPos;
            if (skillData.skill.skillID == 8)
                hitFxPos = goTarget.transform;
            
            if (hitFxPos != null)
            {
                var go = GameObjectPool.I.CreateObject(
                    skillData.skill.hitFxName,
                    skillData.hitFxPrefab,
                    hitFxPos.position,
                    hitFxPos.rotation);
                go.transform.SetParent(hitFxPos);
                GameObjectPool.I.Destory(go, 2f);
            }
        }
    }

    ///对自身的影响
    public virtual void SelfImpact(GameObject goSelf)
    {
        //释放者: 消耗SP
        var chStaus = goSelf.GetComponent<CharacterStatus>();
        if (chStaus.SP != 0)
        {
            chStaus.SP -= m_skillData.skill.costSP;
            //add+2 魔法条更新
        }
        
        //自身buff
    }

    private void OnCollisionEnter(Collision other)
    {
        if ((skillData.skill.damageType & DamageType.Bullet) == DamageType.Bullet)
        {
            if (skillData.skill.attckTargetTags.Contains(other.collider.tag))
            {
                StartCoroutine(DoDamageByCollider(other));
            }
        }
    }
    
    private IEnumerator DoDamageByCollider(Collision other)
    {
        float attackTimer = 0; //已持续攻击的时间
        do
        {
            if (skillData.skill.attackNum == 1)
            {
                TargetImpact(other.gameObject);
            }
            else
            {
                //通过选择器选好攻击目标
                IAttackSelector selector = new CircleAttackSelector();
                selector.SelectTarget(m_skillData, other.transform);
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
            }
        } while (skillData.skill.durationTime > attackTimer);
    }

    private string SubstringZero(string social)
    {
        string temp = "";
        if (social.StartsWith("0"))
        {
            temp = social.Substring(1, social.Length);
            SubstringZero(temp);
            return temp;
        }
        return temp;
    }
}