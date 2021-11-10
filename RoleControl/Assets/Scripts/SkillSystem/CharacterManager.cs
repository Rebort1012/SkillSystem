using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    //加载技能数据，英雄数据等   
    private Animator mAnimator;
    private CharacterSkillSystem css;
    
    public void Start()
    {
        css = GetComponent<CharacterSkillSystem>();
        BuffRun.InitAllBuff();
        //mAnimator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
           css.AttackUseSkill(1);
          //mAnimator.SetInteger("Attack", 1);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            css.AttackUseSkill(2);
            //mAnimator.SetInteger("Attack", 1);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            css.AttackUseSkill(3);
            //mAnimator.SetInteger("Attack", 1);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            css.AttackUseSkill(4);
            //mAnimator.SetInteger("Attack", 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            css.AttackUseSkill(5);
            //mAnimator.SetInteger("Attack", 1);
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (Cursor.visible)
                Cursor.visible = false;
            else
                Cursor.visible = true;
        }
        
    }

    public void OnAttackEnd()
    {
        mAnimator.SetInteger("Attack", 0);
    }

    
}