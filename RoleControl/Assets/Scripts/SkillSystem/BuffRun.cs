using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BuffRun : MonoBehaviour
{
    private float durationTime;
    public BuffType bufftype;
    private float value;            //伤害或者加成

    private float interval;

    private float attackTimer;

    private CharacterStatus target;
    
    public void InitBuff(BuffType buffType,float duration,float value,float interval)
    {
        bufftype = buffType;
        durationTime = duration;
        this.value = value;
        this.interval = interval;
    }

    public void Reset()
    {
        attackTimer = 0;
    }

    void Start()
    {
        target = GetComponent<CharacterStatus>();
        StartCoroutine(ExcuteDamage());
    }
    

    private IEnumerator ExcuteDamage()
    {
        attackTimer = 0; //已持续攻击的时间

        do
        {
            //对敌人的影响
            TargetImpact();
            
            yield return new WaitForSeconds(interval);
            attackTimer += interval;
            //做伤害数值的计算
        } while (durationTime > attackTimer);
        
        Destroy(this);
    }

    private void TargetImpact()
    {
        Transform fxPosTf = target.HitFxPos;
        
        if(bufftype == BuffType.Burn || bufftype == BuffType.Poison || bufftype == BuffType.Light) 
            target.OnDamage(value,null);
        else if (bufftype == BuffType.Slow)//减速
            fxPosTf = target.transform;
        else if (bufftype == BuffType.BeatBack)
            target.transform.DOMove(target.transform.position - target.transform.forward * value,0.5f);
        else if (bufftype == BuffType.BeatUp)
            target.transform.DOMove(target.transform.position - Vector3.up * value,0.5f);
        else if (bufftype == BuffType.AddDefence)
        {
            fxPosTf = target.transform;
            target.defence += value;
        }
        else if (bufftype == BuffType.RecoverHp)
            target.OnDamage(-value,null);

        GameObject go = Resources.Load<GameObject>($"Skill/{buffFx[bufftype]}");
        GameObject buffGo = GameObjectPool.I.CreateObject(buffFx[bufftype], go, fxPosTf.position, fxPosTf.rotation);
        buffGo.transform.SetParent(fxPosTf);
        GameObjectPool.I.Destory(buffGo, interval);
    }

    private static Dictionary<BuffType, string> buffFx = new Dictionary<BuffType, string>();

    public static void InitAllBuff()
    {
        buffFx.Add(BuffType.Burn,"Skill_32_R_Fly_100");
        buffFx.Add(BuffType.Light,"Skill_75_Cast");
        buffFx.Add(BuffType.Slow,"Skill_21_R_Fly_100");
        buffFx.Add(BuffType.Poison,"Skill_12_R_Fly_100");
        buffFx.Add(BuffType.AddDefence,"FX_CHAR_Aura");
        buffFx.Add(BuffType.RecoverHp,"FX_Heal_Light_Cast");
    }

    /*Burn = 2,        
    Slow = 4,        
    Light = 8,       
    Stun = 16,       
    Poison = 32,     
    BeatBack = 64,   
    BeatUp = 128,    
    Pull = 256,      
    AddDefence = 512,
    RecoverHp = 1024,*/
}
