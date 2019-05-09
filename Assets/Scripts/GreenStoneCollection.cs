using UnityEngine;
using System.Collections;

public class GreenStoneCollection : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag=="Player") {
			GameManager.gm.CollectGreenStone (1, true);
			Destroy (gameObject);
		}
	}
}
