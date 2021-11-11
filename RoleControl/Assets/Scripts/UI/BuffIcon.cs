using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class BuffIcon : MonoBehaviour
{
    public Text textCD;
    public Image imgIcon;
    
    private float durationTime;
    private float curTime;

    public BuffType buffType;

    public void LoadIcon(BuffType buffType, float duration)
    {
        durationTime = duration;
        this.buffType = buffType;
        Sprite[] temp = Resources.LoadAll<Sprite>("BuffIcon/Buff");
        if (temp != null)
        {
            foreach (var sp in temp)
            {
                if (sp.name == SkillDeployer.buffIconName[buffType])
                {
                    imgIcon.sprite = Instantiate(sp);
                }
            }
        }
    }

    private void OnEnable()
    {
        curTime = 0;
    }

    void Update()
    {
        curTime += Time.deltaTime;
        
        textCD.text = (durationTime - curTime).ToString("F0");

        if (curTime > durationTime)
        {
            gameObject.SetActive(false);
            curTime = 0;
        }
    }

    public void Refresh()
    {
        //Debug.Log("已有buff刷新持续时间");
        curTime = 0;
    }
}
