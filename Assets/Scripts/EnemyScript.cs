using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public int defaultHealthPoints;
    public int currentHealthPoints;

    public int scoreOnKill;
    public Rigidbody2D rb2D;

    public float velocity;

    public EnemyType enemyType;

    public List<EnemyType> childsCreatedOnDestroy;

    public IEnumerator roleCoroutine;

    public void TakeDamage()
    {
        currentHealthPoints--;
        if (currentHealthPoints == 0)
        {
            gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        currentHealthPoints = defaultHealthPoints;


        roleCoroutine = RoleCoroutine();
        StartCoroutine(roleCoroutine);

        transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        rb2D.velocity = (transform.up * velocity);
    }

    public IEnumerator RoleCoroutine()
    {
        rb2D.velocity = (transform.up * velocity);

        switch (enemyType)
        {
            case EnemyType.ovniA:
            case EnemyType.ovniB:
            case EnemyType.ovniC:
            case EnemyType.ovniD:

                while (gameObject.activeInHierarchy)
                {
                    yield return null;
                    if (Vector2.Distance(GameManager.instance.playerScript.transform.position, transform.position) < 5)
                    {
                        if (Vector2.Distance(GameManager.instance.playerScript.transform.position, transform.position) < 3)
                        {
                            transform.up = GameManager.instance.playerScript.transform.position - transform.position;
                            rb2D.velocity = (transform.up * velocity);
                            yield return new WaitForSeconds(.5f);
                            GameManager.instance.enemyController.Fire(transform.position, transform.eulerAngles);
                            continue;
                        }
                        transform.up = GameManager.instance.playerScript.transform.position - transform.position;
                        transform.eulerAngles += new Vector3(0, 0, Random.Range(-45, 45));
                        rb2D.velocity = (transform.up * velocity);
                        GameManager.instance.enemyController.Fire(transform.position, transform.eulerAngles);
                        yield return new WaitForSeconds(1f);
                    }
                }

                break;
        }
        yield break;
    }


    void OnDisable()
    {
        StopCoroutine(roleCoroutine);

        if (currentHealthPoints != 0)
            return;

        if (GameManager.instance.playerScript.hyperspaceCharge <= 0.95f)
            GameManager.instance.playerScript.hyperspaceCharge += 0.05f;

        if (GameManager.instance.playerScript.hyperspaceCharge > 0.95f)
            GameManager.instance.playerScript.hyperspaceCharge = 1;

        GameManager.instance.hyperspaceScrollbar.size = GameManager.instance.playerScript.hyperspaceCharge;

        GameManager.instance.currentGameScore += scoreOnKill;
        GameManager.instance.CurrentGameScoreForLifes += scoreOnKill;
        GameManager.instance.inGameScore.text = GameManager.instance.currentGameScore.ToString().PadLeft(8, '0');

        foreach (EnemyType newborn in childsCreatedOnDestroy)
        {
            GameManager.instance.enemyController.CreateNewEnemy(newborn, transform.position);
        }

        GameManager.instance.enemyController.CurrentEnemysAlive--;
    }

}