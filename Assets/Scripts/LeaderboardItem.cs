using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeaderboardItem : MonoBehaviour
{
    [SerializeField] private TMP_Text numberText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text bestScoreText;
    [SerializeField] private Image background;

    public void Setup(PlayerData playerData, int number, Color backgroundColor)
    {
        nameText.SetText(playerData.name);
        numberText.SetText(number + ".");
        bestScoreText.SetText(playerData.bestScore.ToString());
        background.color = backgroundColor;
    }
}
