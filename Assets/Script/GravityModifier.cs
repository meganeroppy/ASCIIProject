using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;

/// <summary>
/// このスクリプトがアタッチされたオブジェクトに
/// VRMSprinBoneがアタッチされている前提のスクリプト
/// 
/// モデルルートの向きに対しての下方向を重力の方向とする
/// </summary>
public class GravityModifier : MonoBehaviour
{
    VRMSpringBone[] list;

    [SerializeField]
    Transform root;

    Quaternion rotPrev = Quaternion.identity;

	// Use this for initialization
	void Start ()
    {
        list = GetComponents<VRMSpringBone>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (root == null) return;

        if (root.rotation.Equals(rotPrev)) return;

        for( int i=0; i <list.Length; ++i )
        {
            list[i].m_gravityDir = -root.up;
        }

        rotPrev = root.rotation;
	}
}
