using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderScript : MonoBehaviour
{

    public BorderType borderType;

    void Start()
    {
        switch (borderType)
        {
            case BorderType.bottom:
                transform.position = new Vector3(0, GameManager.instance.pontoZero.y - 1.25f, 0);
                transform.GetComponent<BoxCollider2D>().size = new Vector2(1.75f + Mathf.Abs(GameManager.instance.pontoZero.x * 2), 1);
                break;
            case BorderType.top:
                transform.position = new Vector3(0, (GameManager.instance.pontoZero.y * -1) + 1.25f, 0);
                transform.GetComponent<BoxCollider2D>().size = new Vector2(1.75f + Mathf.Abs(GameManager.instance.pontoZero.x * 2), 1);
                break;
            case BorderType.left:
                transform.position = new Vector3(GameManager.instance.pontoZero.x - 1.25f, 0, 0);
                transform.GetComponent<BoxCollider2D>().size = new Vector2(1, 1.75f + Mathf.Abs(GameManager.instance.pontoZero.y * 2));
                break;
            case BorderType.right:
                transform.position = new Vector3((GameManager.instance.pontoZero.x * -1) + 1.25f, 0, 0);
                transform.GetComponent<BoxCollider2D>().size = new Vector2(1, 1.75f + Mathf.Abs(GameManager.instance.pontoZero.y * 2));
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (borderType)
        {
            case BorderType.bottom:
                other.transform.position = new Vector3(other.transform.position.x, (GameManager.instance.pontoZero.y * -1) - 0.5f, 0);
                break;
            case BorderType.top:
                other.transform.position = new Vector3(other.transform.position.x, GameManager.instance.pontoZero.y + 0.5f, 0);
                break;
            case BorderType.left:
                other.transform.position = new Vector3((GameManager.instance.pontoZero.x * -1) - 0.5f, other.transform.position.y, 0);
                break;
            case BorderType.right:
                other.transform.position = new Vector3(GameManager.instance.pontoZero.x + 0.5f, other.transform.position.y, 0);
                break;
        }

    }

}
