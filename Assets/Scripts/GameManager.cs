using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public enum gameStates {Menu, Store, Play, Pause, Over};
	public static GameManager gm;

	public GameObject camera1;
	public GameObject player;

	public GameObject menuCanvas;
    public GameObject settingsCanvas;
    public Slider bgmVolumeSlider;
    public Slider sfxVolumeSlider;
    public GameObject tipCanvas;
	public GameObject promptCanvas;
	public GameObject storeCanvas;
	public Text spiritStoneStoreDisplay;
	public Text barrierBoostDisplay;
	public Text freezeBoostDisplay;
	public Text timeBoostDisplay;
	public Text damageBoostDisplay;
	public Text scoreBoostDisplay;
	public Text purpleStoneStoreDisplay;
	public Text greenStoneStoreDisplay;
	public Text blueStoneStoreDisplay;
	public GameObject mainCanvas;
	public Text mainScoreDisplay;
	public GameObject pauseCanvas;
	public Text purpleStonePauseDisplay;
	public Text greenStonePauseDisplay;
	public Text redStonePauseDisplay;
	public Text blueStonePauseDisplay;
	public GameObject overCanvas;
	public Text overScoreDisplay;
	public Text overHighScoreDisplay;

    public AudioClip menuClip;
    public AudioClip mainClip;
    public AudioClip overClip;
    public AudioClip collectClip;

    public int bgmVolume;
    public int sfxVolume;
    public int score=0;
	public int highscore=0;
	public int purpleStones = 0;
	public int greenStones= 0;
	public int blueStones= 0;
	public int redStones= 0;
	public int spiritStones=0;
	public int timeBoostLvl = 0;
	public int barrierBoostLvl = 0;
	public int freezeBoostLvl= 0;
	public int damageBoostLvl = 0;
	public int scoreBoostLvl = 0;
	public gameStates gameState=gameStates.Menu;
	public bool freezed;

    bool touchWasDown;
    Vector2 touchStart;
    bool newHigh;
	LookAtPointer lp;
	PlayerController pc;
    TipCanvasScript tpc;
	Animator cameraAnimator;
	Animator playerAnimator;
    AudioSource audioSource;
	// Use this for initialization
	void Start () {
		if (gm == null)
			gm = GetComponent<GameManager> ();

		if (camera1 == null)
			camera1 = GameObject.Find ("MainCamera");
		cameraAnimator = camera1.GetComponent<Animator> ();

		if (player == null)
			player = GameObject.Find ("Eye");
		playerAnimator = player.GetComponent<Animator> ();

        bgmVolume = PlayerPrefManager.GetBGMVolume();
        sfxVolume = PlayerPrefManager.GetSFXVolume();
		spiritStones = PlayerPrefManager.GetSpiritStone ();
		highscore = PlayerPrefManager.GetHighScore ();
		barrierBoostLvl = PlayerPrefManager.GetBarrierBoostLvl ();
		freezeBoostLvl = PlayerPrefManager.GetFreezeBoostLvl ();
		timeBoostLvl = PlayerPrefManager.GetTimeBoostLvl ();
		damageBoostLvl = PlayerPrefManager.GetDamageBoostLvl ();
		scoreBoostLvl = PlayerPrefManager.GetScoreBoostLvl ();
		purpleStones = PlayerPrefManager.GetPurpleStones();
		greenStones = PlayerPrefManager.GetGreenStones ();
		blueStones = PlayerPrefManager.GetBlueStones ();

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = bgmVolume / 5f;
        audioSource.clip = menuClip;
        audioSource.Play();
        newHigh = false;
        freezed = false;
        touchWasDown = false;

		lp = player.GetComponent<LookAtPointer> ();
		pc = player.GetComponent<PlayerController> ();
        tpc = tipCanvas.GetComponent<TipCanvasScript>();
	}
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        switch (gameState)
        {
            case gameStates.Menu:
                if (Input.GetButtonDown("Cancel"))
                    promptCanvas.SetActive(true);
                break;
            case gameStates.Play:
                if (Input.GetButtonDown("Cancel"))
                {
                    if (tpc.tipOn)
                        tpc.CloseTip();
                    else
                        PauseGame();
                }
                break;
            case gameStates.Pause:
                if (Input.GetButtonDown("Cancel"))
                    ResumeGame();
                break;
            case gameStates.Store:
                if (Input.GetButtonDown("Cancel"))
                {
                    Debug.Log("Escape button detected");
                    if (tpc.tipOn)
                    {
                        Debug.Log("Tip is on trying to shut it down");
                        tpc.CloseTip();
                    }
                    else
                    {
                        Debug.Log("No tip detected closing the store");
                        CloseStore();
                    }
                }
                break;
            default:
                break;
        }
