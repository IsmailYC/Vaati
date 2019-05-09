using UnityEngine;
using System.Collections;

public class LookAtPointer : MonoBehaviour {

	private Vector2 mousePos;

	void Start () {
		mousePos = Vector2.down;
	}

	// Update is called once per frame
	void Update () {
        Vector2 tempPosition;
		switch (GameManager.gm.gameState) {
		case GameManager.gameStates.Play:
            #if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL || UNITY_EDITOR
                if (Input.GetMouseButton(0))
                {
                    tempPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (tempPosition.magnitude > 1.5f)
                        mousePos = tempPosition;
                }
            #else
			if (Input.touchCount > 0)
            {
			    tempPosition = Camera.main.ScreenToWorldPoint (Input.touches [Input.touchCount - 1].position);
                if (tempPosition.magnitude > 1.5f)
                        mousePos = tempPosition;
            }
            #endif
            float sign = Mathf.Sign (Vector3.Cross (-transform.up, mousePos).z);
			float speed = 10 * (sign * Vector2.Angle (-transform.up, mousePos));
			transform.Rotate (speed * Time.deltaTime * Vector3.forward);
			break;
		case GameManager.gameStates.Over:
			sign = Mathf.Sign (Vector3.Cross (transform.up, Vector2.up).z);
			speed = 10 * (sign * Vector2.Angle (transform.up, Vector2.up));
			transform.RotateAround (Vector3.zero, Vector3.forward, speed * Time.deltaTime);
			break;
		default:
			break;
		}
	}

	public void RestorePosition()
	{
		mousePos = Vector2.down;
	}
}
