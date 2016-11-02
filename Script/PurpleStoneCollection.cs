using UnityEngine;
using System.Collections;

public class PurpleStoneCollection : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag=="Player") {
			GameManager.gm.CollectPurpleStone(1, true);
			Destroy (gameObject);
		}
	}
}