#else
        switch (gameState)
        {
            case gameStates.Menu:
                if (Input.GetButtonDown("Cancel"))
                    promptCanvas.SetActive(true);
                break;
            case gameStates.Play:
                if (Input.touchCount > 0)
                {
                    if (Input.touches[0].phase == TouchPhase.Began)
                    {
                        touchWasDown = true;
                        touchStart = Input.touches[0].position;
                    }
                    if (Input.touches[0].phase == TouchPhase.Ended && touchWasDown)
                    {
                        Vector2 touchTranslation = Input.touches[0].position - touchStart;
                        if (tpc.tipOn && touchTranslation.magnitude<1)
                            tpc.CloseTip();
                    }
                }
                break;
            case gameStates.Store:
                if (Input.touchCount > 0)
                {
                    if (Input.touches[0].phase == TouchPhase.Began)
                    {
                        touchWasDown = true;
                        touchStart = Input.touches[0].position;
                    }
                    if (Input.touches[0].phase == TouchPhase.Ended && touchWasDown)
                    {
                        Vector2 touchTranslation = Input.touches[0].position - touchStart;
                        if (touchTranslation.magnitude < 1)
                        {
                            if (tpc.tipOn)
                                tpc.CloseTip();
                            else
                                CloseStore();
                        }
                    }
                }
                break;
            default:
                break;
        }
