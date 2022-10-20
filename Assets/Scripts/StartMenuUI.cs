using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartMenuUI : MonoBehaviour
{
    public static StartMenuUI Instance { get; private set; }

    [SerializeField] private TMP_InputField newPlayerNameInput;
    [SerializeField] private TMP_Text bestScoreOverall;
    [SerializeField] private GameObject playerItemPrefab;
    [SerializeField] private Transform container;

    private SaveManager _saveManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There are more than (1) StartMenuUI in scene.");
            Destroy(this);
            return;
        }

        _saveManager = SaveManager.Instance;

        DestroyAllPlayerItems();

        if (_saveManager.LoadAllPlayer())
        {
            if (_saveManager.GetPlayerList().Count > 0)
            {
                float nameFontSize = bestScoreOverall.fontSize + 5;
                // Player with the highest score
                PlayerData bestPlayer = _saveManager.GetPlayerList()[0];

                if (AllPlayersHasTheSameScore())
                {
                    bestScoreOverall.SetText($"<color=#EEE><size={nameFontSize}><b>All Players</b></size></color> have the same best score of <b><i>{bestPlayer.bestScore}</i></b>!");
                }
                else
                {
                    bestScoreOverall.SetText($"<color=#EEE><size={nameFontSize}><b>{bestPlayer.name}</b></size></color> has the highest score of <b><i>{bestPlayer.bestScore}</i></b>!");
                }
            }

            SpawnPlayerItem();
        }
        else
        {
            bestScoreOverall.SetText(string.Empty);
        }
    }

    private void StartGameWithSelectedPlayer(int id)
    {
        PlayerData player = _saveManager.LoadPlayer(id);
        StartGame(player);
    }

    public void StartGameWithNewPlayer()
    {
        PlayerData newPlayerData = new PlayerData
        {
            name = newPlayerNameInput.text,
            bestScore = 0
        };

        _saveManager.AddNewPlayer(newPlayerData);
        StartGame(newPlayerData);
    }

    private void StartGame(PlayerData currentPlayer)
    {
        MainManager.CurrentPlayer = currentPlayer;
        SceneManager.LoadScene("Main");
    }

    private void SpawnPlayerItem()
    {
        if (_saveManager.GetPlayerList() == null)
        {
            Debug.LogWarning("Players in SaveManager are null");
            return;
        }

        for (int i = 0; i < _saveManager.GetPlayerList().Count; i++)
        {
            PlayerData player = _saveManager.GetPlayerList()[i];

            GameObject itemGO = Instantiate(playerItemPrefab, container);
            PlayerItem playerItem = itemGO.GetComponent<PlayerItem>();
            playerItem.Setup(player, i);
            playerItem.OnClicked += StartGameWithSelectedPlayer;
        }
    }

    private bool AllPlayersHasTheSameScore()
    {
        List<PlayerData> playerList = _saveManager.GetPlayerList();

        for (int i = 0, l = playerList.Count; i < l; i++)
        {
            for (int j = 0; j < l; j++)
            {
                // If current(i) player's best score is not equal to other(j) player's
                if (playerList[i].bestScore != playerList[j].bestScore)
                    return false;
            }
        }

        // All player has the same score
        return true;
    }

    public void ViewLeaderboard()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    public void Exit()
    {
        _saveManager.SaveAllPlayer();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
    }

    private void DestroyAllPlayerItems()
    {
        for (int i = 0; i < container.childCount; i++)
        {
            Destroy(container.GetChild(i).gameObject);
        }
    }
}
