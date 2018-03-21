using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
#if UNITY_ANDROID || UNITY_EDITOR
using Vuforia; 
#endif

public class StageManager : NetworkBehaviour
{
    [SerializeField]
    private GameObject root;

#if UNITY_ANDROID || UNITY_EDITOR

    /// <summary>
    /// 自身の土台となるオブジェクト 来場者から見られる時だけ使用
    /// </summary>
    private ImageTargetBehaviour myBase = null;

    private TrackableBehaviour.Status trackableStatusPrev = TrackableBehaviour.Status.NOT_FOUND;

#endif

    void Start()
    {
        // 自身がTuberの時は非表示
        if( NetworkScript.instance.AppType == NetworkScript.AppTypeEnum.Tuber && !NetworkScript.instance.ForceDisplayStage)
        {
            root.SetActive(false);
        }
    }

#if UNITY_ANDROID || UNITY_EDITOR

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
            myBase = baseImageTarget.GetComponent<ImageTargetBehaviour>();
        }
    }

    [ClientCallback]
	void Update()
	{
		/// 対象は来場者環境のみ
		if( NetworkScript.instance.AppType != NetworkScript.AppTypeEnum.Audience ) return;

        if (myBase == null)
        {
            Debug.LogWarning(gameObject.name + "はベースが未定義");
            return;
        }

        var newStatus = myBase.CurrentStatus;
        if (newStatus != trackableStatusPrev)
        {
            Debug.Log(gameObject.name + "の表示切り替え :" + trackableStatusPrev.ToString() + " -> " + newStatus.ToString());
            root.SetActive(myBase.CurrentStatus == TrackableBehaviour.Status.TRACKED
                || myBase.CurrentStatus == TrackableBehaviour.Status.DETECTED
                || myBase.CurrentStatus == TrackableBehaviour.Status.EXTENDED_TRACKED);
        }

        trackableStatusPrev = newStatus;

        }

#endif

}
