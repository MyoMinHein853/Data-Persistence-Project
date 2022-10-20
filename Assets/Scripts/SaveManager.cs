using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[DefaultExecutionOrder(-1)]
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private AllPlayers allPlayers;
    private const string FILE_NAME = "Saved Data.json";
    private static string PATH { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        allPlayers ??= new AllPlayers();
        PATH = Path.Combine(Application.persistentDataPath, FILE_NAME);
    }

    public void AddNewPlayer(PlayerData data)
    {
        if (!allPlayers.PlayerList.Contains(data))
        {
            Debug.Log("New Player Added!");
            Instance.allPlayers.PlayerList.Add(data);
            SaveAllPlayer();
        }
        else
        {
            Debug.LogWarning("Player already existed!");
        }
    }

    public PlayerData LoadPlayer(int id)
    {
        if (id < 0 || id > Instance.allPlayers.PlayerList.Count)
        {
            return null;
        }

        return Instance.allPlayers.PlayerList[id];
    }

    public void SaveAllPlayer()
    {
        var json = JsonUtility.ToJson(Instance.allPlayers);

        File.WriteAllText(PATH, json);

        Debug.Log("Saved!");
    }

    public bool LoadAllPlayer()
    {
        if (File.Exists(PATH))
        {
            var json = File.ReadAllText(PATH);
            allPlayers = JsonUtility.FromJson<AllPlayers>(json);
            allPlayers ??= new AllPlayers();
            return true;
        }
        else
        {
            Debug.LogWarning("No File Found!");
            return false;
        }
    }

    public List<PlayerData> GetPlayerList()
    {
        // Sort the list in descending order according to best score - ref: StackOverflow
        allPlayers.PlayerList.Sort((a, b) => b.bestScore.CompareTo(a.bestScore));

        return allPlayers.PlayerList;
    }

    [System.Serializable]
    private class AllPlayers
    {
        public List<PlayerData> PlayerList = new List<PlayerData>();
    }
}

[System.Serializable]
public class PlayerData
{
    public string name = string.Empty;
    public int bestScore = 0;
}
