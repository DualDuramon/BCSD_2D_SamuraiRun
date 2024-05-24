using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //�̱��� ����
    #region instance
    public static GameManager instance;

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
    public Slider specialSlider;    //�ʻ�� ������
    public Text scoreText;
    public Text maxScoreText;
    public Text HeartText;

    public float gameSpeed = 4; //���� ���ǵ�. �⺻�� 4
    public bool isPlay = false;
    public GameObject gameOverPanel;

    public delegate void OnPlay(bool isplay);  //RespawnManager�� �ڷ�ƾ �۵�
    public OnPlay onPlay;

    //�÷��̾�
    public PlayerScript playerData;


    public void PlayStart()
    {
        gameOverPanel.SetActive(false);
        isPlay = true;
        onPlay.Invoke(isPlay);
        gameSpeed = 4;
        Time.timeScale = 1.0f;

        playerData.resetAllStats();
        nowScore = 0;
        UpdateHeartText(playerData.heart);
        UpdateScoreText(0);  //���� �ʱ�ȭ
        UpdateSpecialAttackBar();

        playerData.animator.SetBool("isDead",false);   //�÷��̾� �״� ���� ����
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        isPlay = false;
        onPlay.Invoke(isPlay);

        if (nowScore > maxScore) maxScore = nowScore;
        gameOverPanel.GetComponent<GameOverPanelScript>().UpadeScore(nowScore);    //���ӿ��� �г� ���� �ؽ�Ʈ ����
    }

    public void GoTitle()
    {
        SceneManager.LoadScene("FirstScreen");
    }


    public void AddScore(int score) //���� �߰� �Լ�
    {
        nowScore += score;
        UpdateScoreText(nowScore);
        if(playerData.specialAtkCount < playerData.maxSpecialAtkCount)  //�÷��̾� �ʻ�� ������ ����
        {
            playerData.specialAtkCount += 10;
            UpdateSpecialAttackBar();
        }

        if (gameSpeed < 8.0f)
        {
            gameSpeed += 0.1f;                 //�� óġ�� ���ǵ� ����
            playerData.nowJumpSpeed += 0.05f;   //�÷��̾��� �������ǵ� ����
            playerData.nowAtkCoolTime -= 0.0025f;   //���ݼӵ� ����
                                                    //gameSpeed�� 8.0�϶����� ������.
        }
    }

    public void AddScore_from_SpecialAttack(int score)  //�ʻ��� ���� �߰����� ���
    {
        nowScore += score;
        UpdateScoreText(nowScore);
    }

    public void UpdateSpecialAttackBar()    //�ʻ�� ������ ������Ʈ �Լ�
    {
        specialSlider.GetComponent<Slider>().value = playerData.specialAtkCount;
        specialSlider.GetComponentInChildren<Text>().text = playerData.specialAtkCount + " / " + playerData.maxSpecialAtkCount;
    }

    public void UpdateHeartText(int heart)
    {
        HeartText.text = "Heart : " + (heart == 0 ? "Dead" : heart);
    }

    public void UpdateScoreText(int score) //���� �ؽ�Ʈ ����
    {
        scoreText.text = $"Score : {score}";
        maxScoreText.text = $"Max : {maxScore}";
    }
}
