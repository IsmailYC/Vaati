using UnityEngine;
using System.Collections;

public class LinkTimeBehaviour : MonoBehaviour {
	public int[] scoreBoosts;

	SpawnGameObject spawner;
	int scoreBoost;
	// Use this for initialization
	void Start () {
		spawner = gameObject.GetComponent<SpawnGameObject> ();
		scoreBoost = scoreBoosts [PlayerPrefManager.GetScoreBoostLvl()];
	}
	
	// Update is called once per frame
	void Update () {
		float nbrOfVirus = GameManager.gm.score / scoreBoost;
		float spawnTime = 3.0f * Mathf.Exp (-(1 / 200.0f) * nbrOfVirus);
		if (spawnTime > 0.1f)
			spawner.secondsBetweenSpawning = spawnTime;
	}
}
