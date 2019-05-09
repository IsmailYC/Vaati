using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject barrier;
	public GameObject freezeImage;
	public GameObject beam;
    public GameObject whitePanel;
	public GameObject[] heartShards;

    public AudioClip laughClip;
    public AudioClip transformClip;
    public AudioClip hurtClip;
    public AudioClip radicalBeamClip;
    public AudioClip lowHpClip;
    public AudioClip deathClip;

    public float life;
	public float refreshTime;
	public float[] barrierTimes;
	public float[] freezeTimes;
	public float beamTime;

	bool damaged= false;
	bool barriered= false;
	bool beaming= false;
	float barrierTime;
	float freezeTime;
	float timeToRefresh;
	float timeToBarrier;
	float timeToFreeze;
	float timeToBeam;

	Animator animator;
    AudioSource audioSource;

	// Use this for initialization
	void Start () {
		if (barrier == null)
			barrier = GameObject.Find ("Barrier");
		if (beam == null)
			beam = GameObject.Find ("Beam");
		
		animator = GetComponent<Animator> ();
        audioSource = GetComponent<AudioSource>();
		barrierTime = barrierTimes [PlayerPrefManager.GetBarrierBoostLvl()];
		freezeTime = freezeTimes [PlayerPrefManager.GetFreezeBoostLvl()];

        audioSource.volume = GameManager.gm.sfxVolume / 5f;
        audioSource.PlayOneShot(laughClip);
	}
	
	// Update is called once per frame
	void Update () {
		switch (GameManager.gm.gameState) {
		case GameManager.gameStates.Play:
			if (damaged) {
				if (Time.time > timeToRefresh) {
					damaged = false;
					animator.SetTrigger ("Refresh");
				}
			}
			if (barriered) {
				if (Time.time > timeToBarrier) {
					barriered = false;
					barrier.SetActive (false);
				}
			}
			if (GameManager.gm.freezed) {
				if (Time.time > timeToFreeze) {
					GameManager.gm.freezed = false;
					freezeImage.SetActive (false);
				}
			}
			if (beaming) {
				if (Time.time > timeToBeam) {
					beaming = false;
					beam.SetActive (false);
				}
			}
			break;
		case GameManager.gameStates.Over:
			if (damaged) {
				damaged = false;
				animator.SetTrigger ("Refresh");
			}
			if (barriered) {
				barriered = false;
				barrier.SetActive (false);
			}
			if (GameManager.gm.freezed) {
				GameManager.gm.freezed = false;
				freezeImage.SetActive (false);
			}
			if (beaming) {
				beaming = false;
				beam.SetActive (false);
			}
			break;
		default:
			break;
		}

	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Enemy") {
			if (!damaged) {
				ReduceLife(coll.gameObject.GetComponent<LinkBehaviour>().damage);
			}
			Destroy (coll.gameObject);
		}
	}

    public void Transform()
    {
        audioSource.PlayOneShot(transformClip, GameManager.gm.sfxVolume / 5f);
        animator.SetTrigger("Transform");
    }
 
	private void ReduceLife(float damage)
	{
        life = life - damage;
        if (life > 0)
        {
            audioSource.PlayOneShot(hurtClip, GameManager.gm.sfxVolume / 5f);
            if (life < 1)
            {
                if (audioSource.clip == null)
                {
                    audioSource.volume = GameManager.gm.sfxVolume / 5f;
                    audioSource.clip = lowHpClip;
                    audioSource.Play();
                }
            }
            int startShard = (int)(life * 4);
            int stopShard = (int)((life + damage) * 4);
            for (int i = startShard; i < stopShard; i++)
                heartShards[i].SetActive(false);
            damaged = true;
            animator.SetTrigger("Damage");
            timeToRefresh = Time.time + refreshTime;
        }
        else
        {
            audioSource.clip = null;
            GameManager.gm.PsuedoEndGame();
            whitePanel.transform.parent.gameObject.SetActive(true);
            audioSource.PlayOneShot(deathClip);
            animator.SetTrigger("Explode");
            Invoke("Fade", 0.5f);
            Invoke("EndGame", 1.5f);
            Invoke("DeactivateFadeCanvas", 2.5f);
        }
	}

    private void Fade()
    {
        whitePanel.GetComponent<Animator>().SetTrigger("Fade");
    }

    private void EndGame()
    {
        GameManager.gm.EndGame();
    }

    private void DeactivateFadeCanvas()
    {
        whitePanel.transform.parent.gameObject.SetActive(false);
    }

	public bool ActivateBarrier()
	{
		if (barriered)
			return false;
		else {
			barriered = true;
			barrier.SetActive (true);
			timeToBarrier = Time.time+barrierTime;
			return true;
		}
	}

	public void ActivateFreeze()
	{
		freezeImage.SetActive (true);
		timeToFreeze = Time.time+freezeTime;
	}

	public bool RestoreLife()
	{
		if (life == 3)
			return false;
		else {
			life = 3;
			for (int i = 0; i < heartShards.Length; i++)
				heartShards[i].SetActive (true);
            audioSource.clip = null;
			return true;
		}
	}

	public bool ActivateBeam()
	{
		if (beaming)
			return false;
		else {
            audioSource.PlayOneShot(radicalBeamClip, GameManager.gm.sfxVolume / 5f);
            beaming = true;
            timeToBeam = Time.time + beamTime;
			beam.SetActive (true);
			return true;
		}
	}
}
