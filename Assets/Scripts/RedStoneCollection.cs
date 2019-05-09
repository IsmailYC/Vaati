using UnityEngine;
using System.Collections;

public class RedStoneCollection : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag=="Player") {
			GameManager.gm.CollectRedStone (1, true);
			Destroy (gameObject);
		}
	}
}
