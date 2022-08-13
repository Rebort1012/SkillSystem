using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMgr
{
    private static MonsterMgr instance;
    private Vector2 hidePos = new Vector2(1600, 444);

    private MonsterMgr(){}
    public static MonsterMgr I {
        get
        {
            if (instance == null)
                instance = new MonsterMgr();
            return instance;
        }
    }

    private List<UIPortrait> allEnemyPortraits = new List<UIPortrait>();

    public void AddEnemyPortraits(UIPortrait uiPortrait)
    {
        allEnemyPortraits.Add(uiPortrait);
    }
    
    public void RemoveEnemyPortraits(UIPortrait uiPortrait)
    {
        allEnemyPortraits.Remove(uiPortrait);
    }

    public void HideAllEnemyPortraits()
    {
        foreach (var it in allEnemyPortraits)
        {
            it.GetComponent<RectTransform>().anchoredPosition = hidePos;
        }
    }
}
