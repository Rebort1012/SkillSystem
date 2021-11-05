using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkillDeployer : MonoBehaviour
{
    private SkillData m_skillData;

    ///<summary>敌人选区，选择目标的算法</summary>
    public IAttackSelector attackTargetSelector;

    //发出者
    private CharacterStatus status;

    /// <summary> 要释放的技能 </summary>
    public SkillData skillData
    {
        set
        {
            m_skillData = value;
            attackTargetSelector = SelectorFactory.CreateSelector(value.damageMode);
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

            yield return new WaitForSeconds(skillData.damageInterval);
            attackTimer += skillData.damageInterval;
            //做伤害数值的计算
        } while (skillData.durationTime > attackTimer);
    }

    private void ResetTargets()
    {
        if (m_skillData == null) return;
        m_skillData.attackTargets = attackTargetSelector.SelectTarget(m_skillData, transform);
        
    }

    private float CirculateDamage(GameObject goTarget)
    {
        CharacterStatus goStatus = goTarget.GetComponent<CharacterStatus>();
        
        //命中率计算
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
        
        if (m_skillData.name.StartsWith("普攻"))
        {
            var damageVal = status.damage * (1000 / (1000 + goStatus.defence));
            return damageVal;
        }
        else
        {
            return skillData.damage;
        }
    }

    ///对敌人的影响nag
    public virtual void TargetImpact(GameObject goTarget)
    {
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
            if (skillData.skillID == 8)
                hitFxPos = goTarget.transform;
            
            if (hitFxPos != null)
            {
                var go = GameObjectPool.I.CreateObject(
                    skillData.hitFxName,
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
            chStaus.SP -= m_skillData.costSP;
            //add+2 魔法条更新
         
        }
    }
}