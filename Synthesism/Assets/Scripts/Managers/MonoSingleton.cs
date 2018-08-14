using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic Singleton Implementation for a Monobehavior
/// </summary>
/// <typeparam name="T">Type of the Component</typeparam>
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{

	private static T _instance;

    public static T instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>(); //Look for component in scene

                if (_instance == null) //If there is no componenet in the scene create one
                {
                    Debug.Log("No " + typeof(T).Name + " exists in the scene. Creating one...");
                    createNewSingleton();
                }
            }
            return _instance;
        }
        private set { }
    }

    private static void createNewSingleton()
    {
        GameObject go = new GameObject(typeof(T).Name + "Manager");
        _instance = go.AddComponent<T>();
    }

    public virtual void Awake()
    {
        if(_instance == null)
        {
            _instance = this as T;

            //This will make the GameObject that the manager is attached to persist across scenes.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy the Manager because one already exists
            Debug.LogWarning("Attempt to create another " + typeof(T).Name + " was made. Destroyed it instead.");
            Destroy(gameObject);
        }
    }

	
}
