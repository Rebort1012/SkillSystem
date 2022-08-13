using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// 技能管理类
/// </summary>

[RequireComponent(typeof(CharacterSkillSystem))]
public class CharacterSkillManager : MonoBehaviour
{
    /// <summary>管理所有技能的容器</summary>
    public List<SkillData> skills = new List<SkillData>();

    /// <summary>技能的拥有者</summary>
    private CharacterStatus chStatus = null;

    private SkillData curSkill;

    private void AddSkill(string path)
    {
        SkillTemp skTemp = Instantiate(Resources.Load<SkillTemp>(path));
        Skill sk = LoadSkill(skTemp);;
        SkillData skd = new SkillData();
        skd.skill = sk;
        skills.Add(skd);
    }

    //初始化技能数据(有什么技能)
    public void Start()
    {
        chStatus = GetComponent<CharacterStatus>();

        AddSkill("Skill_1");
        AddSkill("Skill_2");
        AddSkill("Skill_3");
        AddSkill("Skill_4");
        AddSkill("Skill_5");
        
        foreach (var item in skills)
        {
            //动态加载技能特效预制体  //Resources/Skill -- 技能特效预制体 
            if (item.skillPrefab == null && !string.IsNullOrEmpty(item.skill.prefabName))
                item.skillPrefab = LoadFxPrefab("Skill/" + item.skill.prefabName);
            
            //Resources/Skill/HitFx     技能伤害特效预制体
            if (item.hitFxPrefab == null && !string.IsNullOrEmpty(item.skill.hitFxName))
                item.hitFxPrefab = LoadFxPrefab("Skill/" + item.skill.hitFxName);
        }
    }

    //将特效预制件载入到对象池，以备将来使用
    private GameObject LoadFxPrefab(string path)
    {
        var key = path.Substring(path.LastIndexOf("/") + 1);
        var go = Resources.Load<GameObject>(path);
        GameObjectPool.I.Destory(
            GameObjectPool.I.CreateObject(
                key, go, transform.position, transform.rotation)
        );
        return go;
    }

    //准备技能
    public SkillData PrepareSkill(int id)
    {
        //从技能容器中找出相应ID的技能
        var skillData = skills.Find(p => p.skill.skillID == id);
        if (skillData != null && //查找到技能
            chStatus.SP >= skillData.skill.costSP && //检查角色SP是否够使用该技能
            skillData.coolRemain == 0) //且该技能已经冷却结束
        {
            skillData.Owner = gameObject;
            return skillData;
        }

        return null;
    }

    //释放技能
    public void DeploySkill(SkillData skillData)
    {
        //开始冷却计时
        StartCoroutine(CoolTimeDown(skillData));

        //动画某一帧触发技能特效，这里写一个延迟调用的方法，使用动画时间的百分解决特效释放时间问题
        if (skillData.skill.delayAnimaTime != 0)
        {
            curSkill = skillData;
            Invoke("DelayDeploySkill", skillData.skill.delayAnimaTime);
            return;
        }
        
        GameObject tempGo = null;
        //创建技能预制体+创建位置的偏移
        if ((skillData.skill.damageType & DamageType.FxOffset) == DamageType.FxOffset)
            tempGo = GameObjectPool.I.CreateObject(skillData.skill.prefabName, skillData.skillPrefab,
                transform.position + transform.forward * skillData.skill.fxOffset, transform.rotation);
       
        else if ((skillData.skill.damageType & DamageType.FirePos) == DamageType.FirePos)
            tempGo = GameObjectPool.I.CreateObject(skillData.skill.prefabName, skillData.skillPrefab,
                chStatus.FirePos.position, chStatus.FirePos.rotation);

        if(tempGo == null)
            return;

        //从预制体对象上找到技能释放对象 
        var deployer = tempGo.GetComponent<SkillDeployer>();
        if (deployer == null)
            deployer = tempGo.AddComponent<SkillDeployer>();

        //设置要释放的技能
        deployer.skillData = skillData;
        //调用释放方法
        deployer.DeploySkill();
        
        //技能持续时间过后，技能要销毁
        if ((skillData.skill.damageType & DamageType.Bullet) != DamageType.Bullet)
        {
            if (skillData.skill.durationTime > 0)
                GameObjectPool.I.Destory(tempGo, skillData.skill.durationTime);
            else
                GameObjectPool.I.Destory(tempGo, 0.5f);
        }
    }

    private void DelayDeploySkill()
    {
        GameObject tempGo = null;
        //创建技能预制体+创建位置的偏移
        if ((curSkill.skill.damageType & DamageType.FxOffset) == DamageType.FxOffset)
            tempGo = GameObjectPool.I.CreateObject(curSkill.skill.prefabName, curSkill.skillPrefab,
                transform.position + transform.forward * curSkill.skill.fxOffset, transform.rotation);
        
        else if ((curSkill.skill.damageType & DamageType.FirePos) == DamageType.FirePos)
            tempGo = GameObjectPool.I.CreateObject(curSkill.skill.prefabName, curSkill.skillPrefab,
                chStatus.FirePos.position, chStatus.FirePos.rotation);

        //从预制体对象上找到技能释放对象 
        var deployer = tempGo.GetComponent<SkillDeployer>();
        if (deployer == null)
            deployer = tempGo.AddComponent<SkillDeployer>();

        //设置要释放的技能
        deployer.skillData = curSkill;
        //调用释放方法
        deployer.DeploySkill();

        //技能持续时间过后，技能要销毁
        if ((curSkill.skill.damageType & DamageType.Bullet) != DamageType.Bullet)
        {
            if (curSkill.skill.durationTime > 0)
                GameObjectPool.I.Destory(tempGo, curSkill.skill.durationTime);
            else
                GameObjectPool.I.Destory(tempGo, 0.5f);
        }
    }

    //冷却时间倒计时
    public IEnumerator CoolTimeDown(SkillData skillData)
    {
        skillData.coolRemain = skillData.skill.coolTime;
        while (skillData.coolRemain > 0)
        {
            yield return new WaitForSeconds(0.1f);
            skillData.coolRemain -= 0.1f;
        }

        skillData.coolRemain = 0;
    }

    //取得冷却倒计时的剩余时间(秒)
    public float GetSkillCoolRemain(int id)
    {
        return skills.Find(p => p.skill.skillID == id).coolRemain;
    }

    private Skill LoadSkill(SkillTemp skillTemp)
    {
        Skill sk = skillTemp.skill;
        int count = skillTemp.damageType.Length;
        for (int i = 0; i < count; ++i)
        {
            sk.damageType = sk.damageType | skillTemp.damageType[i];
        }
        return sk;
    }
}