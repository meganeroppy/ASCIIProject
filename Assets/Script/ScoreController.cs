using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

    public Text scoreText_A;
    public Text scoreText_B;
    public Text scoreText_C;

    int score_A;
    int score_B;
    int score_C;

    void Start()
    {
        score_A = 0;
        score_B = 0;
        score_C = 0;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText_A.text = "Love Score: " + score_A;
        scoreText_B.text = "Love Score: " + score_B;
        scoreText_C.text = "Love Score: " + score_C;
    }

    public void Score_A_Plus()
    {
        score_A += 1;
    }

    public void Score_B_Plus()
    {
        score_B += 1;
    }

    public void Score_C_Plus()
    {
        score_C += 1;
    }

}
