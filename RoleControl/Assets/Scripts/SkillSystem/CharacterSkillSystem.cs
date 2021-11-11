using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// 角色系统
/// </summary>
[RequireComponent(typeof(CharacterSkillManager))]
public class CharacterSkillSystem : MonoBehaviour
{
    //技能管理
    public CharacterSkillManager chSkillMgr;

    //角色状态
    private CharacterStatus chStatus;

    //角色动画
    private Animator mAnimator;

    //当前使用的技能
    private SkillData currentUseSkill;

    //当前攻击的目标
    private Transform currentSelectedTarget;

    //初始化
    public void Start()
    {
        mAnimator = GetComponent<Animator>();
        chSkillMgr = GetComponent<CharacterSkillManager>();
        chStatus = GetComponent<CharacterStatus>();
    }

    /// <summary>
    /// 使用指定技能
    /// </summary>
    /// <param name="skillid">技能编号</param>
    /// <param name="isBatter">是否连击</param>
    public void AttackUseSkill(int skillid, bool isBatter = false)
    {
        //如果是连击，找当前技能的下一个连击技能
        if (currentUseSkill != null && isBatter)
            skillid = currentUseSkill.skill.nextBatterId;
        //准备技能
        currentUseSkill = chSkillMgr.PrepareSkill(skillid);
        if (currentUseSkill != null)
        {
            //选中释放技能调用
            if ((currentUseSkill.skill.damageType & DamageType.Select) == DamageType.Select)
            {
                var selectedTaget = SelectTarget();
                if (currentUseSkill.skill.attckTargetTags.Contains("Player"))
                    selectedTaget = gameObject;

                if (selectedTaget != null)
                {
                    CharacterStatus selectStatus = null;
                    //修改成获取characterStatus中的Selected节点设置隐藏；
                    if (currentSelectedTarget != null)
                    {
                        selectStatus = currentSelectedTarget.GetComponent<CharacterStatus>();
                        selectStatus.selected.SetActive(false);
                    }
                    currentSelectedTarget = selectedTaget.transform;
                    selectStatus = currentSelectedTarget.GetComponent<CharacterStatus>();
                    selectStatus.selected.SetActive(true);
                    
                    //buff技能
                    if ((currentUseSkill.skill.damageType & DamageType.Buff) == DamageType.Buff)
                    {
                        foreach (var buff in currentUseSkill.skill.buffType)
                        {
                            //加bufficon
                            //英雄不需要隐藏
                            if (!selectedTaget.CompareTag("Player"))
                            {
                                MonsterMgr.I.HideAllEnemyPortraits();
                                selectStatus.uiPortrait.ShowPortrait();
                            }

                            //uiPortrait.transform.SetAsLastSibling();
                            selectStatus.uiPortrait.AddBuffIcon(buff, currentUseSkill.skill.buffDuration);

                            //已有该buff刷新
                            bool exist = false;
                            var buffs = selectedTaget.GetComponents<BuffRun>();
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
                            var buffRun = selectedTaget.AddComponent<BuffRun>();
                            buffRun.InitBuff(buff, currentUseSkill.skill.buffDuration,
                                currentUseSkill.skill.buffValue, currentUseSkill.skill.buffInterval);
                        }
						return;
                    }

                    //转向目标
                    //transform.LookAt(currentSelectedTarget);
                    chSkillMgr.DeploySkill(currentUseSkill);
                    mAnimator.Play(currentUseSkill.skill.animtionName);
                }
            }
            else
            {
                chSkillMgr.DeploySkill(currentUseSkill);
                mAnimator.Play(currentUseSkill.skill.animtionName);
            }
        }
    }

    /// <summary>
    /// 随机选择技能
    /// </summary>
    public void RandomSelectSkill()
    {
        if (chSkillMgr.skills.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, chSkillMgr.skills.Count);
            currentUseSkill = chSkillMgr.PrepareSkill(chSkillMgr.skills[index].skill.skillID);
            if (currentUseSkill == null) //随机技能未找到或未冷却结束
                currentUseSkill = chSkillMgr.skills[0]; //用技能表中第一个（默认技能）做补充
        }
    }

    //选择目标
    private GameObject SelectTarget()
    {
        //发一个球形射线，找出所有碰撞体
        var colliders = Physics.OverlapSphere(transform.position, currentUseSkill.skill.attackDisntance);
        if (colliders == null || colliders.Length == 0) return null;

        //从碰撞体列表中挑出所有的敌人
        String[] attTags = currentUseSkill.skill.attckTargetTags;
        var array = CollectionHelper.Select<Collider, GameObject>(colliders, p => p.gameObject);
       
        //正前方，tag正确，血量大于0的敌人
        array = CollectionHelper.FindAll<GameObject>(array,
            p => Array.IndexOf(attTags, p.tag) >= 0
                 && p.GetComponent<CharacterStatus>().HP > 0 &&
                 Vector3.Angle(transform.forward, p.transform.position - transform.position) <= 90);

        if (array == null || array.Length == 0) return null;

        //将所有的敌人，按与技能的发出者之间的距离升序排列，
        CollectionHelper.OrderBy<GameObject, float>(array,
            p => Vector3.Distance(transform.position, p.transform.position));
        return array[0];
    }
}