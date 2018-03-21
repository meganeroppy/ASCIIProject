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

    [Client]
    public override void OnStartClient()
    {
        base.OnStartClient();

        if(NetworkScript.instance.AppType == NetworkScript.AppTypeEnum.Audience)
        {
            // 自身が来場者プレイヤーの場合

            // 最初は非表示
            root.SetActive(false);

            var baseImageTarget = YvGameManager.instance.StageBase;
            if (baseImageTarget == null)
            {
                Debug.LogError("ステージベースが存在しない");
                return;
            }

            transform.SetParent(baseImageTarget.transform, false);
        //    myBase = baseImageTarget.GetComponent<ImageTargetBehaviour>();
        }
    }

}
