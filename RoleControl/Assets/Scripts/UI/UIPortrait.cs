using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class UIPortrait : MonoBehaviour
{
    public Transform buffContent;
    public Slider silderHP;
    public Slider silderMP;
    public GameObject buffItem;
    [HideInInspector]
    public CharacterStatus cstatus;

    private List<GameObject> buffItems = new List<GameObject>();
    private float curTime;
    private Vector2 defaultPos;
    private Vector2 hidePos = new Vector2(1600,444);
    
    void Start()
    {
        defaultPos = GetComponent<RectTransform>().anchoredPosition;
        if (gameObject.name != "UIHeroPortrait")
            GetComponent<RectTransform>().anchoredPosition = hidePos;
        
        silderHP.maxValue = 1;
        silderMP.maxValue = 1;
    }

    private void OnEnable()
    {
        curTime = 0;
    }

    private void Update()
    {
        if (gameObject.name == "UIHeroPortrait")
            return;
        
        curTime += Time.deltaTime;
        
        if (curTime > 40.0f)
        {
            GetComponent<RectTransform>().anchoredPosition = hidePos;
        }
    }
    
    public void RefreshHpMp()
    {
        silderHP.value = (cstatus.HP / cstatus.MaxHP);
        silderMP.value = (cstatus.SP / cstatus.MaxSP);
    }
 
    public void AddBuffIcon(BuffType buffType,float druation)
    {
        curTime = 0;
        BuffIcon curBuff = null;
        foreach (var item in buffItems)
        {
            if (item.activeSelf)
            { 
                curBuff = item.GetComponent<BuffIcon>();
                if (curBuff.buffType == buffType)
                {
                    curBuff.Refresh();
                    return;
                }
            }
        }

        if (buffType == BuffType.BeatBack || buffType == BuffType.BeatUp || buffType == BuffType.Pull)
            druation = 2f;

        GameObject go = GetChild();
        buffItems.Add(go);
        curBuff = go.GetComponent<BuffIcon>();
        curBuff.LoadIcon(buffType, druation);
    }

    private GameObject GetChild()
    {
        foreach (var item in buffItems)
        {
            if (!item.activeSelf)
            {
                item.SetActive(true);
                return item;
            }
        }
       
        GameObject go = Instantiate<GameObject>(buffItem, buffContent);
        return go;
    }

    public void Reset()
    {
        curTime = 0;
    }

    public void ShowPortrait()
    {
        GetComponent<RectTransform>().anchoredPosition = defaultPos;
    }
}
