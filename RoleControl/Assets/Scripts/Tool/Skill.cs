using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    None,           
    Burn,           //点燃
    Slow,           //减速
    Light,          //感电
    Stun,           //眩晕
    Poison,         //中毒
    BeatBack,
    BeatUp,
    Hook
}

/// <summary>
/// 技能类型，可以叠加
/// </summary>
public enum SkillType
{
    Once = 1,               //单次伤害
    Multy = 2,              //多次伤害
    Bullet = 4,             //弹道技能
    Buff = 8,               //给自身加增益buff
    DeBuff = 16,            //给目标加减益buff
}

public class Skill
{
    /// <summary>技能编号</summary>
    public int skillID;

    /// <summary>技能图标</summary>
    public string skillIcon;

    /// <summary>描述</summary>
    public string description;

    /// <summary>技能名称</summary>
    public string name;

    /// <summary>技能类型</summary>>
    public SkillType skillType;
    
    /// <summary>持续时间</summary>
    public float durationTime;

    /// <summary>在持续时间内，两次伤害之间的间隔时间</summary>
    public float damageInterval;

    /// <summary>伤害比</summary>
    public float damageRatio;

    /// <summary>固定伤害</summary>
    public float damage;
    
    /// <summary>冷却时间</summary>
    public int coolTime;
    
    // <summary>魔法消耗</summary>
    public int costSP;

    /// <summary>攻击距离</summary>
    public float attackDisntance;

    public string[] attckTargetTags;

    /// <summary>攻击范围 线形，矩形，扇形，圆形</summary>
    public DamageMode damageMode;

    /// <summary>攻击类型，单攻，群攻</summary>
    public SkillAttackType attackType;

    /// <summary> 攻击范围角度</summary>
    public int attackAngle;
    
    public string animtionName;
    
    /// <summary>预制文件名</summary>
    public string prefabName;
    
    /// <summary>目标受击特效</summary>
    public string hitFxName;

    /// <summary>下一个连击技能编号</summary>
    public int nextBatterId;

    /// <summary>Buff种类</summary>
    public BuffType buffType;

    /// <summary>Buff持续时间</summary>
    public float buffTime;
    

}
