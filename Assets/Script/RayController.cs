using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayController : MonoBehaviour {

    public GameObject ARCamera;

    //Rayの飛ばせる距離
    public float distance = 1.0f;


    //Rayが当たったオブジェクトの情報を入れる箱
    RaycastHit hit;


    public GameObject Canvas_flower01 ;

    void Start()
    {

    }

    void Update()
    {
        Ray ray = new Ray(ARCamera.transform.position, ARCamera.transform.forward);



        //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　　　　　↓Rayの色
        Debug.DrawLine(ray.origin, ray.direction * distance, Color.red);

        if (Physics.Raycast(ray, out hit, distance))
        {
            //Rayが当たったオブジェクトのtagがCuberだったら
            if (hit.collider.tag == "Cube")
            {
                Debug.Log("RayがCubeに当たった");
                Canvas_flower01.SetActive(true);
            }

        }




    }
}
