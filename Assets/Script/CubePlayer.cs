using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CubePlayer : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    public void LButtonDown()
    {
        transform.Translate(-0.5f, 0, 0);
    }

    public void RButtonDown()
    {
        transform.Translate(0.5f, 0, 0);

    }


}
