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
        //mAnimator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            css.AttackUseSkill(1);
            //mAnimator.SetInteger("Attack", 1);
        }
    }

    public void OnAttackEnd()
    {
        mAnimator.SetInteger("Attack", 0);
    }

    
}