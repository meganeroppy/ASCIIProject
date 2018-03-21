using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// 来場者のスクリプト
/// </summary>
public class YvAudienceController : NetworkBehaviour
{
	public enum EmoteEnum
	{
		Like,	// いいね！
		DisLike // 微妙だね！
	}

    /// <summary>
    /// 現在フォーカスしているチャンネル
    /// エモート送信動作を行うとこのチャンネルの実況者プレイヤーに対して送信される
    /// とりあえずインスタンスIDを使う
    /// </summary>
    private string currentFocusChannel = "";

    /// <summary>
    /// 前フレームのフォーカス中チャンネル
    /// </summary>
    private string prevFocusCannel = "";

    private float checkDistanceInterval = 1f;
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

    /// <summary>
    /// 自分自身の時のみ有効なオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject[] localOnlyObjects;

	private int focusIndexPrev = -1;

    /// <summary>
    /// いいね！ボタン＆微妙だね！ボタン
    /// </summary>
    [SerializeField]
    private Button[] emoteButtons; 

	/// <summary>
	/// フォーカス中のTuberのUiセット
	/// </summary>
	[SerializeField]
	private GameObject[] uiSet;

    [SerializeField]
    private Text scoreText;

    private float scoreUpdateTimer = 0;
    private float scoreUpdateInterval = 5f;

    void Awake()
    {
        Debug.Log("Awake");

        // 自身専用オブジェクトをいったんすべて無効
		foreach (GameObject g in localOnlyObjects)
		{
			g.SetActive(false);
		}
    }

	public override void OnStartClient ()
	{
		base.OnStartClient ();

		// 初期位置と回転を指定する
		transform.position = YvGameManager.instance.AudienceOrigin.transform.position;
		transform.rotation = YvGameManager.instance.AudienceOrigin.transform.rotation;

        // 最初はエモート送信ボタン無効
        foreach (Button b in emoteButtons)
        {
            b.interactable = false;
			b.gameObject.SetActive( false );
        }
    }

	public override void OnStartLocalPlayer ()
	{
		base.OnStartLocalPlayer ();

		// 自身をARカメラの子要素にする
		transform.SetParent( YvGameManager.instance.ArCamera.transform, false ); 
	}

