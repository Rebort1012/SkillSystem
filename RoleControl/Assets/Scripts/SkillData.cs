using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>攻击类型</summary>
public enum SkillAttackType
{
    /// <summary>单体攻击</summary>
    Single = 1,

    /// <summary>群体攻击</summary>
    Group = 2,
}

/// <summary>
/// 受击模式
/// </summary>
public enum DamageMode
{
    /// <summary>圆形区</summary>                  
    Circle = 1,

    /// <summary>扇形区 </summary>
    Sector = 2,

    /// <summary>线性</summary>
    Line = 3,
}

public class SkillData
{
    [HideInInspector] public GameObject Owner;

    /// <summary>技能编号</summary>
    public int skillID;

    /// <summary>技能图标</summary>
    public string skillIcon;

    /// <summary>描述</summary>
    public string description;

    /// <summary>技能名称</summary>
    public string name;

    /// <summary>持续时间</summary>
    public float durationTime;

    /// <summary>在持续时间内，两次伤害之间的间隔时间</summary>
    public float damageInterval;

    /// <summary>伤害比</summary>
    public float damage;

    /// <summary>冷却时间</summary>
    public int coolTime;

    /// <summary>冷却剩余</summary>
    [HideInInspector]
    public float coolRemain;

    /// <summary>魔法消耗</summary>
    public int costSP;

    /// <summary>攻击距离</summary>
    public float attackDisntance;

    /// <summary>攻击目标</summary>
    [HideInInspector] public GameObject[] attackTargets;

    /// <summary>攻击目标的TAG</summary>
    [HideInInspector] 
    public string attckTargetTag = "Enemy";

    /// <summary>技能等级</summary>
    public int level;

    /// <summary>攻击范围 线形，矩形，扇形，圆形</summary>
    public DamageMode damageMode;

    /// <summary>攻击类型，单攻，群攻</summary>
    public SkillAttackType attackType;

    /// <summary> 攻击范围角度</summary>
    public int attackAngle;
    
    /// <summary>是否激活</summary>
    [HideInInspector]
    public bool Activated;

    /// <summary>技能buff种类</summary>

    
    /// <summary>技能对应的动画名称 </summary>
    public string animtionName;
    
    /// <summary>技能预制对象</summary>
    [HideInInspector] 
    public GameObject skillPrefab;

    /// <summary>预制文件名</summary>
    public string prefabName;
    
    /// <summary>目标受击特效</summary>
    public string hitFxName;
    
    [HideInInspector] 
    public GameObject hitFxPrefab;

    /// <summary>下一个连击技能编号</summary>
    public int nextBatterId;
}