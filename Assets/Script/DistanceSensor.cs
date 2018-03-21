using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceSensor : MonoBehaviour 
{
	/// <summary>
	/// インスタンス
	/// </summary>
	public static DistanceSensor instance;

    public const float NOTHING = -1;    // 計測不能

    public GameObject Canvas_flowerA;
    public GameObject Canvas_flowerB;
    public GameObject Canvas_flowerC;

    public float maxDistance = 5;      // 計測可能な最大距離
    public float distance;              // 計測距離

	private float interval = 1f;

	private float timer = 0;

	[SerializeField]
	private ParticleAttraction particle;
	public ParticleAttraction Particle { get {  return particle;} }

	[SerializeField]
	private GameObject loveTarget;
	public GameObject LoveTarget { get {  return loveTarget;} }

	[SerializeField]
	private ScoreController scoreController;
	public ScoreController ScoreController { get {  return scoreController;} }

	/// <summary>
	/// フォーカス中なしは -1
	/// </summary>
	/// <value>The index of the current forus.</value>
	public int currentFocusIndex { get; private set; }

	void Awake()
	{
		instance = this;
		currentFocusIndex = -1;
	}

    // 距離計測
    void Update()
    {
		// インターバルの感覚で処理
		timer += Time.deltaTime;
		if( timer < interval ) return;
		timer = 0;

        // 前方ベクトル計算
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        // 距離計算
        RaycastHit hit;
        if (Physics.Raycast(transform.position, fwd, out hit, maxDistance))
        {
            distance = hit.distance;

            //Rayが当たったオブジェクトのtagがPlayerだったら
			if (hit.collider.tag == "Target_Player")
			{
				currentFocusIndex = 0;
				//    Canvas_flowerA.SetActive(true);
				//    Canvas_flowerB.SetActive(false);
				//    Canvas_flowerC.SetActive(false);

				Debug.Log("Target_A:"+distance);
			}
			else if (hit.collider.tag == "Target_A")
            {
				currentFocusIndex = 1;
            //    Canvas_flowerA.SetActive(true);
            //    Canvas_flowerB.SetActive(false);
            //    Canvas_flowerC.SetActive(false);

                Debug.Log("Target_A:"+distance);
            }
            else if (hit.collider.tag == "Target_B")
            {
				currentFocusIndex = 2;
            //    Canvas_flowerA.SetActive(false);
            //    Canvas_flowerB.SetActive(true);
            //    Canvas_flowerC.SetActive(false);

                Debug.Log("Target_B:" + distance);
            }
            else if(hit.collider.tag == "Target_C")
            {
				currentFocusIndex = 3;

            //    Canvas_flowerA.SetActive(false);
            //    Canvas_flowerB.SetActive(false);
            //    Canvas_flowerC.SetActive(true);

                Debug.Log("Target_C:" + distance);
            }
            else
            {
				currentFocusIndex = -1;

            //    Canvas_flowerA.SetActive(false);
            //    Canvas_flowerB.SetActive(false);
            //    Canvas_flowerC.SetActive(false);
				Debug.Log("Something:" + distance);
            }

        }
        else
        {
			currentFocusIndex = -1;

			//    Canvas_flowerA.SetActive(false);
			//    Canvas_flowerB.SetActive(false);
			//    Canvas_flowerC.SetActive(false);

            distance = NOTHING;
        }

    }

	public void ShowEmoteEffect()
	{
		Debug.Log("インデックス" + currentFocusIndex.ToString() + " のプレイヤーにエモートを送信します ");

		var target = YvTuberController.tuberList.Find( t => t.BaseIndex == currentFocusIndex );
		if( target != null )
		{
			particle.Target = target.transform;
			particle.gameObject.SetActive( true );

			particle.GetComponent<ParticleSystem>().Play();
		}
	}
}
