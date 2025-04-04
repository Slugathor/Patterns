using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSingleton<T> : MonoBehaviour where T : Component
{
    public static T instance { get; private set; }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            Debug.Log($"{this.gameObject.name} was destroyed because another instance of {this.name} already existed.\n");
            Destroy(gameObject);
        }
    }
}
