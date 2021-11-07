using System;
using System.Collections.Generic;
using UnityEngine;

class SectorAttackSelector : IAttackSelector
{
    public GameObject[] SelectTarget(SkillData skillData, Transform skillTransform)
    {
        //发一个球形射线，找出所有碰撞体
        var colliders = Physics.OverlapSphere(skillTransform.position, skillData.skill.attackDisntance);
        if (colliders == null || colliders.Length == 0) return null;

        //从碰撞体转为gameobject
        var array = CollectionHelper.Select<Collider, GameObject>(colliders, p => p.gameObject);
        
        //选出tag为技能攻击tag，血量大于0，且在攻击扇形范围内的物体
        String[] attTags = skillData.skill.attckTargetTags;
        array = CollectionHelper.FindAll<GameObject>(array, p =>
            {
                float angle = Vector3.Angle(skillTransform.forward, p.transform.position - skillTransform.position);
 
                if (Array.IndexOf(attTags, p.tag) >= 0 &&
                    p.GetComponent<CharacterStatus>().HP > 0 &&
                    Vector3.Angle(skillTransform.forward, p.transform.position - skillTransform.position) <=
                    skillData.skill.attackAngle / 2)
                    return true;
                else
                    return false;
            }
        );

        if (array == null || array.Length == 0)
            return null;

        GameObject[] targets = null;
        //根据技能是单体还是群攻，决定返回多少敌人对象
        if (skillData.skill.attackType == SkillAttackType.Single)
        {
            //将所有的敌人，按与技能的发出者之间的距离升序排列，
            CollectionHelper.OrderBy<GameObject, float>(array,
                p => Vector3.Distance(skillData.Owner.transform.position, p.transform.position));
            targets = new GameObject[] {array[0]};
        }
        else if (skillData.skill.attackType == SkillAttackType.Group)
            targets = array;

        return targets;
    }
}