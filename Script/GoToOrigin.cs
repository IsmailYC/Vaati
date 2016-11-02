using UnityEngine;
using System.Collections;

public class GoToOrigin : MonoBehaviour {
	
	public float speed;

	private Vector2 dir;
	// Use this for initialization
	void Start () {
		dir = -transform.position;
		dir.Normalize ();
	}
	
	// Update is called once per frame
	void Update () {
		switch (GameManager.gm.gameState) {
		case GameManager.gameStates.Play:
			transform.Translate (speed * dir * Time.deltaTime);
			break;
		case GameManager.gameStates.Over:
			Destroy (gameObject);
			break;
		default:
			break;
		}
	}
}
