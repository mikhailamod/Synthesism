using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic Singleton Implementation for a Monobehavior
/// </summary>
/// <typeparam name="T">Type of the Component</typeparam>
public class MonoSingleton<T> : MonoBehaviour
{

	private static T _instance;

    public static T instance
    {
        get
        {
            if(_instance == null)
            {
                
            }
            //Add some component creation here
            //Dont destroy on load
            return _instance;
        }
        private set { }
    }

	
}
