using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Create SkillTemp")]
public class SkillTemp : ScriptableObject
{
    public Skill skill = new Skill();

    /// <summary>技能类型，可用 | 拼接</summary>>
    public DamageType[] damageType;
}
