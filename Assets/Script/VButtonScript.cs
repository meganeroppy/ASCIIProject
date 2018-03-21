using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID || UNITY_EDITOR
using Vuforia; 
#endif

public class VButtonScript : MonoBehaviour
#if UNITY_ANDROID || UNITY_EDITOR
    , IVirtualButtonEventHandler
#endif
{

    public GameObject UP_btnObj;
    public GameObject Plane01;

    // Use this for initialization
#if UNITY_ANDROID || UNITY_EDITOR
    void Start()
    {
        UP_btnObj = GameObject.Find("UP_btn");
        UP_btnObj.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
        Plane01 = GameObject.Find("Plane01");
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Plane01.transform.Translate(0, 1, 0);
        Debug.Log("Button pressed");
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        Plane01.transform.Translate(0, -1, 0);
        Debug.Log("Button released");
    }
#endif
}
