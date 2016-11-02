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

	bool newHigh;
	LookAtPointer lp;
	PlayerController pc;
    TipCanvasScript tpc;
	Animator cameraAnimator;
	Animator playerAnimator;
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
		newHigh = false;

		lp = player.GetComponent<LookAtPointer> ();
		pc = player.GetComponent<PlayerController> ();
        tpc = tipCanvas.GetComponent<TipCanvasScript>();
	}
	
	// Update is called once per frame
	void Update () {
		switch (gameState) {
		case gameStates.Menu:
                if (Input.GetKeyDown(KeyCode.Escape))
				    promptCanvas.SetActive (true);
			break;
		case gameStates.Play:
                if (tpc.tipOn && Input.GetButtonDown("Cancel"))
                    tpc.CloseTip();
                else if (Input.GetKeyDown(KeyCode.Escape))
				    PauseGame ();
			break;
		case gameStates.Pause:
			if (Input.GetKeyDown (KeyCode.Escape))
				ResumeGame ();
			break;
		case gameStates.Store:
                if (tpc.tipOn && Input.GetButtonDown("Cancel"))
                    tpc.CloseTip();
                else if (Input.GetKeyDown(KeyCode.Escape))
				    CloseStore ();
			break;
		default:
			break;
		}
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
		menuCanvas.SetActive (false);
		playerAnimator.SetTrigger ("Transform");
		cameraAnimator.SetTrigger ("Start");
		yield return new WaitForSeconds (7.0f);
		gameState = gameStates.Play;
		mainCanvas.SetActive (true);
		Collect (0);
		CollectRedStone (0,false);
		CollectBlueStone (0, false);
		CollectPurpleStone(0, false);
		CollectGreenStone(0, false);
	}

	public void EndGame()
	{
		gameState = gameStates.Over;
		mainCanvas.SetActive (false);
		overCanvas.SetActive (true);
		lp.RestorePosition ();
		overScoreDisplay.text = score.ToString ();
		if (newHigh) {
			overHighScoreDisplay.text= "New Record";
			overHighScoreDisplay.color = Color.green;
		} else {
			overHighScoreDisplay.text = highscore.ToString ();
			overHighScoreDisplay.color = Color.red;
		}
		score = 0;
		redStones = 0;
		newHigh = false;
	}

	public void ReplayGame()
	{
		overCanvas.SetActive (false);
		pc.RestoreLife ();
		gameState = gameStates.Play;
		mainCanvas.SetActive (true);
		Collect (0);
	}

	public void RestartGame()
	{
		PlayerPrefManager.SetHighScore (highscore);
		PlayerPrefManager.SetSpiritStone (spiritStones);
		PlayerPrefManager.SetPurpleStones (purpleStones);
		PlayerPrefManager.SetGreenStones (greenStones);
		PlayerPrefManager.SetBlueStones (blueStones);
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
            tpc.ShowTip(0);
        }
		purpleStones+=i;
		purpleStonePauseDisplay.text = purpleStones.ToString ();
	}

	public void CollectGreenStone(int i, bool tip)
	{
        if (tip && PlayerPrefManager.GetGreenStoneTip())
        {
            PlayerPrefManager.SetGreenStoneTip(false);
            tpc.ShowTip(1);
        }
        greenStones +=i;
		greenStonePauseDisplay.text = greenStones.ToString ();
	}

	public void CollectBlueStone(int i, bool tip)
	{
        if (tip && PlayerPrefManager.GetBlueStoneTip())
        {
            PlayerPrefManager.SetBlueStoneTip(false);
            tpc.ShowTip(2);
        }
        blueStones +=i;
		blueStonePauseDisplay.text = blueStones.ToString ();
	}

	public void CollectRedStone(int i, bool tip)
	{
        if (tip && PlayerPrefManager.GetRedStoneTip())
        {
            PlayerPrefManager.SetRedStoneTip(false);
            tpc.ShowTip(3);
        }
        redStones +=i;
		redStonePauseDisplay.text = redStones.ToString ();
	}

	public void CollectSpiritStone(int i, bool tip)
	{
        if (tip && PlayerPrefManager.GetStoreTip())
        {
            PlayerPrefManager.SetStoreTip(false);
            tpc.ShowTip(4);
        }
        spiritStones += i;
	}

	public void PauseGame()
	{
		Time.timeScale = 0.0f;
		gameState = gameStates.Pause;
		mainCanvas.SetActive (false);
		pauseCanvas.SetActive (true);
	}

	public void ResumeGame()
	{
		pauseCanvas.SetActive (false);
		mainCanvas.SetActive (true);
		gameState = gameStates.Play;
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
        if (PlayerPrefManager.GetStoreTip())
        {
            PlayerPrefManager.SetStoreTip(false);
            tpc.ShowTip(4);
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
	}

	public void UpgradeScore()
	{
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
		if (spiritStones > 0) {
			spiritStones--;
			spiritStoneStoreDisplay.text = spiritStones.ToString ();
			purpleStones++;
			purpleStoneStoreDisplay.text = purpleStones.ToString ();
		}
	}

	public void BuyGreenStone()
	{
		if (spiritStones > 0) {
			spiritStones--;
			spiritStoneStoreDisplay.text = spiritStones.ToString ();
			greenStones++;
			greenStoneStoreDisplay.text = greenStones.ToString ();
		}
	}

	public void BuyBlueStone()
	{
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

	public void CancelPrompt()
	{
		promptCanvas.SetActive (false);
	}
}
