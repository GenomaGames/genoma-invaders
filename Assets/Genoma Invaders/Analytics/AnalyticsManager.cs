using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance
    {
        get;
        private set;
    }

    [SerializeField]
    private bool isPersistent = false;

    protected void Awake()
    {
        if (Instance == null)
        {
            if (isPersistent)
            {
                DontDestroyOnLoad(gameObject);
            }

            Instance = this as T;
        }
        else
        {
            Debug.Log($"{typeof(T)} already on scene, destroying conflicting {name}");
            Destroy(gameObject);
        }
    }
}

public class AnalyticsManager : SingletonMonoBehaviour<AnalyticsManager>
{
    public static void SendPlayerKilled()
    {
        if (Instance != null)
        {
            Debug.Log("Player Killed");
            AnalyticsService.Instance.CustomData("playerKilled", new Dictionary<string, object>());
        }
    }

    private new void Awake()
    {
        base.Awake();
    }

    private async void Start()
    {
        try
        {
            InitializationOptions initOptions = new();

#if DEVELOPMENT_BUILD || UNITY_EDITOR
            initOptions.SetEnvironmentName("development");
#else
            initOptions.SetEnvironmentName("production");
#endif

            await UnityServices.InitializeAsync(initOptions);

            List<string> contentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();

            foreach (string contentIdentifier in contentIdentifiers)
            {
                Debug.Log($"Consent Identifier: {contentIdentifier}");
            }

            Debug.Log($"Analytics user ID: {AnalyticsService.Instance.GetAnalyticsUserID()}");
        }
        catch (ConsentCheckException exception)
        {
            throw exception;
        }
    }
}

public class SerializedSingletonMonoBehaviour<T> : SerializedMonoBehaviour
{
    public static SerializedSingletonMonoBehaviour<T> Instance
    {
        get;
        private set;
    }

    [SerializeField]
    private bool isPersistent = false;

    protected void Awake()
    {
        if (Instance == null)
        {
            if (isPersistent)
            {
                DontDestroyOnLoad(gameObject);
            }

            Instance = this;
        }
        else
        {
            Debug.Log($"{typeof(T)} already on scene, destroying conflicting {name}");
            Destroy(gameObject);
        }
    }
}