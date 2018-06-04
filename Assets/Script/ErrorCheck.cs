using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ErrorCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine( OnLoad() );
	}
	
	IEnumerator OnLoad()
	{
		// Player Settingの設定が勝手に変わるので再設定
		{
			#if UNITY_EDITOR

			while( NetworkScript.instance == null ) yield return null;

			if( NetworkScript.instance.AppType == NetworkScript.AppTypeEnum.Tuber && !UnityEditor.PlayerSettings.virtualRealitySupported )
			{
				Debug.LogError("実況者モードにもかかわらずVR無効");
			}
			else if( NetworkScript.instance.AppType == NetworkScript.AppTypeEnum.Audience && UnityEditor.PlayerSettings.virtualRealitySupported )
			{
				Debug.LogError("来場者モードにもかかわらずVR有効");
			}

			#endif

			while( VuforiaBehaviour.Instance == null ) yield return null;

			if( NetworkScript.instance.AppType == NetworkScript.AppTypeEnum.Audience && !VuforiaBehaviour.Instance.enabled )
			{
				Debug.LogError("来場者モードにもかかわらずVuforia無効");
			}

			else if( NetworkScript.instance.AppType == NetworkScript.AppTypeEnum.Tuber && VuforiaBehaviour.Instance.enabled )
			{
				Debug.LogError("実況者モードにもかかわらずVuforia有効");
			}
		}
	}

}
