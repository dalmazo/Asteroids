using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Primeiro na ordem de execução
public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public EnemyController enemyController;
    public PlayerScript playerScript;

    [Header("UI")]
    public GameObject panelEndGame;
    public Text textEndGameLifes;
    public Text textEndGameScore;
    public Image imgEndGameNave;
    public GameObject panelMainMenu;

    [Header("In Game")]
    public PauseGame pauseGame;
    public float timeScaleNormal = 1f;
    public float timeScalePaused = 0f;
    // 50 - 100 lifes
    public float timeScaleBugged = 0.3f;
    public Text inGameScore;
    public Text inGameLifes;
    public Scrollbar hyperspaceScrollbar;
    public Text uiMensageText;
    public Image imgInGameNave;

    [Header("SelectShip")]
    public Toggle toggleNaveA;
    public Toggle toggleNaveB;
    public Toggle toggleNaveC;
    public Toggle toggleNaveD;
    public int choosedNave;

    [Header("Nave Sprites")]
    public Sprite[] spriteNave;
    public Sprite[] spriteNaveMini;

    [Header("BG")]
    public SpriteRenderer gameBG;
    public Sprite bgBlack;
    public Sprite bgBlue;
    public Sprite bgDarkPurple;
    public Sprite bgPurple;

    [Header("Leaderboard")]
    public LeaderboardManager leaderboardManager;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip audioClipPew;
    public AudioClip audioClipMorte;
    public AudioClip audioClipHyperspace;
    public AudioClip audioClipPause;

    [Header("CurrentGame")]
    public int currentGameScore;
    private int currentGameScoreForLifes;
    public int CurrentGameScoreForLifes
    {
        get
        {
            return currentGameScoreForLifes;
        }
        set
        {
            if (value >= 10000)
            {
                playerScript.lifes++;

                //Little bug implementation
                if (playerScript.lifes > 50)
                {
                    timeScaleNormal = timeScaleBugged;
                    Time.timeScale = timeScaleNormal;
                }

                inGameLifes.text = playerScript.lifes.ToString().PadLeft(3, '0');
                value -= 10000;
            }
            currentGameScoreForLifes = value;
        }
    }
    public static GameState gameState;

    public Vector2 pontoZero;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        gameState = GameState.Menu;

        pontoZero = Camera.main.ScreenToWorldPoint(Vector3.zero);
        gameBG.size = new Vector2((Mathf.Abs(pontoZero.x * 2)), (Mathf.Abs(pontoZero.y * 2)));
    }

    public void ExitToMenu()
    {
        gameState = GameState.Menu;
        panelMainMenu.SetActive(true);

        playerScript.RestartPlayer();
        enemyController.RestartEnemies();
    }

    public void StartGame()
    {
        playerScript.RestartPlayer();
        enemyController.RestartEnemies();
        enemyController.CurrentEnemysAlive = 0;

        timeScaleNormal = 1f;
        Time.timeScale = timeScaleNormal;

        pauseGame.isPaused = false;

        currentGameScore = 0;
        currentGameScoreForLifes = 0;
        inGameScore.text = currentGameScore.ToString().PadLeft(8, '0');

        gameState = GameState.InGame;
    }

    public void SetBG(Sprite bg)
    {
        gameBG.sprite = bg;
    }

    public void SetToggleNave(int naveId)
    {
        choosedNave = naveId;

        playerScript.spritePlayer.sprite = spriteNave[naveId];
        imgInGameNave.sprite = spriteNaveMini[naveId];
        imgEndGameNave.sprite = spriteNaveMini[naveId];
    }

}
