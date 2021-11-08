using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMFX_PlayerController : MonoBehaviour
{
    public void PlayerAnimation() 
    {
        Animator anim;
        anim = GetComponent<Animator>();
        anim.SetTrigger("Active");
    }
}