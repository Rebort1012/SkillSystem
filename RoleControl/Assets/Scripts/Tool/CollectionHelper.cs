using UnityEngine;
using System.Collections.Generic;
using System;

//集合或数组的助手类
public static class CollectionHelper
{
    //升序排列
    public static void OrderBy<T, TKey>(T[] array, SelectHandler<T, TKey> handler)
        where TKey : IComparable<TKey>
    {
        for (int i = 0; i < array.Length - 1; i++)
        for (int j = i + 1; j < array.Length; j++)
            if (handler(array[i]).CompareTo(handler(array[j])) > 0)
            {
                var temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
    }

    //降序排列
    public static void OrderByDescending<T, TKey>(T[] array, SelectHandler<T, TKey> handler)
        where TKey : IComparable
    {
        for (int i = 0; i < array.Length - 1; i++)
        for (int j = i + 1; j < array.Length; j++)
            if (handler(array[i]).CompareTo(handler(array[j])) < 0)
            {
                var temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
    }

    public delegate bool FindHandler<T>(T item);

    public delegate TKey SelectHandler<TSource, TKey>(TSource source);

    //查找
    public static T Find<T>(T[] array, FindHandler<T> handler)
    {
        foreach (var item in array)
        {
            //调用委托
            if (handler(item))
                return item;
        }

        return default(T);
    }

    //查找
    public static T[] FindAll<T>(T[] array, FindHandler<T> handler)
    {
        List<T> tempList = new List<T>();
        foreach (var item in array)
        {
            //调用委托
            if (handler(item))
                tempList.Add(item);
        }

        return tempList.Count > 0 ? tempList.ToArray() : null;
    }

    public static TKey[] Select<T, TKey>(T[] array, SelectHandler<T, TKey> handler)
    {
        TKey[] tempArr = new TKey[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            tempArr[i] = handler(array[i]);
        }

        return tempArr;
    }
}