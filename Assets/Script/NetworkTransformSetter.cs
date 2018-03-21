using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 子要素のTransformをすべて取得し、NetworkTransformChildの対象として指定する
/// </summary>
public class NetworkTransformSetter : NetworkBehaviour
{
    [SerializeField]
    private Transform root;
    Transform[] childTransformList;

    private void Awake()
    {
        childTransformList = root.GetComponentsInChildren<Transform>();

        // 一旦オブジェクトを無効にしないと NetworkTransformChildを追加する行でエラーになる Unityのバグ
        gameObject.SetActive(false);

        foreach (Transform t in childTransformList)
        {
            var n = gameObject.AddComponent<NetworkTransformChild>();
            n.target = t;
            n.enabled = true;
        }

        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