	void Start()
	{
		// 自身専用オブジェクトを有効
		if (isLocalPlayer)
		{
			foreach (GameObject g in localOnlyObjects)
			{
				g.SetActive(true);
			}

			// エモート送信ボタンも表示
			foreach (Button b in emoteButtons)
			{
				b.gameObject.SetActive( true );
			}
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
		UpdateUIFromDistanceSensor();
	}

    /// <summary>
    /// クライアント専用
    /// </summary>
    [ClientCallback]
    void LateUpdate()
    {
        prevFocusCannel = currentFocusChannel;
    }
		
	/// <summary>
	/// ターゲットとの距離に応じてフォーカス対象を変更する
	/// </summary>
	[Client]
	void UpdateFocusChannel()
	{
		// 毎フレーム処理すると負荷が高いので毎秒
		timer ++;
		if(  timer < checkDistanceInterval ) return;
			
		timer = 0;

		// 実況者がいなければなにもしない
		if( YvTuberController.tuberList == null )
		{
			return;
		}

		// 閾値取得
		var threshold = NetworkScript.instance.FocusChangeThreshold;

		// 範囲内リストを取得
		var inRange = YvTuberController.tuberList.FindAll(  o => Vector3.Distance( o.transform.position, transform.position ) <= threshold  );

		// 表示されていないものは除外
		inRange = inRange.FindAll( o => o.ModelRootActive );

		// リストが空ならフォーカス中なし
		if( inRange.Count <= 0 )
		{
			currentFocusChannel = "";
			return;
		}

		// リストから最寄りを取得
		inRange.Sort( (a,b) =>  (int)Vector3.Distance( a.transform.position, transform.position ) - (int)Vector3.Distance( b.transform.position, transform.position ) );
		var nearest = inRange[0];

        var newChannel = nearest.netId.ToString();

        if (currentFocusChannel != newChannel)
        {
            // チャンネル切り替え時に演出などするならここで

            Debug.Log("フォーカス中チャンネル変更 -> " + newChannel);
        }

		currentFocusChannel = newChannel;
	}

	/// <summary>
	/// エモート送信ボタンを押した時のイベント
	/// </summary>
	public void OnClickEmoteButton( int emoteIdx )
	{
		// フォーカス風チャンネルがなければなにもしない
		if( string.IsNullOrEmpty( currentFocusChannel) )
		{
			Debug.LogWarning("フォーカス中のチャンネルなし");
			return;
		}

		// 対象の実況者を取得
		var target = YvTuberController.tuberList.Find( o => o.netId.ToString() == currentFocusChannel );
		if( target == null )
		{
			Debug.LogError(" チャンネル(NetId) =  " + currentFocusChannel + " に該当する実況者が見つからない");
			return;
		}

		Debug.Log( emoteIdx == 0 ? "いいね！" : "微妙だね！");

		CmdSendEmote( (EmoteEnum)emoteIdx, target.netId );

		//演出
		DistanceSensor.instance.ShowEmoteEffect();
	}

	/// <summary>
	/// デバッグ用のキーボードによる入力を拾う
	/// </summary>
	[Client]
	private void UpdateTempInput()
	{
		if( Input.GetKeyDown( KeyCode.L ) )
		{
			OnClickEmoteButton( 0 );
		}

		if( Input.GetKeyDown( KeyCode.K ) )
		{
			OnClickEmoteButton( 1 );
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
    /// UI表示物更新
    /// </summary>
    [Client]
    private void UpdateUI()
    {
        if( currentFocusChannel != prevFocusCannel )
        {
            foreach( Button b in emoteButtons)
            {
                b.interactable = !string.IsNullOrEmpty(currentFocusChannel);
            }
        }
    }

	/// <summary>
	/// DistanceSensorの値を使用してUI表示変更
	/// </summary>
	[Client]
	private void UpdateUIFromDistanceSensor()
	{
		var d = DistanceSensor.instance;
		if( d == null ) return;

		if( d.currentFocusIndex == -1 )
		{
			foreach( GameObject b in uiSet )
			{
				b.SetActive( false );
			}

			//ボタンを無効にする
			foreach( Button b in emoteButtons)
			{
				b.gameObject.SetActive(false);
			}

			// テキスト非表示
			scoreText.gameObject.SetActive(false);

		}
		else
		{
			int uiIndex = d.currentFocusIndex -1;
			for( int i=0 ; i < uiSet.Length ; ++i )
			{
				uiSet[i].SetActive( i == uiIndex );
			}

			// ボタンを有効にする
			foreach( Button b in emoteButtons)
			{
				b.gameObject.SetActive(true);
				b.interactable = true;
			}

			// テキスト表示
			scoreText.gameObject.SetActive(true);

			// 切り替え時は表示スコアをすぐ更新
			scoreUpdateTimer = scoreUpdateInterval;
		}

		// チャンネルに変換
		if( focusIndexPrev != d.currentFocusIndex )
		{
			currentFocusChannel = GetNetIdByIndex( d.currentFocusIndex );
		}

		focusIndexPrev = d.currentFocusIndex;

        // スコア表示更新
        {
            // ５秒に一回更新
            scoreUpdateTimer += Time.deltaTime;
            if (scoreUpdateTimer < scoreUpdateInterval) return;
            scoreUpdateTimer = 0;

            int val = 0;
            // スコア
            var tuber = YvTuberController.tuberList.Find(t => t.netId.ToString() == currentFocusChannel);
            if (tuber != null)
            {
                val = tuber.LikeCount;
            }

            scoreText.text = "Love Score : " + val.ToString();

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
}
