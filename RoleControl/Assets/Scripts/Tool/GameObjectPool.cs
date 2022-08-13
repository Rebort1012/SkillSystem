using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Text;

//对象池
public class GameObjectPool : SingletonMono<GameObjectPool>
{
    /// <summary>可能存放多个种类的对象，每个种类有多个对象 </summary>
    private Dictionary<string, List<GameObject>> cache = new Dictionary<string, List<GameObject>>();

    GameObjectPool()
    { }

    /// <summary>增加物体进入池(按类别增加)</summary>
    public void Add(string key, GameObject go)
    {
        //1.如果key在容器中存在，则将go加入对应的列表
        //2.如果key在容器中不存在，是先创建一个列表，再加入
        if (!cache.ContainsKey(key))
            cache.Add(key, new List<GameObject>());
        cache[key].Add(go);
    }

    /// <summary>销毁物体(将对象隐藏)</summary>
    public void Destory(GameObject destoryGo)
    {
        //设置destoryGo隐藏
        destoryGo.SetActive(false);
    }

    /// <summary>将对象归入池中<summary>
    public void Destory(GameObject tempGo, float delay)
    {
        //开启一个协程
        StartCoroutine(DelayDestory(tempGo, delay));
    }

    /// <summary>延迟销毁</summary>
    private IEnumerator DelayDestory(GameObject destoryGO, float delay)
    {
        //等待一个延迟的时间
        yield return new WaitForSeconds(delay);
        Destory(destoryGO);
    }

    /// <summary>取出可用的物体（已经隐藏的）</summary>
    private GameObject FindUsable(string key)
    {
        //1.在容器中找出key对应的列表，从列表中找出已经为隐藏状态的对象，返回
        if (cache.ContainsKey(key))
        {
            foreach (GameObject item in cache[key])
            {
                if (!item.activeSelf)
                   return item;
            }
        }
        return null;
    }

    /// <summary>创建一个游戏物体到场景 </summary>
    public GameObject CreateObject(string key, GameObject go, Vector3 position, Quaternion quaternion)
    {
        //先找是否有可用的，如果没有则创建，如果有找到后设置好位置，朝向再返回
        GameObject tempGo = FindUsable(key);
        if (tempGo != null)
        {
            tempGo.transform.position = position;
            tempGo.transform.rotation = quaternion;
            tempGo.SetActive(true);
        }
        else
        {
            tempGo = GameObject.Instantiate(go, position, quaternion) as GameObject;
            Add(key, tempGo);
        }
        
        return tempGo;
    }

    /// <summary>清空某类游戏对象</summary>
    public void Clear(string key)
    {
        cache.Remove(key);
    }

    /// <summary>清空池中所有游戏对象</summary>
    public void ClearAll()
    {
        cache.Clear();
    }

    private GameObject GetTemplate(string path)
    {
        int index = 0;
        bool isResource = false;

        string[] tempStr = path.Split('/');
        for (int i = 0; i < tempStr.Length; ++i)
        {
            if (tempStr[i] == "Resources")
            {
                index = i;
                isResource = true;
            }
        }

        StringBuilder resPath = new StringBuilder("");
        for (int i = index + 1; i < tempStr.Length; ++i)
            resPath.Append($"{tempStr[i]}/");

        resPath.Remove(resPath.Length - 1, 1);

        if (isResource)
        {
            path = resPath.ToString().Split('.')[0];
        }

        return Resources.Load<GameObject>(path);
    }
}