using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.Networking;

public class UITestManager : MonoBehaviour
{
    [SerializeField]
    public Button ClickButton;

    [SerializeField]
    public Image ChangeImage;
    // Start is called before the first frame update
    void Start()
    {
        ClickButton.onClick.AddListener(ClickTest);
    }

    private void Update()
    {
      
    }

    void ClickTest()
    {
        Sprite sp = ResourceManager.Load<Sprite>("role_tx_1");
        ChangeImage.sprite = sp;

//        AudioPlayManager.PlayMusic2D("101", 1, 2, false, 0.5f, 0.1f,"");
    }

    private IEnumerator Load()
    {
        int index = UnityEngine.Random.Range(1, 3);
        Debug.Log("index =======" + index);
        string path = @"https://yueli01.oss-cn-shanghai.aliyuncs.com/SnowGame/StreamingAssets/_role_tx_" + index.ToString();
        //UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(path);
        //yield return request.SendWebRequest();
        //// AssetBundle ab = DownloadHandlerAssetBundle.GetContent (request );
        //AssetBundle ab = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;

        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();
        while (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError("ERROR" + request.error);
            yield return null;
        }
        while (!request.isDone)
        {
            yield return null;
        }
        byte[] results = request.downloadHandler.data;

        Debug.LogError("resultsLength" + results.Length);
        AssetBundle ab = AssetBundle.LoadFromMemory(results);
        Sprite s = ab.LoadAsset<Sprite>("role_tx_" + index.ToString());
        ChangeImage.sprite = s;
        ab.Unload(false);
    }


}
