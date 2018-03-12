using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class YvPlayerController : NetworkBehaviour
{
    /// <summary>
    /// 自分自身の時でも自身のモデルを表示する
    /// </summary>
    [SerializeField]
    private bool ForceDisplaySelf;

    [SerializeField]
    private GameObject modelRoot;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // 自分の時は強制表示フラグがない限り自身のモデルを非表示
        if( !ForceDisplaySelf )
        {
            modelRoot.SetActive(false);
        }
    }
}
