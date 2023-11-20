using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GenericSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static bool Exists { get => m_Instance != null; }

    protected static T m_Instance;
    public static T Instance
    {
        get
        {
            // If there's no private instance...
            if (m_Instance == null)
            {
                // Search for it!
                m_Instance = FindObjectOfType<T>();


                // And if we don't find it,
                if (m_Instance == null)
                {
                    Debug.Log($"Creating new GenericSingleton of type {typeof(T).Name}!");
                    GameObject newObject = new()
                    {
                        name = "NEW" + typeof(T).Name
                    };
                    m_Instance = newObject.AddComponent<T>();
                    DontDestroyOnLoad(newObject);
                }
            }

            return m_Instance;
        }
    }

    protected void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (m_Instance != this)
        {
            Destroy(gameObject);
        }
    }

}
