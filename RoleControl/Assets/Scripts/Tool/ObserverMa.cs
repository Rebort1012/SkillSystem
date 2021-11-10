using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsKey
{
    public const string refreshPortrait = "1";
}

public delegate void obsAct(object args);

public class ObserverMa : SingletonMono<ObserverMa>
{
    //有参数
    private Dictionary<string, List<obsAct>> dicAll = new Dictionary<string, List<obsAct>>();//存储所有事件和响应
    private List<string> curAct = new List<string>();//存储当前帧注册的事件key值
    private ArrayList objArgs = new ArrayList();	 //存储key对应的参数
    //无参数
    private Dictionary<string, List<Action>> dicActAll = new Dictionary<string, List<Action>>();
    private List<string> curAct2 = new List<string>();

    public void Register(string key, Action act)
    {
        if (dicActAll.ContainsKey(key))
        {
            dicActAll[key].Add(act);
        }
        else
        {
            List<Action> actions = new List<Action>();
            actions.Add(act);
            dicActAll.Add(key, actions);
        }
    }

    public void Register(string key, obsAct act)
    {
        if (dicAll.ContainsKey(key))
        {
            dicAll[key].Add(act);
        }
        else
        {
            List<obsAct> actions = new List<obsAct>();
            actions.Add(act);
            dicAll.Add(key, actions);
        }
    }

    public void Notify(string key)
    {
        if (dicActAll.ContainsKey(key))
        {
            curAct2.Add(key);
        }
        else
        {
            Debug.Log("未注册观察事件");
        }
    }

    public void Notify(string key, object args)
    {
        if (dicAll.ContainsKey(key))
        {
            curAct.Add(key);
            objArgs.Add(args);
        }
        else
        {
            Debug.Log("未注册观察事件");
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < curAct.Count; ++i)
        {
            List<obsAct> acts = dicAll[curAct[i]];
            for (int j = 0; j < acts.Count; ++j)
            {
                acts[j](objArgs[i]);
            }
        }

        for (int i = 0; i < curAct2.Count; ++i)
        {
            List<Action> acts = dicActAll[curAct2[i]];
            for (int j = 0; j < acts.Count; ++j)
            {
                acts[j]();
            }
        }
        
        curAct2.Clear();
        curAct.Clear();
        objArgs.Clear();
    }
}