#endif
    }

    public void Collect(int c)
	{
		score = score + c;
		mainScoreDisplay.text = score.ToString ();
		if (score > highscore) {
			highscore = score;
			newHigh = true;
		}
	}

	public void StartGame()
	{
		StartCoroutine (Starting ());
	}

	IEnumerator Starting()
	{
        audioSource.clip = null;
		menuCanvas.SetActive (false);
        pc.Transform();
		cameraAnimator.SetTrigger ("Start");
		yield return new WaitForSeconds (3.5f);
		gameState = gameStates.Play;
		mainCanvas.SetActive (true);
		Collect (0);
		CollectRedStone (0,false);
		CollectBlueStone (0, false);
		CollectPurpleStone(0, false);
		CollectGreenStone(0, false);
        touchWasDown = false;
        if (PlayerPrefManager.GetStartTip())
        {
            PlayerPrefManager.SetStartTip(false);
            tpc.ShowTip(0);
        }
        audioSource.clip = mainClip;
        audioSource.Play();
	}

    public void PsuedoEndGame()
    {
        lp.RestorePosition();
        audioSource.clip = null;
        gameState = gameStates.Over;
        mainCanvas.SetActive(false);
    }
	public void EndGame()
	{
        audioSource.clip = overClip;
        audioSource.Play();
		overCanvas.SetActive (true);
		overScoreDisplay.text = score.ToString ();
		if (newHigh) {
			overHighScoreDisplay.text= "New Record";
		} else {
			overHighScoreDisplay.text = highscore.ToString ();
		}
		score = 0;
		redStones = 0;
		newHigh = false;
        touchWasDown = false;
	}

	public void ReplayGame()
	{
        if (pauseCanvas.activeSelf)
            ResumeGame();
        else
        {
            overCanvas.SetActive(false);
            gameState = gameStates.Play;
            touchWasDown = false;
            mainCanvas.SetActive(true);
            audioSource.clip = mainClip;
            audioSource.Play();
        }
		pc.RestoreLife ();
		Collect (0);
	}

	public void RestartGame()
	{
		PlayerPrefManager.SetHighScore (highscore);
		PlayerPrefManager.SetSpiritStone (spiritStones);
		PlayerPrefManager.SetPurpleStones (purpleStones);
		PlayerPrefManager.SetGreenStones (greenStones);
		PlayerPrefManager.SetBlueStones (blueStones);
        if(pauseCanvas.activeSelf)
            Time.timeScale = 1.0f;
        SceneManager.LoadScene ("main");
	}

	public void ActivateBarrier()
	{
		if (purpleStones > 0) {
			if (pc.ActivateBarrier ()) {
				CollectPurpleStone(-1, false);
				ResumeGame ();
			}
		}
	}

	public void ActivateFreeze()
	{
		if (greenStones > 0) {
			if (!freezed) {
				CollectGreenStone (-1, false);
				freezed = true;
				pc.ActivateFreeze ();
				ResumeGame ();
			}
		}
	}

	public void RestorePlayer()
	{
		if (blueStones > 0) {
			if (pc.RestoreLife ()) {
				CollectBlueStone (-1, false);
				ResumeGame ();
			}
		}
	}

	public void ActivateBeam()
	{
		if (redStones > 0) {
			if (pc.ActivateBeam ()) {
				CollectRedStone (-1, false);
				ResumeGame ();
			}
		}
	}

	public void CollectPurpleStone(int i, bool tip){
        if(tip && PlayerPrefManager.GetPurpleStoneTip())
        {
            PlayerPrefManager.SetPurpleStoneTip(false);
            tpc.ShowTip(1);
        }
        if(i > 0)
            audioSource.PlayOneShot(collectClip, sfxVolume/5f);
        purpleStones +=i;
		purpleStonePauseDisplay.text = purpleStones.ToString ();
	}

	public void CollectGreenStone(int i, bool tip)
	{
        if (tip && PlayerPrefManager.GetGreenStoneTip())
        {
            PlayerPrefManager.SetGreenStoneTip(false);
            tpc.ShowTip(2);
        }
        if (i > 0)
            audioSource.PlayOneShot(collectClip, sfxVolume / 5f);
        greenStones +=i;
		greenStonePauseDisplay.text = greenStones.ToString ();
    }

	public void CollectBlueStone(int i, bool tip)
	{
        if (tip && PlayerPrefManager.GetBlueStoneTip())
        {
            PlayerPrefManager.SetBlueStoneTip(false);
            tpc.ShowTip(3);
        }
        if (i > 0)
            audioSource.PlayOneShot(collectClip, sfxVolume / 5f);
        blueStones +=i;
		blueStonePauseDisplay.text = blueStones.ToString ();
	}

	public void CollectRedStone(int i, bool tip)
	{
        if (tip && PlayerPrefManager.GetRedStoneTip())
        {
            PlayerPrefManager.SetRedStoneTip(false);
            tpc.ShowTip(4);
        }
        if (i > 0)
            audioSource.PlayOneShot(collectClip, sfxVolume / 5f);
        redStones +=i;
		redStonePauseDisplay.text = redStones.ToString ();
	}

	public void CollectSpiritStone(int i, bool tip)
	{
        if (tip && PlayerPrefManager.GetStoreTip())
        {
            PlayerPrefManager.SetStoreTip(false);
            tpc.ShowTip(5);
        }
        if (i > 0)
            audioSource.PlayOneShot(collectClip, sfxVolume / 5f);
        spiritStones += i;
	}

	public void PauseGame()
	{
		Time.timeScale = 0.0f;
		gameState = gameStates.Pause;
        touchWasDown = false;
		mainCanvas.SetActive (false);
		pauseCanvas.SetActive (true);
	}

	public void ResumeGame()
	{
		pauseCanvas.SetActive (false);
		mainCanvas.SetActive (true);
		gameState = gameStates.Play;
        touchWasDown = false;
		Time.timeScale = 1.0f;
	}

	public void OpenStore()
	{
		menuCanvas.SetActive (false);
		spiritStoneStoreDisplay.text = spiritStones.ToString ();
		barrierBoostDisplay.text = barrierBoostLvl.ToString ();
		freezeBoostDisplay.text = freezeBoostLvl.ToString ();
		timeBoostDisplay.text = timeBoostLvl.ToString ();
		damageBoostDisplay.text = damageBoostLvl.ToString ();
		scoreBoostDisplay.text = scoreBoostLvl.ToString ();
		purpleStoneStoreDisplay.text = purpleStones.ToString ();
		greenStoneStoreDisplay.text = greenStones.ToString ();
		blueStoneStoreDisplay.text = blueStones.ToString ();
		storeCanvas.SetActive (true);
		gameState = gameStates.Store;
        touchWasDown = false;
        if (PlayerPrefManager.GetStoreTip())
        {
            PlayerPrefManager.SetStoreTip(false);
            tpc.ShowTip(5);
        }
    }

	public void CloseStore()
	{
		PlayerPrefManager.SetSpiritStone (spiritStones);
		PlayerPrefManager.SetBarrierBoostLvl (barrierBoostLvl);
		PlayerPrefManager.SetFreezeBoostLvl (freezeBoostLvl);
		PlayerPrefManager.SetTimeBoostLvl (timeBoostLvl);
		PlayerPrefManager.SetDamageBoostLvl (damageBoostLvl);
		PlayerPrefManager.SetScoreBoostLvl (scoreBoostLvl);
		PlayerPrefManager.SetPurpleStones (purpleStones);
		PlayerPrefManager.SetGreenStones (greenStones);
		PlayerPrefManager.SetBlueStones (blueStones);
		storeCanvas.SetActive (false);
		menuCanvas.SetActive (true);
		gameState = gameStates.Menu;
        touchWasDown = false;
	}

	public void UpgradeScore()
	{
        touchWasDown = false;
		if (spiritStones > 0) {
			if (scoreBoostLvl < 5) {
				spiritStones--;
				spiritStoneStoreDisplay.text = spiritStones.ToString ();
				scoreBoostLvl++;
				scoreBoostDisplay.text = scoreBoostLvl.ToString ();
			}
		}
	}

	public void UpgradeTime()
	{
        touchWasDown = false;
        if (spiritStones > 0) {
			if (timeBoostLvl < 5) {
				spiritStones--;
				spiritStoneStoreDisplay.text = spiritStones.ToString ();
				timeBoostLvl++;
				timeBoostDisplay.text = timeBoostLvl.ToString ();
			}
		}
	}

	public void UpgradeDamage()
	{
        touchWasDown = false;
        if (spiritStones > 0) {
			if (damageBoostLvl < 5) {
				spiritStones--;
				spiritStoneStoreDisplay.text = spiritStones.ToString ();
				damageBoostLvl++;
				damageBoostDisplay.text = damageBoostLvl.ToString ();
			}
		}
	}

	public void UpgradeBarrier()
	{
        touchWasDown = false;
        if (spiritStones > 0) {
			if (barrierBoostLvl < 5) {
				spiritStones--;
				spiritStoneStoreDisplay.text = spiritStones.ToString ();
				barrierBoostLvl++;
				barrierBoostDisplay.text = barrierBoostLvl.ToString ();
			}
		}
	}

	public void UpgradeFreeze()
	{
        touchWasDown = false;
        if (spiritStones > 0) {
			if (freezeBoostLvl < 5) {
				spiritStones--;
				spiritStoneStoreDisplay.text = spiritStones.ToString ();
				freezeBoostLvl++;
				freezeBoostDisplay.text = freezeBoostLvl.ToString ();
			}
		}
	}

	public void BuyPurpleStone()
	{
        touchWasDown = false;
        if (spiritStones > 0) {
			spiritStones--;
			spiritStoneStoreDisplay.text = spiritStones.ToString ();
			purpleStones++;
			purpleStoneStoreDisplay.text = purpleStones.ToString ();
		}
	}

	public void BuyGreenStone()
	{
        touchWasDown = false;
        if (spiritStones > 0) {
			spiritStones--;
			spiritStoneStoreDisplay.text = spiritStones.ToString ();
			greenStones++;
			greenStoneStoreDisplay.text = greenStones.ToString ();
		}
	}

	public void BuyBlueStone()
	{
        touchWasDown = false;
        if (spiritStones > 0) {
			spiritStones--;
			spiritStoneStoreDisplay.text = spiritStones.ToString ();
			blueStones++;
			blueStoneStoreDisplay.text = blueStones.ToString ();
		}
	}

	public void CloseGame()
	{
		PlayerPrefs.Save ();
		Application.Quit ();
	}

    public void OpenSettings()
    {
        bgmVolumeSlider.value = bgmVolume;
        sfxVolumeSlider.value = sfxVolume;
        settingsCanvas.SetActive(true);
    }

    public void ApplySettings()
    {
        sfxVolume =(int) sfxVolumeSlider.value;
        bgmVolume = (int)bgmVolumeSlider.value;
        audioSource.volume = bgmVolume / 5f;
        PlayerPrefManager.SetBGMVolume(bgmVolume);
        PlayerPrefManager.SetSFXVolume(sfxVolume);
        settingsCanvas.SetActive(false);
    }

    public void CancelSettings()
    {
        settingsCanvas.SetActive(false);
    }
	public void CancelPrompt()
	{
		promptCanvas.SetActive (false);
	}
}
