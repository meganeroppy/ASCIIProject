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

    private IKControl ik;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // 自分の時は強制表示フラグがない限り自身のモデルを非表示
        if( !ForceDisplaySelf )
        {
            modelRoot.SetActive(false);
        }

        ik = GetComponent<IKControl>();
    }

    void Update()
    {
        UpdateInput();
        UpdateIkObjects();
    }

    /// <summary>
    /// IKターゲットの更新
    /// </summary>
    void UpdateIkObjects()
    {
        // 両手のコントローラと該当オブジェクトの位置と回転を同期

        // HMDが向いている方向に見る対象オブジェクトを移動
        // 親要素を回転させる実装

    }

    /// <summary>
    /// 入力更新
    /// </summary>
    void UpdateInput()
    {
        if( Input.GetKey(KeyCode.I))
        {
            ik.leftHandObj.transform.Translate(Vector3.up * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.K))
        {
            ik.leftHandObj.transform.Translate(-Vector3.up * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.O))
        {
            ik.rightHandObj.transform.Translate(Vector3.up * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.L))
        {
            ik.rightHandObj.transform.Translate(-Vector3.up * Time.deltaTime);
        }

    }
}
