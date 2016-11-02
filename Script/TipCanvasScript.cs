using UnityEngine;
using System.Collections;

public class TipCanvasScript : MonoBehaviour {
    public GameObject[] tips;
    public bool tipOn;

    float currentTimeScale;
    int currentTip;

    public void ShowTip(int i)
    {
        tips[i].SetActive(true);
        tipOn = true;
        currentTip = i;
        currentTimeScale = Time.timeScale;
        Time.timeScale = 0.0f;
    }

    public void CloseTip()
    {
        tips[currentTip].SetActive(false);
        tipOn = false;
        Time.timeScale = currentTimeScale;
    }
}
