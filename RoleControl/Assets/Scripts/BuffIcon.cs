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
    [HideInInspector]
    public BuffRun buffRun = new BuffRun();

    private static Dictionary<BuffType, string> buffIconName = new Dictionary<BuffType, string>();
    
    public static void InitBuffIconName()
    {
        buffIconName.Add(BuffType.Burn,"Buff_0");
        buffIconName.Add(BuffType.Light,"Buff_9");
        buffIconName.Add(BuffType.Slow,"Buff_1");
        buffIconName.Add(BuffType.Stun,"Buff_2");
        buffIconName.Add(BuffType.Poison,"Buff_3");
        buffIconName.Add(BuffType.BeatBack,"Buff_4");
        buffIconName.Add(BuffType.BeatUp,"Buff_5");
        buffIconName.Add(BuffType.Pull,"Buff_6");
        buffIconName.Add(BuffType.AddDefence,"Buff_7");
        buffIconName.Add(BuffType.RecoverHp,"Buff_8");
    }

    public void LoadIcon()
    {
        Sprite[] temp = Resources.LoadAll<Sprite>("BuffIcon/Buff");
        Debug.Log(temp);
        if (temp != null)
        {
            foreach (var sp in temp)
            {
                if (sp.name == buffIconName[buffRun.bufftype])
                {
                    imgIcon.sprite = Instantiate(sp);
                }
            }
        }
    }

    void Update()
    {
        textCD.text = buffRun.GetRemainTime().ToString("F0");
        
        if(textCD.text == "0")
            gameObject.SetActive(false);
    }

    public void Refresh()
    {
        Debug.Log("已有buff刷新持续时间");
    }
}
