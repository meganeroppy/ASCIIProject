using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class YvTuberController : NetworkBehaviour
{
	/// <summary>
	/// 実況者のリスト
	/// </summary>
	public static List<YvTuberController> tuberList;

    [SerializeField]
    private GameObject modelRoot;

    private IKControl ik;

    private SteamVR_ControllerManager stController;
    private SteamVR_Camera stCamera ;

	/// <summary>
	/// 受け取ったいいね！の数
	/// </summary>
	[SyncVar]
	private int likeCount;
	public int LikeCount{ get{  return likeCount;}}

	/// <summary>
	/// 受け取った微妙だね！の数
	/// </summary>
	[SyncVar]
	private int disLikeCount;
	public int DislikeCount{ get{  return disLikeCount;}}

	void Start()
	{
		// リストに自身を追加
		if( tuberList == null ) tuberList = new List<YvTuberController>();
		tuberList.Add( this );
	}

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // 自分の時は強制表示フラグがない限り自身のモデルを非表示
		if( !NetworkScript.instance.ForceDisplaySelf )
        {
            modelRoot.SetActive(false);
        }

        ik = GetComponent<IKControl>();

        // ARカメラを無効
        YvGameManager.instance.ArCamera.SetActive(false);
    }

	public override void OnStartServer ()
	{
		base.OnStartServer ();

		// とりあえず現状は起動時にいいね/微妙だねかうんとを0にしてしまう
		likeCount = 0;
		disLikeCount = 0;
	}

	[ClientCallback]
    void Update()
    {
        UpdateInput();
		if ( NetworkScript.instance.IgnoreViveTracking )
		{
        	UpdatePosition();
		}
        UpdateIkObjects();
    }

    /// <summary>
    /// IKターゲットの更新
    /// </summary>
	[Client]
    void UpdateIkObjects()
    {
        // 両手のコントローラと該当オブジェクトの位置と回転を同期

        // HMDが向いている方向に見る対象オブジェクトを移動
        // 親要素を回転させる実装

    }

	[Client]
    void UpdatePosition()
    {
        // iKinema対応したら体と両足も対応する

        if ( stCamera == null )
        {
            stCamera = SteamVR_Camera.instance;
            if (stCamera == null) return;
        }


        // 頭 回転のみ
        if(stCamera.camera != null )
        {
            ik.headObj.transform.rotation = stCamera.camera.transform.rotation;
        }


        if (stController == null)
        {
            stController = SteamVR_ControllerManager.instance;
            if (stController == null) return;
        }

        // 両手　位置と回転
        if ( stController.right != null )
        {
            ik.rightHandObj.transform.position = stController.right.transform.position;
            ik.rightHandObj.transform.rotation = stController.right.transform.rotation;
        }
        if (stController.left != null)
        {
            ik.leftHandObj.transform.position = stController.left.transform.position;
            ik.leftHandObj.transform.rotation = stController.left.transform.rotation;
        }

    }
    /// <summary>
    /// 入力更新
    /// </summary>
	[Client]
    void UpdateInput()
    {
        if( Input.GetKey(KeyCode.I))
        {
            ik.leftHandObj.transform.Translate(Vector3.up * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.K))
        {
            ik.leftHandObj.transform.Translate(-Vector3.up * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.O))
        {
            ik.rightHandObj.transform.Translate(Vector3.up * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.L))
        {
            ik.rightHandObj.transform.Translate(-Vector3.up * Time.deltaTime);
        }
    }

	/// <summary>
	/// エモートを受け取る
	/// </summary>
	[Server]
	public void ReceiveEmote( YvAudienceController.EmoteEnum emote, NetworkInstanceId netId )
	{
		if( emote == YvAudienceController.EmoteEnum.Like )
		{
			likeCount++;
			Debug.Log( "NetId = " + netId.ToString() + " のプレイヤーから「いいね！」がとどいた" );
		}
		else
		{
			disLikeCount++;
			Debug.Log( "NetId = " + netId.ToString() + " のプレイヤーから「微妙だね！」がとどいた" );
		}
	}

}
