using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkScript : NetworkManager
{
	/// <summary>
	/// 自身へのインスタンス
	/// </summary>
	public static NetworkScript instance;

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
	/// 実況者スクリプトでViveによる入力を無視するか？（＝キーボード入力を受け付けるか？）
	/// 基本false Viveがないデバッグ時などは適宜true
	/// </summary>
	[SerializeField]
	private bool ignoreViveTracking;
	public bool IgnoreViveTracking{ get{ return ignoreViveTracking; }}

	/// <summary>
	/// 特定の実況者との距離がこの値以下になったらその実況者をフォーカス対象とする
	/// </summary>
	[SerializeField]
	private float focusChangeThreshold = 1f;
	public float FocusChangeThreshold{get{return focusChangeThreshold;}}


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

	/// <summary>
	/// 自分自身の時でも自身のモデルを表示するか？
	/// </summary>
	[SerializeField]
	private bool forceDisplaySelf;
	public bool ForceDisplaySelf{get{ return forceDisplaySelf; }} 

    [SerializeField]
    public Canvas canvas;

    [SerializeField]
    private InputField inputField;

    //public GameObject dualTouchControls;

	void Start () 
	{
		instance = this;

    //    dualTouchControls = GameObject.Find("DualTouchControls");
    }
	
	void Update () {}

    public void OnHostButton()
    {
        // 自分のアプリタイプによってプレハブを変える
        NetworkManager.singleton.playerPrefab = appType == AppTypeEnum.Tuber ? playerPrefabTuber : playerPrefabAudience;

        canvas.gameObject.SetActive(false);
        StartHost();
    //    dualTouchControls.SetActive(true);
    }

    public void OnClientButton()
    {
        // 自分のアプリタイプによってプレハブを変える
        NetworkManager.singleton.playerPrefab = appType == AppTypeEnum.Tuber ? playerPrefabTuber : playerPrefabAudience;

        // テキストフィールドに値が入力されていたらそれを接続先アドレスにする
        if (!string.IsNullOrEmpty(inputField.text))
        {
            networkAddress = inputField.text;
        }

        canvas.gameObject.SetActive(false);
        var client = StartClient();
    //    dualTouchControls.SetActive(true);
        Debug.Log(client.serverIp);
        Debug.Log(client.serverPort);
        Debug.Log(client.GetType());
    } 

    public void OnServerButton()
    {
        canvas.gameObject.SetActive(false);
        StartServer();
    //    dualTouchControls.SetActive(true);
    }

}
