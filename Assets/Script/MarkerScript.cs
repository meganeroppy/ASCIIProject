using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MarkerScript : NetworkBehaviour {

    public GameObject marker;

    // Use this for initialization
    void Start () {
        marker = GameObject.Find("Marker");
    }
	

    public void OnButtonDown()
    {
        marker.SetActive(true);
    }

}
