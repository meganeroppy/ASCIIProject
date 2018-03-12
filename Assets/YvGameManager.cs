using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ネットワーク以外の部分の制御を行う
/// </summary>
public class YvGameManager : MonoBehaviour
{
    public static YvGameManager instance;

    [SerializeField]
    private GameObject arCamera;
    public GameObject ArCamera { get { return arCamera; } }

	// Use this for initialization
	void Awake ()
    {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
