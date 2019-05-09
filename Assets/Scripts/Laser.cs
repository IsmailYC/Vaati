using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

	public GameObject laserStart;
	public GameObject laserMiddle;
	public LayerMask enemyLayer;
    public float laserWidth;
    public float maxLaserLength;
    public float linkHeight;

	private GameObject start;
	private GameObject middle;

    void Start()
    {
        start = (GameObject) Instantiate(laserStart);
        start.transform.parent = transform;
        start.transform.localScale = laserWidth * Vector3.one;
        start.transform.localPosition = 0.25f*laserWidth*Vector3.up;
        start.transform.rotation = transform.rotation;
        middle = (GameObject) Instantiate(laserMiddle);
        middle.transform.parent = transform;
        middle.transform.localScale = laserWidth * Vector3.one;
        middle.transform.localPosition = 0.5f*laserWidth * Vector3.up;
        middle.transform.rotation = transform.rotation;
        start.SetActive(false);
        middle.SetActive(false);
    }

	void Update()
	{
		switch (GameManager.gm.gameState) {
            case GameManager.gameStates.Play:
                if(!start.activeInHierarchy)
                {
                    start.SetActive(true);
                    middle.SetActive(true);
                }
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, maxLaserLength, enemyLayer);
                if(hit)
                {
                    float distance = hit.distance;
                    middle.transform.localScale = new Vector3(laserWidth, distance + linkHeight, 1.0f);
                    middle.transform.localPosition = 0.5f*(distance + laserWidth + linkHeight) *Vector3.up;
                }
                else
                {
                    float distance = maxLaserLength;
                    middle.transform.localScale = new Vector3(laserWidth, distance, 1.0f);
                    middle.transform.localPosition = 0.5f * (distance + laserWidth) * Vector3.up;
                }
                break;
		default:
			break;
		}

	}
}
