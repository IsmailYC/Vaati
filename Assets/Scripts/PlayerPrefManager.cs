using UnityEngine;
using System.Collections;

public static class PlayerPrefManager{

    public static bool GetStartTip()
    {
        if (PlayerPrefs.HasKey("StartTip"))
            return PlayerPrefs.GetInt("StartTip") == 1;
        else
            return true;
    }

    public static bool GetPurpleStoneTip()
    {
        if (PlayerPrefs.HasKey("PurpleStoneTip"))
            return PlayerPrefs.GetInt("PurpleStoneTip") == 1;
        else
            return true;
    }

    public static bool GetGreenStoneTip()
    {
        if (PlayerPrefs.HasKey("GreenStoneTip"))
            return PlayerPrefs.GetInt("GreenStoneTip") == 1;
        else
            return true;
    }
    public static bool GetBlueStoneTip()
    {
        if (PlayerPrefs.HasKey("BlueStoneTip"))
            return PlayerPrefs.GetInt("BlueStoneTip") == 1;
        else
            return true;
    }

    public static bool GetRedStoneTip()
    {
        if (PlayerPrefs.HasKey("RedStoneTip"))
            return PlayerPrefs.GetInt("RedStoneTip") == 1;
        else
            return true;
    }

    public static bool GetStoreTip()
    {
        if (PlayerPrefs.HasKey("StoreTip"))
            return PlayerPrefs.GetInt("StoreTip") == 1;
        else
            return true;
    }

    public static int GetBGMVolume()
    {
        if (PlayerPrefs.HasKey("BGM Volume"))
            return PlayerPrefs.GetInt("BGM Volume");
        else
            return 3;
    }

    public static int GetSFXVolume()
    {
        if (PlayerPrefs.HasKey("SFX Volume"))
            return PlayerPrefs.GetInt("SFX Volume");
        else
            return 4;
    }

    public static int GetHighScore(){
		if (PlayerPrefs.HasKey ("HighScore"))
			return PlayerPrefs.GetInt ("HighScore");
		else
			return 0;
	}

	public static int GetPurpleStones(){
		if (PlayerPrefs.HasKey ("Purple Stones"))
			return PlayerPrefs.GetInt ("Purple Stones");
		else
			return 0;
	}

	public static int GetGreenStones(){
		if (PlayerPrefs.HasKey ("Green Stones"))
			return PlayerPrefs.GetInt ("Green Stones");
		else
			return 0;
	}

	public static int GetBlueStones()
	{
		if (PlayerPrefs.HasKey ("Blue Stones"))
			return PlayerPrefs.GetInt ("Blue Stones");
		else
			return 0;
	}

	public static int GetSpiritStone()
	{
		if (PlayerPrefs.HasKey ("Spirit Stone"))
			return PlayerPrefs.GetInt ("Spirit Stone");
		else
			return 0;
	}

	public static int GetTimeBoostLvl()
	{
		if (PlayerPrefs.HasKey ("Time Boost Level"))
			return PlayerPrefs.GetInt ("Time Boost Level");
		else
			return 0;
	}

	public static int GetDamageBoostLvl()
	{
		if (PlayerPrefs.HasKey ("Damage Boost Level"))
			return PlayerPrefs.GetInt ("Damage Boost Level");
		else
			return 0;
	}

	public static int GetScoreBoostLvl()
	{
		if (PlayerPrefs.HasKey ("Score Boost Level"))
			return PlayerPrefs.GetInt ("Score Boost Level");
		else
			return 0;
	}

	public static int GetBarrierBoostLvl()
	{
		if (PlayerPrefs.HasKey ("Barrier Boost Level"))
			return PlayerPrefs.GetInt ("Barrier Boost Level");
		else
			return 0;
	}

	public static int GetFreezeBoostLvl()
	{
		if (PlayerPrefs.HasKey ("Freeze Boost Level"))
			return PlayerPrefs.GetInt ("Freeze Boost Level");
		else
			return 0;
	}

    public static void SetStartTip(bool condition)
    {
        if (condition)
            PlayerPrefs.SetInt("StartTip", 1);
        else
            PlayerPrefs.SetInt("StartTip", 0);
    }

    public static void SetPurpleStoneTip(bool condition)
    {
        if (condition)
            PlayerPrefs.SetInt("PurpleStoneTip", 1);
        else
            PlayerPrefs.SetInt("PurpleStoneTip", 0);
    }

    public static void SetGreenStoneTip(bool condition)
    {
        if (condition)
            PlayerPrefs.SetInt("GreenStoneTip", 1);
        else
            PlayerPrefs.SetInt("GreenStoneTip", 0);
    }

    public static void SetBlueStoneTip(bool condition)
    {
        if (condition)
            PlayerPrefs.SetInt("BlueStoneTip", 1);
        else
            PlayerPrefs.SetInt("BlueStoneTip", 0);
    }

    public static void SetRedStoneTip(bool condition)
    {
        if (condition)
            PlayerPrefs.SetInt("RedStoneTip", 1);
        else
            PlayerPrefs.SetInt("RedStoneTip", 0);
    }

    public static void SetStoreTip(bool condition)
    {
        if (condition)
            PlayerPrefs.SetInt("StoreTip", 1);
        else
            PlayerPrefs.SetInt("StoreTip", 0);
    }

    public static void SetBGMVolume(int volume)
    {
        PlayerPrefs.SetInt("BGM Volume", volume);
    }

    public static void SetSFXVolume(int volume)
    {
        PlayerPrefs.SetInt("SFX Volume", volume);
    }

    public static void SetHighScore(int highscore)
	{
		PlayerPrefs.SetInt ("HighScore", highscore);
	}

	public static void SetPurpleStones(int purpleStones)
	{
		PlayerPrefs.SetInt ("Purple Stones", purpleStones);
	}

	public static void SetGreenStones(int greenStones)
	{
		PlayerPrefs.SetInt ("Green Stones", greenStones);
	}

	public static void SetBlueStones(int blueStones)
	{
		PlayerPrefs.SetInt ("Blue Stones", blueStones);
	}

	public static void SetSpiritStone(int spiritStones)
	{
		PlayerPrefs.SetInt ("Spirit Stone", spiritStones);
	}

	public static void SetTimeBoostLvl(int level)
	{
		PlayerPrefs.SetInt ("Time Boost Level", level);
	}

	public static void SetDamageBoostLvl(int level)
	{
		PlayerPrefs.SetInt ("Damage Boost Level", level);
	}

	public static void SetScoreBoostLvl(int level)
	{
		PlayerPrefs.SetInt ("Score Boost Level", level);
	}

	public static void SetBarrierBoostLvl(int level)
	{
		PlayerPrefs.SetInt ("Barrier Boost Level", level);
	}

	public static void SetFreezeBoostLvl(int level)
	{
		PlayerPrefs.SetInt ("Freeze Boost Level", level);
	}

	public static void ResetPrefs()
	{
		SetHighScore (0);
        SetPurpleStoneTip(true);
        SetGreenStoneTip(true);
        SetBlueStoneTip(true);
        SetRedStoneTip(true);
        SetStoreTip(true);
        SetTimeBoostLvl (0);
        SetScoreBoostLvl (0);
        SetDamageBoostLvl (0);
		SetBarrierBoostLvl (0);
		SetFreezeBoostLvl (0);
		SetPurpleStones (0);
		SetGreenStones (0);
		SetBlueStones (0);
	}
}
