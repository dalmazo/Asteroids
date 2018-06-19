using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public GameObject prefabShoot;
    public Transform shootPoolParty; // [ lol \o\ /o/ ]
    List<GameObject> shootPool;

    public Rigidbody2D rb2D;
    public PolygonCollider2D col2D;
    public SpriteRenderer spritePlayer;

    public float playerAccelleration = 1;
    public float playerTorque = 1.5f;
    public float maxRotationSpeed = 2.5f;

    public SpriteRenderer spriteRightSteer;
    public SpriteRenderer spriteLeftSteer;

    public int lifes = 3;

    public float hyperspaceCharge = 1;

    int i = 0;
    bool needInstantiate = false;

    void Start()
    {
        GameManager.instance.playerScript = this;
        shootPool = new List<GameObject>();

        Physics2D.maxRotationSpeed = maxRotationSpeed;
        lifes = 3;

        GameManager.instance.currentGameScore = 0;
        GameManager.instance.CurrentGameScoreForLifes = 0;
    }

    public void RestartPlayer()
    {
        transform.position = Vector3.zero;
        transform.eulerAngles = Vector3.zero;

        rb2D.velocity = Vector3.zero;

        GameManager.instance.currentGameScore = 0;
        GameManager.instance.CurrentGameScoreForLifes = 0;
        lifes = 3;

        hyperspaceCharge = 1;
        GameManager.instance.hyperspaceScrollbar.size = hyperspaceCharge;

        for (i = 0; i < shootPool.Count; i++)
        {
            shootPool[i].gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        transform.position = Vector3.zero;
        transform.eulerAngles = Vector3.zero;

        GameManager.instance.inGameLifes.text = lifes.ToString().PadLeft(3, '0');

        hyperspaceCharge = 1;
        GameManager.instance.hyperspaceScrollbar.size = hyperspaceCharge;

    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        StartCoroutine(OnHit());
    }

    IEnumerator OnHit()
    {
        lifes--;
        GameManager.instance.inGameLifes.text = lifes.ToString().PadLeft(3, '0');
        spritePlayer.enabled = false;
        col2D.enabled = false;

        yield return new WaitForSeconds(.5f);

        if (lifes == 0)
        {
            GameManager.instance.panelEndGame.SetActive(true);
            GameManager.instance.textEndGameLifes.text = "000";
            GameManager.instance.textEndGameScore.text = GameManager.instance.currentGameScore.ToString().PadLeft(8, '0');
            GameManager.gameState = GameState.Menu;
            spritePlayer.enabled = true;
            col2D.enabled = true;

            GameManager.instance.leaderboardManager.SaveNewHighscore("0", GameManager.instance.currentGameScore.ToString(), GameManager.instance.choosedNave);
        }
        else
        {
            spritePlayer.enabled = true;

            transform.position = Vector3.zero;
            transform.eulerAngles = Vector3.zero;

            GameManager.instance.uiMensageText.gameObject.SetActive(true);

            yield return new WaitForSeconds(1.5f);

            GameManager.instance.uiMensageText.gameObject.SetActive(false);

            col2D.enabled = true;
        }
    }

    public void Fire()
    {

        if (GameManager.gameState != GameState.InGame)
            return;

        if (!spritePlayer.enabled)
            return;

        needInstantiate = true;

        GameManager.instance.audioSource.PlayOneShot(GameManager.instance.audioClipPew);

        for (i = 0; i < shootPool.Count; i++)
        {
            if (shootPool[i].activeInHierarchy == false)
            {
                shootPool[i].transform.position = transform.position;
                shootPool[i].transform.eulerAngles = transform.eulerAngles;
                shootPool[i].SetActive(true);
                needInstantiate = false;
                break;
            }
        }

        if (needInstantiate)
        {
            GameObject newShoot = Instantiate(prefabShoot) as GameObject;
            newShoot.transform.position = transform.position;
            newShoot.transform.eulerAngles = transform.eulerAngles;
            newShoot.transform.SetParent(shootPoolParty);
            newShoot.name = "Pew";

            shootPool.Add(newShoot);
        }
    }

    public void Steer(int direction)
    {
        if (GameManager.gameState != GameState.InGame)
            return;

        if (!spritePlayer.enabled)
        {
            spriteLeftSteer.enabled = false;
            spriteRightSteer.enabled = false;
            return;
        }

        rb2D.AddTorque(playerTorque * direction);

        spriteLeftSteer.enabled = (direction < 0);
        spriteRightSteer.enabled = (direction > 0);
    }

    public void MoveForward()
    {
        if (GameManager.gameState != GameState.InGame)
            return;

        if (!spritePlayer.enabled)
        {
            spriteLeftSteer.enabled = false;
            spriteRightSteer.enabled = false;
            return;
        }

        rb2D.AddForce(playerAccelleration * transform.up);
        spriteLeftSteer.enabled = true;
        spriteRightSteer.enabled = true;
    }


    public void GoHyperspace()
    {
        if (hyperspaceCharge >= 0.98f)
            StartCoroutine(HyperspaceCalc());
    }

    IEnumerator HyperspaceCalc()
    {
        hyperspaceCharge = 0;
        GameManager.instance.hyperspaceScrollbar.size = hyperspaceCharge;

        Time.timeScale = 0.01f;
        GameManager.instance.SetBG(GameManager.instance.bgPurple);
        yield return new WaitForSeconds(.001f);
        transform.position = new Vector3(1000, 0, 0);
        Time.timeScale = GameManager.instance.timeScaleNormal / 2;
        GameManager.instance.SetBG(GameManager.instance.bgDarkPurple);
        yield return new WaitForSeconds(.125f);
        Time.timeScale = GameManager.instance.timeScaleNormal;
        GameManager.instance.SetBG(GameManager.instance.bgBlue);
        transform.position = new Vector3(Random.Range(GameManager.instance.pontoZero.x, GameManager.instance.pontoZero.x * -1), Random.Range(GameManager.instance.pontoZero.y, GameManager.instance.pontoZero.y * -1), 0);
    }

    public void TakeDamage()
    {
        StartCoroutine(OnHit());
    }
}