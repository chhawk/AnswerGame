using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class ManagerResolver {
	
	private static Dictionary<Type, object> TypeDictionary = new Dictionary<Type, object>();
	
	public static void Register<T>(object obj) where T : class
	{
        if (!TypeDictionary.ContainsKey(typeof(T)))
        {
            TypeDictionary.Add(typeof(T), obj);
        }
        else
        {
            TypeDictionary[typeof(T)] = obj;
        }
	}
	
	public static T Resolve<T>() where T : class
	{
		if (!TypeDictionary.ContainsKey (typeof(T)))
			return null;

		return TypeDictionary[typeof(T)] as T;
	}

}

class Utility
{
    public static T[] RandomSort<T>(T[] array)
    {
        int len = array.Length;

        System.Random rand = new System.Random();
        System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
        T[] ret = new T[len];
        int i = 0;
        while (list.Count < len)
        {
            int iter = rand.Next(0, len);
            if (!list.Contains(iter))
            {
                list.Add(iter);
                ret[i] = array[iter];
                i++;
            }
        }
        return ret;
    }
}
