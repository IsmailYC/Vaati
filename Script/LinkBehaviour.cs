using UnityEngine;
using System.Collections;

public class LinkBehaviour : MonoBehaviour {
	public float[] damageBoosts;
	public int[] scoreBoosts;
	public float damageSpeed;
	public float life;
	public int score;

	float damageBoost;
	int scoreBoost;
	bool takingDamage;
	GoToOrigin2 movement;
	float speed;
	// Use this for initialization
	void Start () {
		damageBoost = damageBoosts [PlayerPrefManager.GetDamageBoostLvl()];
		scoreBoost = scoreBoosts [PlayerPrefManager.GetScoreBoostLvl()];
		movement= gameObject.GetComponent<GoToOrigin2>();
		speed = movement.speed;
	}
	
	// Update is called once per frame
	void Update () {
		switch (GameManager.gm.gameState) {
		case GameManager.gameStates.Play:
			if (takingDamage) {
				life = life - damageBoost * damageSpeed * Time.deltaTime;
				if (life < 0) {
					GameManager.gm.Collect (score * scoreBoost);
					Destroy (gameObject);
				}
			}
			break;
		case GameManager.gameStates.Over:
			Destroy (gameObject);
			break;
		default:
			break;
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Laser")
		{
			takingDamage = true;
			movement.speed = speed/2.0f;
		}
		if (coll.gameObject.tag == "Barrier" || coll.gameObject.tag=="Beam") {
			GameManager.gm.Collect (score*scoreBoost);
			Destroy (gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Laser")
		{
			takingDamage = false;
			movement.speed= speed;
		}
	}
}
