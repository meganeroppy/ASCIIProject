using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoveTargetController : MonoBehaviour {

    public Transform LoveTarget_A;
    public Transform LoveTarget_B;
    public Transform LoveTarget_C;

    private Transform Target;


    public void Target_A()
    {
        Target = LoveTarget_A;
    }

    public void Target_B()
    {
        Target = LoveTarget_B;
    }

    public void Target_C()
    {
        Target = LoveTarget_C;
    }

}
