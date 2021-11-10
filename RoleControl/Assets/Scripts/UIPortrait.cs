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
    void Start()
    {
        silderHP.maxValue = 1;
        silderMP.maxValue = 1;
    }

    private void OnEnable()
    {
        curTime = 0;
    }

    private void Update()
    {
        curTime += Time.deltaTime;
        if (curTime > 30.0f)
        {
            gameObject.SetActive(false);
        }
    }
    
    public void RefreshHpMp()
    {
        silderHP.value = (cstatus.HP / cstatus.MaxHP);
        silderMP.value = (cstatus.SP / cstatus.MaxSP);
    }
 
    public void AddBuffIcon(BuffRun buffRun)
    {
        BuffIcon curBuff = null;
        foreach (var item in buffItems)
        {
            if (item.activeSelf)
            { 
                curBuff = item.GetComponent<BuffIcon>();
                if (curBuff.buffRun.bufftype == buffRun.bufftype)
                {
                    curBuff.Refresh();
                    return;
                }
            }
        }

        curBuff = GetChild().GetComponent<BuffIcon>();
        curBuff.buffRun = buffRun;
        curBuff.LoadIcon();
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
}
