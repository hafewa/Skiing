using UnityEngine;

public class UnityInstanceBase<T> : MonoBehaviour where T : UnityInstanceBase<T>
{
    static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject(typeof(T) + "(Instance)");
                instance = go.AddComponent<T>();
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        instance = this as T;
    }
}