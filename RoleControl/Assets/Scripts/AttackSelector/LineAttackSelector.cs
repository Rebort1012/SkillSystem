using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAttackSelector : IAttackSelector
{
    public GameObject[] SelectTarget(SkillData skillData, Transform skillTransform)
    {
        //发一个球形射线，找出所有碰撞体
        var colliders = Physics.OverlapSphere(skillTransform.position, skillData.skill.attackDisntance);
        if (colliders == null || colliders.Length == 0) return null;

        //从碰撞体列表中挑出所有的敌人
        string[] attTags = skillData.skill.attckTargetTags;
        var array = CollectionHelper.Select<Collider, GameObject>(colliders, p => p.gameObject);
        array = CollectionHelper.FindAll<GameObject>(array,
            p => Array.IndexOf(attTags, p.tag) >= 0
                 && p.GetComponent<CharacterStatus>().HP > 0 &&
                 Mathf.Abs(p.transform.position.z - skillTransform.position.z) < skillData.skill.attackDisntance &&
                 Mathf.Abs(p.transform.position.x - skillTransform.position.x) < skillData.skill.attackWidth / 2);

        if (array == null || array.Length == 0) return null;

        GameObject[] targets = null;
        //根据技能是单体还是群攻，决定返回多少敌人对象
        if (skillData.skill.attackNum == 1)
        {
            //将所有的敌人，按与技能的发出者之间的距离升序排列，
            CollectionHelper.OrderBy<GameObject, float>(array,
                p => Vector3.Distance(skillData.Owner.transform.position, p.transform.position));
            targets = new GameObject[] {array[0]};
        }
        else
        {
            int attNum = skillData.skill.attackNum;
            if (attNum >= array.Length)
                targets = array;
            else
            {
                for (int i = 0; i < attNum; i++)
                {
                    targets[i] = array[i];
                }
            }
        }

        return targets;
    }
}
