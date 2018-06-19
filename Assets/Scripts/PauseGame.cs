using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{

    public bool isPaused = false;

    public GameObject panelPause;

    void Start()
    {
        GameManager.instance.pauseGame = this;
    }

    public void Pause()
    {
        if (GameManager.gameState == GameState.Menu)
            return;

        if (isPaused)
        {
            Time.timeScale = GameManager.instance.timeScaleNormal;
            GameManager.instance.audioSource.PlayOneShot(GameManager.instance.audioClipPause);
            panelPause.SetActive(false);
            isPaused = false;
        }
        else
        {
            Time.timeScale = GameManager.instance.timeScalePaused;
            GameManager.instance.audioSource.PlayOneShot(GameManager.instance.audioClipPause);
            panelPause.SetActive(true);
            isPaused = true;
        }
    }

}
