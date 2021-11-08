using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    //加载技能数据，英雄数据等   
    private Animator mAnimator;
    
    public void Start()
    {
        mAnimator = GetComponent<Animator>();
    }

    public void Update()
    {
        if(Input.GetAxis("Fire1")>0)
            mAnimator.SetInteger("Attack",1);
    }

    public void OnAttackEnd()
    {
        mAnimator.SetInteger("Attack",0);
    }
}