using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ネットワーク以外の部分の制御を行う
/// </summary>
public class YvGameManager : MonoBehaviour
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

	// Use this for initialization
	void Awake ()
    {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
