using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;

public class BuffRun : MonoBehaviour
{
    private float durationTime;
    public BuffType bufftype;
    private float value;            //伤害或者加成

    private float interval;

    private float attackTimer;

    private float curTime;

    private CharacterStatus target;
    
    public void InitBuff(BuffType buffType,float duration,float value,float interval)
    {
        bufftype = buffType;
        
        if (buffType == BuffType.BeatBack || buffType == BuffType.BeatUp || buffType == BuffType.Pull)
            duration = 2f;
        
        durationTime = duration;
        this.value = value;
        this.interval = interval;
        curTime = 0;
    }

    public void Reset()
    {
        attackTimer = 0;
        curTime = 0;
    }

    void Start()
    {
        curTime = 0;
        target = GetComponent<CharacterStatus>();
        StartCoroutine(ExcuteDamage());
    }

    private void Update()
    {
        curTime += Time.deltaTime;
        
        if(curTime > durationTime)
            Destroy(this);
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

        if (bufftype == BuffType.Burn || bufftype == BuffType.Poison || bufftype == BuffType.Light)
            target.OnDamage(value, gameObject, true);
        else if (bufftype == BuffType.Slow)//减速
            fxPosTf = target.transform;
        else if (bufftype == BuffType.BeatBack)
        {
            Vector3 dir = -target.transform.position + GameObject.FindGameObjectWithTag("Player").transform.position;
            dir.y = 0;
            target.transform.DOMove(target.transform.position - dir.normalized * value,0.5f);
            durationTime = 2f;
        }
        else if (bufftype == BuffType.BeatUp)
        {
            target.transform.DOMove(target.transform.position - Vector3.up * value,0.5f);
            durationTime = 2f;
        }
        else if (bufftype == BuffType.AddDefence)
        {
            fxPosTf = target.transform;
            target.defence += value;
        }
        else if (bufftype == BuffType.RecoverHp)
        {
            target.OnDamage(-value, gameObject, true);
        }

        if (buffFx.ContainsKey(bufftype))
        {
            GameObject go = Resources.Load<GameObject>($"Skill/{buffFx[bufftype]}");
            GameObject buffGo = GameObjectPool.I.CreateObject(buffFx[bufftype], go, fxPosTf.position, fxPosTf.rotation);
            buffGo.transform.SetParent(fxPosTf);
            GameObjectPool.I.Destory(buffGo, interval);
        }
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

    public float GetRemainTime()
    {
        return durationTime - curTime;
    }
}
