using UnityEngine;
using System.Collections;

public class TimeBoostBehaviour : MonoBehaviour {
	public float[] boostSpawnTimes;

	SpawnGameObject spawner;
	void Start () {
		spawner = gameObject.GetComponent<SpawnGameObject> ();
		spawner.secondsBetweenSpawning = boostSpawnTimes [PlayerPrefManager.GetTimeBoostLvl()];
	}
}
