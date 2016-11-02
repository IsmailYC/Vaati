using UnityEngine;
using System.Collections;

public class ResetPrefScript : MonoBehaviour {
    public void ResetPlayerPrefs()
    {
        PlayerPrefManager.ResetPrefs();
    }
}
