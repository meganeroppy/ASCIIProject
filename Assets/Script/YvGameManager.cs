using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Vuforia;

/// <summary>
/// ネットワーク以外の部分の制御を行う
/// </summary>
public class YvGameManager : NetworkBehaviour
{
    public static YvGameManager instance;

	/// <summary>
	/// 来場者プレハブのカメラがちゃんと動いていることが確認できたら削除
	/// </summary>
    [SerializeField]
    private GameObject arCamera;
    public GameObject ArCamera { get { return arCamera; } }

	/// <summary>
	/// VR用カメラリグ
	/// 自身が実況者でなければ無効にする
	/// </summary>
	[SerializeField]
	private GameObject cameraRig;

	/// <summary>
	/// 来場者プレイヤーの初期生成位置
	/// </summary>
	[SerializeField]
	private Transform audienceOrigin;
	public Transform AudienceOrigin{ get {return audienceOrigin;}}

	/// <summary>
	/// 実況者ダミーのプレハブ
	/// </summary>
	[SerializeField]
	private YvTuberController[] tuberDummyPrefabs;

	/// <summary>
	/// 実況者ダミーを生成するルート
	/// tuberDummyPrefabsと長さが一致していること
	/// </summary>
	[SerializeField]
	private ImageTargetBehaviour[] tuberDummyBase;

    /// <summary>
    /// ステージのプレハブ
    /// </summary>
    [SerializeField]
    private NetworkIdentity stagePrefab;

    /// <summary>
    /// ステージ生成ベース
    /// </summary>
    [SerializeField]
    private ImageTargetBehaviour stageBase;
    public ImageTargetBehaviour StageBase { get { return stageBase; } }

    // Use this for initialization
    void Awake ()
    {
        instance = this;
	}

	void Start()
	{
		if( isServer )
		{
			// ダミーを生成する
			for( int i=0 ; i < tuberDummyPrefabs.Length ; ++i )
			{
				var tuber = Instantiate( tuberDummyPrefabs[i] );

				// サーバー上に生成
				NetworkServer.Spawn( tuber.gameObject );
			}

            // ステージを生成
            var stage = Instantiate(stagePrefab);
            NetworkServer.Spawn(stage.gameObject);        
		}

		// 自分が実況者でなければカメラリグ無効
		if( NetworkScript.instance.AppType != NetworkScript.AppTypeEnum.Tuber )
		{
			cameraRig.SetActive( false );
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[Client]
	public ImageTargetBehaviour GetTuberBase( int index )
	{
		if( tuberDummyBase.Length <= index )
		{
			Debug.LogError( "実況者ダミーインデックス[ " + index.ToString() + " ]に対応するルートがない");
			return null;
		}

		return tuberDummyBase[index];
	}
}
