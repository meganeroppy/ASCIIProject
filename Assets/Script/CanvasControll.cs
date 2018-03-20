using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasControll : MonoBehaviour {

    public GameObject Canvas_flowerA;
    public GameObject Canvas_flowerB;
    public GameObject Canvas_flowerC;
    //   public GameObject Particle;

    /// ボタンをクリックした時の処理
    public void OnClick_A()
    {
        Canvas_flowerA.SetActive(true);
        Canvas_flowerB.SetActive(false);
        Canvas_flowerC.SetActive(false);
    }

    public void OnClick_B()
    {
        Canvas_flowerA.SetActive(false);
        Canvas_flowerB.SetActive(true);
        Canvas_flowerC.SetActive(false);
    }

    public void OnClick_C()
    {
        Canvas_flowerA.SetActive(false);
        Canvas_flowerB.SetActive(false);
        Canvas_flowerC.SetActive(true);
    }
}
