using System;
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

/// <summary>
/// Buff类型，可叠加
/// </summary>
public enum BuffType
{
    None,
    Burn = 2,           //点燃
    Slow = 4,           //减速
    Light = 8,          //感电
    Stun = 16,          //眩晕
    Poison = 32,        //中毒
    BeatBack = 64,      //击退
    BeatUp = 128,       //击飞
    Pull = 256,         //拉拽
}

/// <summary>
/// 技能类型，可叠加
/// </summary>
public enum DamageType
{
    JustInTime = 2,         //直接判定伤害
    Bullet = 4,             //特效粒子碰撞伤害
    Anima = 64,             //动画帧判定伤害
    
    DamageOnce = 128,       //单次伤害
    DamageMult = 512,       //多次伤害
    
    Single = 1024,          //单体
    Group = 2048,           //群体伤害
    
    Circle = 4096,          //圈判定
    Sector = 8192,          //选中判定
    Line = 16384,           //线性判定
}

[Serializable]
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

    /// <summary>伤害判定，粒子伤害，增益buff，减益buff，选中目标触发，动画帧判定</summary>>
    public DamageType damageType;
    
    /// <summary>攻击范围 线形，矩形，扇形，圆形，伤害选择器</summary>
    public DamageMode damageMode;

    /// <summary>持续时间</summary>
    public float durationTime;
    
    /// <summary>在持续时间内，两次伤害之间的间隔时间</summary>
    public float damageInterval;

    /// <summary>伤害比，挂钩英雄属性</summary>
    public float damageRatio;

    /// <summary>固定伤害</summary>
    public float damage;
    
    /// <summary>冷却时间</summary>
    public int coolTime;
    
    // <summary>魔法消耗</summary>
    public int costSP;

    /// <summary>攻击距离</summary>
    public float attackDisntance;

    /// <summary>攻击目标的tag</summary>
    public string[] attckTargetTags;

    /// <summary>攻击类型，单攻，群攻</summary>
    public SkillAttackType attackType;

    /// <summary>攻击扇形角度</summary>
    public int attackAngle;
    
    /// <summary>攻击动画名称</summary>
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
