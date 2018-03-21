using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StageManager : NetworkBehaviour
{
    [SerializeField]
    private GameObject root;

    void Start()
    {
        // 自身がTuberの時は非表示
        if( NetworkScript.instance.AppType == NetworkScript.AppTypeEnum.Tuber && !NetworkScript.instance.ForceDisplayStage)
        {
            root.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
