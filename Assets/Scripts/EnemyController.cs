using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public List<Round> rounds;

    public List<List<GameObject>> enemiesPools;
    public List<GameObject> enemiesPrefabs;

    public Transform enemyPoolParty;

    void Start()
    {
        GameManager.instance.enemyController = this;

        enemiesPools = new List<List<GameObject>>();
        for (i = 0; i < enemiesPrefabs.Count; i++)
        {
            enemiesPools.Add(new List<GameObject>());
        }

        shootPool = new List<GameObject>();
        foreach (Transform t in shootPoolParty)
        {
            t.gameObject.SetActive(false);
        }

        currentEnemysAlive = 0;
        currentRound = 0;
    }

    int restartIndex = 0;
    public void RestartEnemies()
    {
        currentEnemysAlive = 0;
        currentRound = 0;
        for (restartIndex = 0; restartIndex < enemiesPools.Count; restartIndex++)
        {
            for (i = 0; i < enemiesPools[restartIndex].Count; i++)
            {
                enemiesPools[restartIndex][i].SetActive(false);
            }
        }
    }

    int i = 0;
    bool needInstantiate = false;
    public void CreateNewEnemy(EnemyType newbornType, Vector2 fatherPos)
    {
        needInstantiate = true;
        CurrentEnemysAlive++;

        for (i = 0; i < enemiesPools[(int)newbornType].Count; i++)
        {
            if (enemiesPools[(int)newbornType][i].activeInHierarchy == false)
            {
                enemiesPools[(int)newbornType][i].transform.position = fatherPos;
                enemiesPools[(int)newbornType][i].transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
                enemiesPools[(int)newbornType][i].SetActive(true);
                needInstantiate = false;
                break;
            }
        }
        if (needInstantiate)
        {
            GameObject newborn = Instantiate(enemiesPrefabs[(int)newbornType]) as GameObject;
            newborn.transform.position = fatherPos;
            newborn.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
            newborn.transform.SetParent(enemyPoolParty);
            newborn.name = newbornType.ToString();

            enemiesPools[(int)newbornType].Add(newborn);
        }
    }


    public GameObject prefabShoot;
    public Transform shootPoolParty; // [ lol \o\ /o/ ]
    List<GameObject> shootPool;
    public void Fire(Vector3 position, Vector3 angle)
    {
        needInstantiate = true;

        GameManager.instance.audioSource.PlayOneShot(GameManager.instance.audioClipPew);

        for (i = 0; i < shootPool.Count; i++)
        {
            if (shootPool[i].activeInHierarchy == false)
            {
                shootPool[i].transform.position = position;
                shootPool[i].transform.eulerAngles = angle;
                shootPool[i].SetActive(true);
                needInstantiate = false;
                break;
            }
        }

        if (needInstantiate)
        {
            GameObject newShoot = Instantiate(prefabShoot) as GameObject;
            newShoot.transform.position = position;
            newShoot.transform.eulerAngles = angle;
            newShoot.transform.SetParent(shootPoolParty);
            newShoot.name = "Pew";

            shootPool.Add(newShoot);
        }
    }

    public int currentEnemysAlive = 0;
    public int CurrentEnemysAlive
    {
        get
        {
            return currentEnemysAlive;
        }
        set
        {
            currentEnemysAlive = value;
            if (value == 0)
            {
                NextRound();
            }
        }
    }

    int currentRound = 0;
    public void NextRound()
    {
        currentRound++;

        if (currentRound == rounds.Count)
        {
            GameManager.instance.panelEndGame.SetActive(true);
            GameManager.instance.textEndGameLifes.text = GameManager.instance.playerScript.lifes.ToString().PadLeft(3, '0');
            GameManager.instance.textEndGameScore.text = GameManager.instance.currentGameScore.ToString().PadLeft(8, '0');

            GameManager.instance.leaderboardManager.SaveNewHighscore(GameManager.instance.playerScript.lifes.ToString(), GameManager.instance.currentGameScore.ToString(), GameManager.instance.choosedNave);
        }

        for (i = 0; i < rounds[currentRound].enemies.Count; i++)
        {
            CreateNewEnemy(rounds[currentRound].enemies[i], GetPointOnBorder());
        }

    }

    public Vector2 GetPointOnBorder()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                return new Vector2(GameManager.instance.pontoZero.x + 1, Random.Range(GameManager.instance.pontoZero.y, GameManager.instance.pontoZero.y * -1));
            case 1:
                return new Vector2((GameManager.instance.pontoZero.x * -1) - 1, Random.Range(GameManager.instance.pontoZero.y, GameManager.instance.pontoZero.y * -1));
            case 2:
                return new Vector2(Random.Range(GameManager.instance.pontoZero.x, GameManager.instance.pontoZero.x * -1), GameManager.instance.pontoZero.y + 1);
            case 3:
                return new Vector2(Random.Range(GameManager.instance.pontoZero.x, GameManager.instance.pontoZero.x * -1), (GameManager.instance.pontoZero.y * -1) - 1);
        }
        return new Vector2(0, 0);
    }

}

[System.Serializable]
public class Round
{
    [SerializeField]
    public List<EnemyType> enemies;
}