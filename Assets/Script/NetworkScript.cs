using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkScript : NetworkBehaviour {

    public Canvas canvas;

    public GameObject dualTouchControls;

	void Start () {
        dualTouchControls = GameObject.Find("DualTouchControls");
    }
	
	void Update () {}

    public void OnHostButton()
    {
        canvas.gameObject.SetActive(false);
        NetworkManager.singleton.StartHost();
        dualTouchControls.SetActive(true);
    }

    public void OnClientButton()
    {
        canvas.gameObject.SetActive(false);
        NetworkClient client = NetworkManager.singleton.StartClient();
        dualTouchControls.SetActive(true);
        Debug.Log(client.serverIp);
        Debug.Log(client.serverPort);
        Debug.Log(client.GetType());
    } 

    public void OnServerButton()
    {
        canvas.gameObject.SetActive(false);
        NetworkManager.singleton.StartServer();
        dualTouchControls.SetActive(true);
    }

}
