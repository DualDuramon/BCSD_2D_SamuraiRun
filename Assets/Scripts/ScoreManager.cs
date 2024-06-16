using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int nowScore;
    public int maxScore;

    //�̱��� ����
    #region instance
    public static ScoreManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public void UpdateMaxScore()
    {
        if (maxScore < nowScore) maxScore = nowScore;
    }
}
