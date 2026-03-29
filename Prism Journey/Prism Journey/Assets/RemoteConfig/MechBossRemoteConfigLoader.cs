using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class MechBossRemoteConfigLoader : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BossContext bossContext;
    [SerializeField] private MechBossLocalConfig localConfig;

    [Header("Remote")]
    [SerializeField] private bool loadRemoteOnStart = true;

    [TextArea(2, 5)]
    [SerializeField] private string remoteJsonUrl ="https://raw.githubusercontent.com/YOUR_USERNAME/YOUR_REPO/main/boss_config.json";

    [Header("Debug")]
    [SerializeField] private bool printLogs = true;

    private void Awake()
    {
        if (bossContext == null)
            bossContext = GetComponent<BossContext>();
    }

    private void Start()
    {
        LoadLocalFirst();

        if (loadRemoteOnStart)
        {
            StartCoroutine(LoadRemoteConfigCoroutine());
        }
    }

    private void LoadLocalFirst()
    {
        if (bossContext == null)
        {
            Debug.LogError("[BossRemoteConfigLoader] BossContext is missing.");
            return;
        }

        if (localConfig == null)
        {
            Debug.LogWarning("[BossRemoteConfigLoader] Local config is missing. Boss will use current Inspector values.");
            return;
        }

        bossContext.ApplyLocalConfig(localConfig);

        if (printLogs)
            Debug.Log("[BossRemoteConfigLoader] Local config loaded first.");
    }



    private IEnumerator LoadRemoteConfigCoroutine()
    {
        if (string.IsNullOrWhiteSpace(remoteJsonUrl))
        {
            Debug.LogWarning("[BossRemoteConfigLoader] Remote URL is empty. Skip remote loading.");
            yield break;
        }

        if (printLogs)
            Debug.Log($"[BossRemoteConfigLoader] Trying remote config: {remoteJsonUrl}");

        using (UnityWebRequest request = UnityWebRequest.Get(remoteJsonUrl))
        {
            yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            bool failed = request.result != UnityWebRequest.Result.Success;
#else
            bool failed = request.isNetworkError || request.isHttpError;
#endif

            if (failed)
            {
                Debug.LogWarning($"[BossRemoteConfigLoader] Remote load failed: {request.error}");
                Debug.Log("[BossRemoteConfigLoader] Keep using local config.");
                yield break;
            }

            string json = request.downloadHandler.text;

            if (printLogs)
            {
                Debug.Log("[BossRemoteConfigLoader] Remote JSON downloaded:");
                Debug.Log(json);
            }

            MechBossRemoteConfigData remoteData = null;

            try
            {
                remoteData = JsonUtility.FromJson<MechBossRemoteConfigData>(json);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[BossRemoteConfigLoader] JSON parse failed: {e.Message}");
                yield break;
            }

            if (remoteData == null)
            {
                Debug.LogError("[BossRemoteConfigLoader] Parsed remote config is null.");
                yield break;
            }

            bossContext.ApplyRemoteConfig(remoteData);

            if (printLogs)
                Debug.Log("[BossRemoteConfigLoader] Remote config applied successfully.");
        }
    }

}
