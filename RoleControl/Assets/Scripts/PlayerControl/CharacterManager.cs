using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    //加载技能数据，英雄数据等   
    private CharacterSkillSystem css;

    private void Awake()
    {
        BuffRun.InitAllBuff();
        SkillDeployer.InitBuffIconName();
    }

    public void Start()
    {
        css = GetComponent<CharacterSkillSystem>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
           css.AttackUseSkill(1);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            css.AttackUseSkill(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            css.AttackUseSkill(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            css.AttackUseSkill(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            css.AttackUseSkill(5);
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (Cursor.visible)
                Cursor.visible = false;
            else
                Cursor.visible = true;
        }
        
    }
}