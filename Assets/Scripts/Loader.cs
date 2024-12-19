
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    // Создаем новый объект, если он не найден
                    GameObject singletonObject = new GameObject($"{typeof(T).Name} (Singleton)");
                    instance = singletonObject.AddComponent<T>();
                }

                // Убеждаемся, что объект в корневой иерархии
                if (instance.transform.parent != null)
                {
                    instance.transform.parent = null;
                }

                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;

            // Убеждаемся, что объект в корневой иерархии
            if (transform.parent != null)
            {
                transform.parent = null;
            }

            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
