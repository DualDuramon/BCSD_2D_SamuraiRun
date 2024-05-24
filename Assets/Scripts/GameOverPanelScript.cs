using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanelScript : MonoBehaviour
{
    public Text nowScore;
    public Text maxScore;

    public void UpadeScore(int score)
    {
        maxScore.text = $"MaxScore : {GameManager.instance.maxScore}";
        nowScore.text = $"       Score : {score}";

    }
}
