using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text bestScoreText;

    public event Action<int> OnClicked;
    private int id = -1;

    public void StartGame()
    {
        OnClicked?.Invoke(id);
    }

    public void Setup(PlayerData playerData, int id)
    {
        this.id = id;
        nameText.SetText($"Play as - <color=#4D77FF>{playerData.name}</color>");
        bestScoreText.SetText(playerData.bestScore.ToString());
    }
}
