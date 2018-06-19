using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LeaderboardManager : MonoBehaviour
{

    [SerializeField]
    public Leaderboard leaderboard;

    public GameObject rowPrefab;

    public Transform content;

    void Start()
    {
        //PlayerPrefs.DeleteAll();

        string s = PlayerPrefs.GetString("save", "");
        print(s);

        if (s == "")
        {
            leaderboard = new Leaderboard();
        }
        else
        {
            leaderboard = JsonUtility.FromJson<Leaderboard>(s);
        }
    }

    public void SaveNewHighscore(string lifes, string points, int naveID)
    {
        LeaderboardRow row = new LeaderboardRow();
        row.stringLifes = lifes;
        row.stringPoints = points;
        row.idNave = naveID;

        if (leaderboard.rows == null)
        {
            leaderboard.rows = new List<LeaderboardRow>();
        }

        leaderboard.rows.Add(row);

        StartCoroutine(CoSave());
    }

    IEnumerator CoSave()
    {
        yield return null;

        print(JsonUtility.ToJson(leaderboard));

        PlayerPrefs.SetString("save", JsonUtility.ToJson(leaderboard));
        PlayerPrefs.Save();
    }

    public void ResetLeaderboardUI()
    {
        foreach (Transform t in content)
        {
            Destroy(t.gameObject);
        }

        content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 40 + (leaderboard.rows.Count * 150));

        leaderboard.rows = leaderboard.rows.OrderByDescending(x => x.stringPoints).ThenBy(x => x.stringLifes).ToList();

        for (int i = 0; i < leaderboard.rows.Count; i++)
        {
            GameObject row = Instantiate(rowPrefab) as GameObject;
            row.GetComponent<LeaderboardRowScript>().row = leaderboard.rows[i];
            row.GetComponent<LeaderboardRowScript>().ResetValues();

            row.transform.SetParent(content);
            row.transform.localScale = Vector3.one;
        }

    }

}