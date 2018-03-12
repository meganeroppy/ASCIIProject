using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkScript : NetworkBehaviour
{
    /// <summary>
    /// アプリタイプ
    /// </summary>
    public enum AppTypeEnum
    {
        Tuber,      // 実況者
        Audience,   // 来場者（＝実況者を見る）
    }

    /// <summary>
    /// 自身のアプリタイプ
    /// </summary>
    [SerializeField]
    private AppTypeEnum appType;
    public AppTypeEnum AppType { get { return appType; } }

    /// <summary>
    /// 実況者プレハブ
    /// </summary>
    [SerializeField]
    private GameObject playerPrefabTuber;

    /// <summary>
    /// 来場者プレハブ
    /// </summary>
    [SerializeField]
    private GameObject playerPrefabAudience;

    [SerializeField]


    public Canvas canvas;

    public GameObject dualTouchControls;

	void Start () {
        dualTouchControls = GameObject.Find("DualTouchControls");
    }
	
	void Update () {}

    public void OnHostButton()
    {
        // 自分のアプリタイプによってプレハブを変える
        NetworkManager.singleton.playerPrefab = appType == AppTypeEnum.Tuber ? playerPrefabTuber : playerPrefabAudience;

        canvas.gameObject.SetActive(false);
        NetworkManager.singleton.StartHost();
        dualTouchControls.SetActive(true);
    }

    public void OnClientButton()
    {
        // 自分のアプリタイプによってプレハブを変える
        NetworkManager.singleton.playerPrefab = appType == AppTypeEnum.Tuber ? playerPrefabTuber : playerPrefabAudience;

        canvas.gameObject.SetActive(false);
        NetworkClient client = NetworkManager.singleton.StartClient();
        dualTouchControls.SetActive(true);
        Debug.Log(client.serverIp);
        Debug.Log(client.serverPort);
        Debug.Log(client.GetType());
    } 

    public void OnServerButton()
    {
        canvas.gameObject.SetActive(false);
        NetworkManager.singleton.StartServer();
        dualTouchControls.SetActive(true);
    }

}
