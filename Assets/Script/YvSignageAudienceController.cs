using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// ARモード時の
/// 来場者のスクリプト
/// </summary>
public class YvSignageAudienceController : NetworkBehaviour
{
    /// <summary>
	private float timer = 0;

	/// <summary>
	/// 自身のビジュアル
	/// </summary>
	[SerializeField]
	private GameObject model;

    [SerializeField]
    private float tempMoveSpeed = 1.5f;

    [SerializeField]
    private float tempRotSpeed = 5f;

    private float scoreUpdateTimer = 0;
    private float scoreUpdateInterval = 5f;

    void Awake()
    {
        Debug.Log("Awake");
    }

	public override void OnStartClient ()
	{
		base.OnStartClient ();

		// 初期位置と回転を指定する
		transform.position = YvARGameManager.instance.AudienceOrigin.transform.position;
		transform.rotation = YvARGameManager.instance.AudienceOrigin.transform.rotation;
    }

	public override void OnStartLocalPlayer ()
	{
		base.OnStartLocalPlayer ();
	}

	void Start()
	{
		// 自身専用オブジェクトを有効
		if (isLocalPlayer)
		{
		}

        // 自分の時は強制表示フラグがない限り自身のモデルを非表示
        if (isLocalPlayer && !NetworkScript.instance.ForceDisplaySelf)
        {
            model.SetActive(false);
        }
    }

    /// <summary>
    /// クライアント専用
    /// </summary>
    [ClientCallback]
	void Update () 
	{
		// 自身でなければなにもしない
		if( !isLocalPlayer ) return;

	//	UpdateFocusChannel();
		UpdateTempInput();
	//	UpdateUI();
	}

    /// <summary>
    /// クライアント専用
    /// </summary>
    [ClientCallback]
    void LateUpdate()
    {
    }
		
	/// <summary>
	/// デバッグ用のキーボードによる入力を拾う
	/// </summary>
	[Client]
	private void UpdateTempInput()
	{
		if( Input.GetKeyDown( KeyCode.L ) )
		{
		//	OnClickEmoteButton( 0 );
		}

		if( Input.GetKeyDown( KeyCode.K ) )
		{
		//	OnClickEmoteButton( 1 );
		}

		var v = Input.GetAxis("Vertical");

		if( Mathf.Abs(v) > 0 )
		{
			transform.Translate( transform.forward * ( v > 0 ? -1f : 1f ) * tempMoveSpeed * Time.deltaTime );
		}

		var h = Input.GetAxis("Horizontal");

		if( Mathf.Abs(h) > 0 )
		{
			transform.Translate( transform.right * ( h > 0 ? -1f : 1f ) * tempMoveSpeed * Time.deltaTime );
		}

		if( Input .GetKey( KeyCode.I ) )
		{
			transform.Rotate( Vector3.up * -tempRotSpeed * Time.deltaTime );
		}

		if( Input .GetKey( KeyCode.O ) )
		{
			transform.Rotate( Vector3.up * tempRotSpeed * Time.deltaTime );
		}

	}

	/// <summary>
	/// インデックスからNetIdを取得する
	/// </summary>
	private string GetNetIdByIndex( int idx )
	{
		var result = YvTuberController.tuberList.Find( t => t.BaseIndex == idx );

		if( result == null )
		{
			Debug.LogWarning( "インデックス " + idx.ToString() +  "のTuberが存在しない");
			return "";
		}

		Debug.LogWarning( "インデックス " + idx.ToString() +  "のTuberはNetId=" + result.netId.ToString() );

		return result.netId.ToString();
	}

    /*
	/// <summary>
	/// エモートの送信をサーバー側で行う
	/// </summary>
	[Command]
	private void CmdSendEmote( EmoteEnum emote, NetworkInstanceId netId )
	{
		var target = YvTuberController.tuberList.Find( o => o.netId == netId );
		if( target == null )
		{
			Debug.LogError( netId.ToString() + " に該当する実況者がみつからない");
			return;
		}

		// エモートを送信
		target.ReceiveEmote( emote, this.netId );
	}
    */
}
