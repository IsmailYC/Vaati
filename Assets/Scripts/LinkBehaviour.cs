using UnityEngine;
using System.Collections;

public class LinkBehaviour : MonoBehaviour {
	public float[] damageBoosts;
	public int[] scoreBoosts;
	public float damageSpeed;
	public float life;
	public int score;
    public float damage;

    public AudioClip creationClip;
    public AudioClip damageClip;
    public AudioClip deathClip;

    Animator animator;
	float damageBoost;
	int scoreBoost;
	bool takingDamage;
	GoToOrigin movement;
	float speed;
    AudioSource audioSource;

	// Use this for initialization
	void Start () {
		damageBoost = damageBoosts [PlayerPrefManager.GetDamageBoostLvl()];
		scoreBoost = scoreBoosts [PlayerPrefManager.GetScoreBoostLvl()];
		movement= gameObject.GetComponent<GoToOrigin>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
		speed = movement.speed;

        audioSource.volume = GameManager.gm.sfxVolume / 5f;
        if (creationClip != null)
            audioSource.PlayOneShot(creationClip);
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.gm.freezed && movement.speed > 0)
        {
            movement.speed = 0;
            animator.speed = 0f;
        }
        else if (!GameManager.gm.freezed && movement.speed == 0)
        {
            animator.speed = 1f;
            movement.speed = speed;
        }
		switch (GameManager.gm.gameState) {
		case GameManager.gameStates.Play:
			if (takingDamage) {
				life = life - damageBoost * damageSpeed * Time.deltaTime;
				if (life < 0) {
					GameManager.gm.Collect (score * scoreBoost);
					Destroy (gameObject);
				}
			}
			break;
		case GameManager.gameStates.Over:
			Destroy (gameObject);
			break;
		default:
			break;
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Laser")
		{
			takingDamage = true;
			movement.speed = speed/2.0f;
            animator.speed = 0.5f;
            if (damageClip != null)
            {
                audioSource.clip = damageClip;
                audioSource.Play();
            }
        }
		if (coll.gameObject.tag == "Barrier" || coll.gameObject.tag=="Beam") {
            if (deathClip != null)
                audioSource.PlayOneShot(deathClip);
			GameManager.gm.Collect (score*scoreBoost);
			Destroy (gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Laser")
		{
			takingDamage = false;
			movement.speed= speed;
            animator.speed = 1f;
            if (audioSource.clip != null)
                audioSource.clip = null;
        }
	}
}
