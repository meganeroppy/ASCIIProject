using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AttackScript : NetworkBehaviour {



 //   public GameObject sphere;

    public GameObject player;


    // Animator コンポーネント
    public Animator animator;

    // 設定したフラグの名前
    private const string key_isRun = "IsRun";
    private const string key_isAttack01 = "IsAttack01";
    private const string key_isAttack02 = "IsAttack02";
    private const string key_isJump = "IsJump";
    private const string key_isDamage = "IsDamage";
    private const string key_isDead = "IsDead";
    // 初期化メソッド

    void Start()
    {
   //     sphere = GameObject.Find("Sphere");

        // 自分に設定されているAnimatorコンポーネントを習得する
     //   Chara_4Hero.animator = GetComponent<Animator>();
    }

    void Update() { }

    public void OnAttackButton()
    {

        // sphere.GetComponent<MeshRenderer>().material.color = Color.blue;


        //idoutest
        //  player.transform.position = new Vector3(0, 5, 0); ;



        //Attack01に遷移する
        // player.GetComponent<Animator>().SetBool(key_isAttack01, true);

        animator.Play("Attack01",-1,0f);

    }

}
