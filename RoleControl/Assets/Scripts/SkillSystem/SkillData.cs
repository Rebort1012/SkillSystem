using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillData
{
    [HideInInspector] public GameObject Owner;
   
    /// <summary>技能数据</summary>
    [SerializeField]
    public Skill skill;

    /// <summary>技能等级</summary>
    public int level;
    
    /// <summary>冷却剩余</summary>
    [HideInInspector]
    public float coolRemain;
    
    /// <summary>攻击目标</summary>
    [HideInInspector] public GameObject[] attackTargets;

    /// <summary>是否激活</summary>
    [HideInInspector]
    public bool Activated;

    /// <summary>技能预制对象</summary>
    [HideInInspector] 
    public GameObject skillPrefab;
    
    [HideInInspector] 
    public GameObject hitFxPrefab;
}