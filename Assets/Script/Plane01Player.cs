using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Plane01Player : NetworkBehaviour {



    // Use this for initialization
    void Start () {

    }
	
    public void LButtonDown()
    {
        transform.Rotate(0, -5, 0);
    }

    public void RButtonDown()
    {
        transform.Rotate(0, 5, 0);
    }


}
