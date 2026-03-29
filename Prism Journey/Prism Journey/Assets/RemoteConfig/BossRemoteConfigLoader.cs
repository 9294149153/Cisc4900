using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class BossRemoteConfigLoader : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BossContext bossContext;
    [SerializeField] private MechBossLocalConfigSO localConfig;

    [Header("Remote")]
    [TextArea(2, 5)]
    [SerializeField] private string remoteJsonUrl;

    [Header("Debug")]
    [SerializeField] private bool printLogs = true;

    private void Start()
    {
        //Apply local config to the boss before try to apply remote config
        if (bossContext == null)
        {
            Debug.LogError("[BossRemoteConfigLoader] bossContext is missing.");
            return;
        }

        if (localConfig == null)
        {
            Debug.LogError("[BossRemoteConfigLoader] localConfig is missing.");
            return;
        }

        ApplyLocalConfigFirst();
        //////////////////////////////////////////////////

        //try apply remote config
        if (localConfig.useRemoteConfig)
        {
            StartCoroutine(LoadRemoteConfigCoroutine());
        }
        else
        {
            if (printLogs)
                Debug.Log("[BossRemoteConfigLoader] Remote config disabled. Using local only.");
        }


    }

    private void ApplyLocalConfigFirst()
    {
        MechBossRemoteConfigData localData = localConfig.ToRuntimeData();
        bossContext.ApplyRemoteConfig(localData);

        if (printLogs)
            Debug.Log("[BossRemoteConfigLoader] Local config applied first.");
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
