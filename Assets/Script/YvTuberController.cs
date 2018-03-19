using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Vuforia;

public class YvTuberController : NetworkBehaviour
{
	/// <summary>
	/// 実況者のリスト
	/// </summary>
	public static List<YvTuberController> tuberList;

    [SerializeField]
    private GameObject modelRoot;
	public bool ModelRootActive{ get{return modelRoot.activeSelf;}}

    private IKControl ik;

    private SteamVR_ControllerManager stController;
    private SteamVR_Camera stCamera ;

	[SerializeField]
	private int baseIndex = 0;

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

	/// <summary>
	/// 自身の土台となるオブジェクト 来場者から見られる時だけ使用
	/// </summary>
	private ImageTargetBehaviour myBase = null;

	private TrackableBehaviour.Status trackableStatusPrev = TrackableBehaviour.Status.NOT_FOUND;
	/// <summary>
	/// ダミープレイヤーか？
	/// </summary>
	[SerializeField]
	private bool isDummyPlayer = false;

	void Start()
	{
		// リストに自身を追加
		if( tuberList == null ) tuberList = new List<YvTuberController>();
		tuberList.Add( this );
	}
		

	[Client]
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // 自分の時は強制表示フラグがない限り自身のモデルを非表示
        modelRoot.SetActive(NetworkScript.instance.ForceDisplaySelf);

        ik = GetComponent<IKControl>();

        // ARカメラを無効
        YvGameManager.instance.ArCamera.SetActive(false);
    }

	public override void OnStartServer ()
	{
		base.OnStartServer ();

		// いいね/微妙だねカウントをローカルセーブから取得
		likeCount = PlayerPrefs.GetInt( "TuberGood" + baseIndex.ToString(), 0 );
		disLikeCount = PlayerPrefs.GetInt( "TuberBad" + baseIndex.ToString(), 0 );

		Debug.Log("エモート数をロード : ID = " + baseIndex.ToString() + " " + likeCount.ToString() + "いいね / " + disLikeCount.ToString() + "微妙だね");
	}

	[Client]
	public override void OnStartClient ()
	{
		base.OnStartClient ();

		if( NetworkScript.instance.AppType == NetworkScript.AppTypeEnum.Tuber ) 
		{		
			// 自身も実況者の場合

			// 自分自身でなければ非表示
			modelRoot.SetActive( false );
		}
		else
		{
			// 自身が来場者プレイヤーの場合

			// 最初は非表示
			modelRoot.SetActive( false );

			var baseImageTarget = YvGameManager.instance.GetBase( baseIndex );
			if( baseImageTarget == null ) 	
			{
				Debug.LogError( baseIndex.ToString() + "に該当するベースが存在しない" );
				return;
			}

			transform.SetParent( baseImageTarget.transform, false );

			myBase = baseImageTarget.GetComponent<ImageTargetBehaviour>();
		}
	}

	[ClientCallback]
    void Update()
    {
        UpdateInput();
		if ( !NetworkScript.instance.IgnoreViveTracking )
		{
        	UpdatePosition();
		}
        UpdateIkObjects();
		UpdateArBehaviour();
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
		if( !isLocalPlayer ) return;

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
		if( !isLocalPlayer ) return;

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
	/// AR上の挙動を更新
	/// </summary>
	[Client]
	private void UpdateArBehaviour()
	{
		/// 対象は来場者環境のみ
		if( NetworkScript.instance.AppType != NetworkScript.AppTypeEnum.Audience ) return;

		if( myBase == null ) 
		{
			Debug.LogWarning( gameObject.name + "はベースが未定義" );
			return;
		}

		var newStatus = myBase.CurrentStatus;
		if( newStatus != trackableStatusPrev )
		{
			Debug.Log( gameObject.name + "の表示切り替え :"  + trackableStatusPrev.ToString() + " -> " + newStatus.ToString() );
			modelRoot.SetActive( myBase.CurrentStatus == TrackableBehaviour.Status.TRACKED
				|| myBase.CurrentStatus == TrackableBehaviour.Status.DETECTED
				|| myBase.CurrentStatus == TrackableBehaviour.Status.EXTENDED_TRACKED);
		}

		trackableStatusPrev = newStatus;
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
			Debug.Log( "NetId = " + netId.ToString() + " の来場者から NetId = " + this.netId.ToString() + "の実況者に「いいね！」がとどいた" );

			// ローカルに保存
			PlayerPrefs.SetInt( "TuberGood" + baseIndex.ToString(), likeCount );
		}
		else
		{
			disLikeCount++;
			Debug.Log( "NetId = " + netId.ToString() + " の来場者から NetId = " + this.netId.ToString() + "の実況者に「微妙だね！」がとどいた" );

			// ローカルに保存
			PlayerPrefs.SetInt( "TuberBad" + baseIndex.ToString(), disLikeCount );
		}
	}

}
