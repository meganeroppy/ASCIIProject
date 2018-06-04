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
	/// 展示環境では基本false Viveがないデバッグ時などは適宜true
	/// 実況者専用
	/// </summary>
	[SerializeField]
	private bool ignoreViveTracking;
	public bool IgnoreViveTracking{ get{ return ignoreViveTracking; }}

	/// <summary>
	/// 特定の実況者との距離がこの値以下になったらその実況者をフォーカス対象とする
	/// 来場者専用
	/// </summary>
	[SerializeField]
	private float focusChangeThreshold = 1f;
	public float FocusChangeThreshold{get{return focusChangeThreshold;}}


    [SerializeField]
    private bool enableIKinema = true;
    public bool EnableIKinema { get { return enableIKinema; } }

    /// <summary>
    /// 実況者プレハブ
    /// </summary>
    [SerializeField]
    private GameObject playerPrefabTuber;

    /// <summary>
    /// 実況者プレハブiKinema併用版
    /// </summary>
    [SerializeField]
    private GameObject playerPrefabTuberIKinema;

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

    /// <summary>
    /// Tuberの時でもステージを表示するか？
    /// </summary>
    [SerializeField]
    private bool forceDisplayStage;
    public bool ForceDisplayStage { get { return forceDisplayStage; } }

    [SerializeField]
    public Canvas canvas;

    [SerializeField]
    private InputField inputField;

    //public GameObject dualTouchControls;

	[SerializeField]
	private bool disableAutoConnectAsAudienceClient = false;

	void Start () 
	{
		instance = this;

        //    dualTouchControls = GameObject.Find("DualTouchControls");

		// 来場者アプリは即クライアントとして接続
		if( AppType == AppTypeEnum.Audience && !disableAutoConnectAsAudienceClient )
		{
			ConnectClient();
		}
    }
	
	void Update () {}

    public void OnHostButton()
    {
        canvas.gameObject.SetActive(false);
        StartHost();
    //    dualTouchControls.SetActive(true);
    }

    public void OnClientButton()
    {
		ConnectClient();
    } 

	private void ConnectClient()
	{
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

    /// <summary>
    /// 生成プレハブを変えるためにオーバーライド
    /// </summary>
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader) // ←第3引数があるバージョンに変更します
    {
        PlayerInfoMessage msg = extraMessageReader.ReadMessage<PlayerInfoMessage>();
        var appType = msg.appType;

        GameObject playerPrefab;

        if (appType == AppTypeEnum.Tuber)
        {
            if (enableIKinema)
            {
                playerPrefab = playerPrefabTuberIKinema;
            }
            else
            {
                playerPrefab = playerPrefabTuber;
            }
        }
        else
        {
            playerPrefab = playerPrefabAudience;
        }

        GameObject player;
        Transform startPos = GetStartPosition();
        if (startPos != null)
        {
            player = (GameObject)Instantiate(playerPrefab, startPos.position, startPos.rotation);
        }
        else
        {
            player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }

        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

	/// <summary>
	/// クライアントがサーバー（またはホスト）に接続した時に呼ばれる
	/// </summary>
    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
        // クライアント上でシーンの準備が完了したらこれを呼ぶ必要がある
        ClientScene.Ready(conn);

        // PlayerInfoMessageオブジェクトを作成し、プレイヤーの情報を格納する
        PlayerInfoMessage msg = new PlayerInfoMessage();
        msg.appType = AppType;

        // サーバーにAddPlayerメッセージを送信する。
        // その際、第3引数に追加情報（PlayerInfoMessage）を付与する。
        ClientScene.AddPlayer(conn, 0, msg);
    }

    // プレイヤー生成時に必要な情報をクライアントからサーバーへ送るための入れ物
    class PlayerInfoMessage : MessageBase
    {
        public AppTypeEnum appType;
    }
}
