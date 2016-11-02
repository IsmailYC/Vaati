using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject barrier;
	public GameObject freezeImage;
	public GameObject beam;
	public GameObject[] hearts;
	public int life;
	public float refreshTime;
	public float[] barrierTimes;
	public float[] freezeTimes;
	public float beamTime;

	bool damaged= false;
	bool barriered= false;
	bool beaming= false;
	float barrierTime;
	float freezeTime;
	float timeToRefresh;
	float timeToBarrier;
	float timeToFreeze;
	float timeToBeam;
	Animator animator;
	// Use this for initialization
	void Start () {
		if (barrier == null)
			barrier = GameObject.Find ("Barrier");
		if (beam == null)
			beam = GameObject.Find ("Beam");
		
		animator = GetComponent<Animator> ();
		barrierTime = barrierTimes [PlayerPrefManager.GetBarrierBoostLvl()];
		freezeTime = freezeTimes [PlayerPrefManager.GetFreezeBoostLvl()];
	}
	
	// Update is called once per frame
	void Update () {
		switch (GameManager.gm.gameState) {
		case GameManager.gameStates.Play:
			if (damaged) {
				if (Time.time > timeToRefresh) {
					damaged = false;
					animator.SetTrigger ("Refresh");
				}
			}
			if (barriered) {
				if (Time.time > timeToBarrier) {
					barriered = false;
					barrier.SetActive (false);
				}
			}
			if (GameManager.gm.freezed) {
				if (Time.time > timeToFreeze) {
					GameManager.gm.freezed = false;
					freezeImage.SetActive (false);
				}
			}
			if (beaming) {
				if (Time.time > timeToBeam) {
					beaming = false;
					beam.SetActive (false);
				}
			}
			break;
		case GameManager.gameStates.Over:
			if (damaged) {
				damaged = false;
				animator.SetTrigger ("Refresh");
			}
			if (barriered) {
				barriered = false;
				barrier.SetActive (false);
			}
			if (GameManager.gm.freezed) {
				GameManager.gm.freezed = false;
				freezeImage.SetActive (false);
			}
			if (beaming) {
				beaming = false;
				beam.SetActive (false);
			}
			break;
		default:
			break;
		}

	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Enemy") {
			if (!damaged) {
				ReduceLife ();
			}
			Destroy (coll.gameObject);
		}
	}

	private void ReduceLife()
	{
		if (life > 0) {
			life = life - 1;
			hearts [life].SetActive (false);
			damaged = true;
			animator.SetTrigger ("Damage");
			timeToRefresh = Time.time+refreshTime;
		} else
			GameManager.gm.EndGame();
	}

	public bool ActivateBarrier()
	{
		if (barriered)
			return false;
		else {
			barriered = true;
			barrier.SetActive (true);
			timeToBarrier = Time.time+barrierTime;
			return true;
		}
	}

	public void ActivateFreeze()
	{
		freezeImage.SetActive (true);
		timeToFreeze = Time.time+freezeTime;
	}

	public bool RestoreLife()
	{
		if (life == 3)
			return false;
		else {
			life = 3;
			for (int i = 0; i < hearts.Length; i++)
				hearts [i].SetActive (true);
			return true;
		}
	}

	public bool ActivateBeam()
	{
		if (beaming)
			return false;
		else {
			beaming = true;
            timeToBeam = Time.time + beamTime;
			beam.SetActive (true);
			return true;
		}
	}
}
