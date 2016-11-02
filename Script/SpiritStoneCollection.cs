using UnityEngine;
using System.Collections;

public class SpiritStoneCollection : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			GameManager.gm.CollectSpiritStone(1, true);
			Destroy (gameObject);
		}
	}
}
