#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LoadFormSever : MonoBehaviour
{
    public string bundleUrl = "https://drive.google.com/uc?export=download&id=1o8z_7-8cU_HzNGJnoB_mPxxgCXxmqWzn";
    public string savePath = "Assets/Prefabs"; // Nơi lưu prefab

    void Start()
    {
        StartCoroutine(DownloadAndSavePrefabs());
    }

    IEnumerator DownloadAndSavePrefabs()
    {
        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to download AssetBundle: " + www.error);
            }
            else
            {
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

                if (bundle != null)
                {
                    string[] assetNames = bundle.GetAllAssetNames();
                    foreach (string assetName in assetNames)
                    {
                        GameObject obj = bundle.LoadAsset<GameObject>(assetName);
                        GameObject instance = Instantiate(obj);

#if UNITY_EDITOR
                        // Đảm bảo thư mục lưu tồn tại
                        if (!System.IO.Directory.Exists(savePath))
                        {
                            System.IO.Directory.CreateDirectory(savePath);
                        }

                        string prefabPath = $"{savePath}/{obj.name}.prefab";
                        PrefabUtility.SaveAsPrefabAsset(instance, prefabPath);
                        Debug.Log($"Saved Prefab: {prefabPath}");
#endif
                        Destroy(instance); // Xóa instance trên Scene vì chỉ cần tạo Prefab
                    }
                }
            }
        }
    }
}