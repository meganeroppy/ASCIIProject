using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
	/// 来場者プレイヤーの初期生成位置
	/// </summary>
	[SerializeField]
	private Transform audienceOrigin;
	public Transform AudienceOrigin{ get {return audienceOrigin;}}

	/// <summary>
	/// 実況者ダミーのプレハブ
	/// </summary>
	[SerializeField]
	private GameObject[] tuberDummyPrefabs;

	/// <summary>
	/// 実況者ダミーを生成するルート
	/// tuberDummyPrefabsと長さが一致していること
	/// </summary>
	[SerializeField]
	private Transform[] tuberDummyRoots;

	// Use this for initialization
	void Awake ()
    {
        instance = this;
	}

	[ServerCallback]
	void Start()
	{
		// ダミーを生成する
		for( int i=0 ; i < tuberDummyPrefabs.Length ; ++i )
		{
			var obj = Instantiate( tuberDummyPrefabs[i] );
			if( tuberDummyRoots.Length <= i )
			{
				Debug.LogError( "実況者ダミーインデックス[ " + i.ToString() + " ]に対応するルートがない");
				continue;
			}

			var root = tuberDummyRoots[i];

			obj.transform.SetParent( root, false );

			// サーバー上に生成
			NetworkServer.Spawn( obj );
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
