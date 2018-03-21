using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceSensor : MonoBehaviour {

    public const float NOTHING = -1;    // 計測不能

    public GameObject Canvas_flowerA;
    public GameObject Canvas_flowerB;
    public GameObject Canvas_flowerC;

    public float maxDistance = 5;      // 計測可能な最大距離
    public float distance;              // 計測距離

    // 距離計測
    void Update()
    {
        // 前方ベクトル計算
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        // 距離計算
        RaycastHit hit;
        if (Physics.Raycast(transform.position, fwd, out hit, maxDistance))
        {
            distance = hit.distance;

            //Rayが当たったオブジェクトのtagがPlayerだったら
            if (hit.collider.tag == "Target_A")
            {
                Canvas_flowerA.SetActive(true);
                Canvas_flowerB.SetActive(false);
                Canvas_flowerC.SetActive(false);

                Debug.Log("Target_A:"+distance);
            }
            else if (hit.collider.tag == "Target_B")
            {
                Canvas_flowerA.SetActive(false);
                Canvas_flowerB.SetActive(true);
                Canvas_flowerC.SetActive(false);

                Debug.Log("Target_B:" + distance);
            }
            else if(hit.collider.tag == "Target_C")
            {
                Canvas_flowerA.SetActive(false);
                Canvas_flowerB.SetActive(false);
                Canvas_flowerC.SetActive(true);

                Debug.Log("Target_C:" + distance);
            }
            else
            {
                Canvas_flowerA.SetActive(false);
                Canvas_flowerB.SetActive(false);
                Canvas_flowerC.SetActive(false);
            }

        }
        else
        {
            Canvas_flowerA.SetActive(false);
            Canvas_flowerB.SetActive(false);
            Canvas_flowerC.SetActive(false);

            distance = NOTHING;
        }

    }
}
