using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //�̱��� ����
    #region instance
    public static GameManager instance;
    public PlayerScript playerData;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    #endregion

    //UI����
    public int nowScore = 0; //���� ����
    public int maxScore = 0; //�ְ�������
    public Text scoreText;
    public Text HeartText;

    public float gameSpeed = 1;
    public bool isPlay = false;
    public GameObject playbtn;

    public delegate void OnPlay(bool isplay);  //RespawnManager�� �ڷ�ƾ �۵�
    public OnPlay onPlay;

    public void PlayButtonClick()
    {
        playbtn.SetActive(false);
        isPlay = true;
        onPlay.Invoke(isPlay);

        playerData.heart = playerData.maxHeart;   //�÷��̾� ��Ʈ �ʱ�ȭ
        UpdateHeartText(playerData.maxHeart);
        UpdateNowScoreText(0);  //���� �ʱ�ȭ

        playerData.animator.SetBool("isDead",false);   //�÷��̾� �״� ���� ����
    }


    public void GameOver()
    {
        //GameOver
        playbtn.SetActive(true);
        isPlay = false;
        onPlay.Invoke(isPlay);

        Debug.Log("GameOver");
        if (nowScore > maxScore) maxScore = nowScore;
    }

    public void UpdateHeartText(int heart)
    {
        HeartText.text = "Heart : " + (heart == 0 ? "Dead" : heart);
    }

    public void AddScore(int score) //���� �߰� �Լ�
    {
        nowScore += score;
        UpdateNowScoreText(nowScore);
    }

    public void UpdateNowScoreText(int score) //������������
    {
        scoreText.text = "Score : " + score;
    }
}
