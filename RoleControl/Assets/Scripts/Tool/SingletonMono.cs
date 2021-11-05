using System;
using System.Reflection;
using UnityEngine;

/// <summary>
/// MonoBehaviour子类 一个抽象单列类
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingletonMono<T> :MonoBehaviour where T: SingletonMono<T>
{
    private static T instance = null;

    public static T I
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (FindObjectsOfType<T>().Length > 1)
                {
                    Debug.Log("More than 1!");
                }

                if (instance == null)
                {
                    string instanceName = typeof(T).Name;
                    Debug.Log("Instance Name: " + instanceName); 
                    GameObject instanceGO = GameObject.Find(instanceName);
                    
                    if (instanceGO == null)
                        instanceGO = new GameObject(instanceName);
                    
                    instance = instanceGO.AddComponent<T>();
                    DontDestroyOnLoad(instanceGO);  //保证实例不会被释放
                    
                    Debug.Log("Add New Singleton " + instance.name + " in Game!");
                }
                else
                {
                    Debug.Log("Already exist: " + instance.name);
                }
            }

            return instance;
        }
    }

    protected virtual void OnDestroy()
    {
        instance = null;
    }
}