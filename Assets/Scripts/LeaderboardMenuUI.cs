using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardItemPrefab;
    [SerializeField] private Transform container;
    [SerializeField] private Color firstItemColor;
    [SerializeField] private Color secondItemColor;
    [SerializeField] private Color thirdItemColor;
    [SerializeField] private Color otherItemColor;

    private void Awake()
    {
        if (SaveManager.Instance == null) return;

        List<PlayerData> playerList = SaveManager.Instance.GetPlayerList();

        for (int i = 0; i < playerList.Count; i++)
        {
            PlayerData playerData = playerList[i];

            GameObject itemGO = Instantiate(leaderboardItemPrefab, container);
            LeaderboardItem leaderboardItem = itemGO.GetComponent<LeaderboardItem>();

            Color color = GetItemColor(i);
            leaderboardItem.Setup(playerData, i + 1, color);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    private Color GetItemColor(int index)
    {
        Color color = Color.black;

        if (index == 0)
        {
            color = firstItemColor;
        }
        else if (index == 1)
        {
            color = secondItemColor;
        }
        else if (index == 2)
        {
            color = thirdItemColor;
        }
        else
        {
            color = otherItemColor;
        }

        return color;
    }
}
