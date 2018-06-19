using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{

    public float shootSpeed = 1f;
    public float shootDuration = 2f;
    public Rigidbody2D rb2D;

    public bool isEnemyShoot;

    void OnEnable()
    {
        missShot = MissShot();
        StartCoroutine(missShot);
    }

    IEnumerator missShot;

    IEnumerator MissShot()
    {
        yield return null;
        rb2D.velocity = (transform.up * shootSpeed);

        yield return new WaitForSeconds(shootDuration);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BorderScript>())
            return;

        if (other.GetComponent<EnemyScript>() && !isEnemyShoot)
            other.GetComponent<EnemyScript>().TakeDamage();
        if (other.GetComponent<PlayerScript>() && isEnemyShoot)
            other.GetComponent<PlayerScript>().TakeDamage();

        StopCoroutine(missShot);
        gameObject.SetActive(false);
    }
}
