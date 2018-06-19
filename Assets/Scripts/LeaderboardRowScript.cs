using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardRowScript : MonoBehaviour
{

    public Text textLifes;
    public Text textPoints;
    public Image imageNave;

    public LeaderboardRow row;

    public void ResetValues()
    {
        textLifes.text = row.stringLifes.PadLeft(3, '0');
        textPoints.text = row.stringPoints.PadLeft(8, '0');
        imageNave.sprite = GameManager.instance.spriteNaveMini[row.idNave];
    }

    public void SetValues(string lifes, string points, int naveID)
    {
        row.stringLifes = lifes;
        row.stringPoints = points;
        row.idNave = naveID;

        textLifes.text = row.stringLifes.PadLeft(3, '0');
        textPoints.text = row.stringPoints.PadLeft(8, '0');
        imageNave.sprite = GameManager.instance.spriteNaveMini[row.idNave];
    }
}

[Serializable]
public class LeaderboardRow
{
    [SerializeField]
    public string stringLifes;
    [SerializeField]
    public string stringPoints;
    [SerializeField]
    public int idNave;
}

[Serializable]
public class Leaderboard
{
    [SerializeField]
    public List<LeaderboardRow> rows;
}