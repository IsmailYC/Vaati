using UnityEngine;
using System.Collections;

public class BlueStoneCollection : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag=="Player") {
			GameManager.gm.CollectBlueStone (1, true);
			Destroy (gameObject);
		}
	}
}
