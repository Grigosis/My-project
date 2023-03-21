using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                    Debug.LogError($"Instance of {typeof(T)} not found");
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
            _instance = (T)this;
        else
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _instance = null;
    }
}
