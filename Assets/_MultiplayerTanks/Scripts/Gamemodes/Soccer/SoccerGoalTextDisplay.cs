using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoccerGoalTextDisplay : Photon.MonoBehaviour
{
    public float duration = 1.5f;

    public TextMeshProUGUI goalText;
    public TextMeshProUGUI team1Score;
    public TextMeshProUGUI team2Score;

    private CanvasGroup group;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
        group.alpha = 0;
    }

    public void UpdateText(int team)
    {
        team1Score.color = GetColor(1);
        team2Score.color = GetColor(2);

        team1Score.text = GetName(1) + " " + GetScore(1).ToString();
        team2Score.text = GetScore(2).ToString() + " " + GetName(2);

        if (team == 1)
        {
            goalText.color = GetColor(1);
            goalText.text = GetName(1) + " Scored";
        }
        else
        {
            goalText.color = GetColor(2);
            goalText.text = GetName(2) + " Scored";
        }

        Display();
    }


    public void Display()
    {
        StartCoroutine(DisplayText());
    }

    private IEnumerator DisplayText()
    {
        group.alpha = 1;

        yield return new WaitForSecondsRealtime(duration);

        while (group.alpha > 0){
            group.alpha -= Time.deltaTime / duration;
            yield return null;
        }
    }

    private string GetName(int team)
    {
        return Server.Current?.GetTeamName(team) ?? $"Team {team}";
    }

    private int GetScore(int team)
    {
        return Server.Current?.GetTeamScore(team) ?? 0;
    }

    private Color GetColor(int team)
    {
        return Server.Current?.GetTeamColor(team) ?? Tank.Default_Color;
    }
}
