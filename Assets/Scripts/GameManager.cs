using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //싱글톤 적용
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

    //UI관련
    public Slider specialSlider;            //필살기 게이지
    public Text scoreText;
    public Text maxScoreText;
    public Text HeartText;

    public float gameSpeed = 4; //게임 스피드. 기본값 4
    public bool isPlay = false;
    public GameObject gameOverPanel;
    public GameObject subMenuPanel;

    public delegate void OnPlay(bool isplay);  //RespawnManager의 코루틴 작동
    public OnPlay onPlay;

    //플레이어
    public PlayerScript playerData;

    public void Start()
    {
        resetAll();
        playerData.animator.SetBool("isDead", true);
    }

    public void PlayStart()
    {
        gameOverPanel.SetActive(false);
        subMenuPanel.SetActive(false);
        isPlay = true;
        onPlay.Invoke(isPlay);
        resetAll();
    }

    public void resetAll()
    {
        gameSpeed = 4;
        Time.timeScale = 1;
        playerData.resetAllStats();
        ScoreManager.instance.nowScore = 0;
        UpdateHeartText(playerData.heart);
        UpdateScoreText(0);
        UpdateSpecialAttackBar();

        playerData.animator.SetBool("isDead", false);   //플레이어 죽는 연출 종료
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        isPlay = false;
        onPlay.Invoke(isPlay);

        ScoreManager.instance.UpdateMaxScore(); //최고기록 갱신
        gameOverPanel.GetComponent<GameOverPanelScript>().UpadeScore(ScoreManager.instance.nowScore);    //게임오버 패널 점수 텍스트 갱신
    }

    public void holdGame()  //게임 일시정지
    {
        subMenuPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ContinueGame()  //게임 계속 진행
    {
        subMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void GoTitle()
    {
        SceneManager.LoadScene("FirstScreen");
    }


    public void AddScore(int score) //점수 추가 함수
    {
        ScoreManager.instance.nowScore += score;
        UpdateScoreText(ScoreManager.instance.nowScore);
        if(playerData.specialAtkCount < playerData.maxSpecialAtkCount)  //플레이어 필살기 게이지 적립
        {
            playerData.specialAtkCount += 10;
            UpdateSpecialAttackBar();
        }

        if (gameSpeed < 8.0f)
        {
            gameSpeed += 0.1f;                 //적 처치시 스피드 증가
            playerData.nowJumpSpeed += 0.05f;   //플레이어의 점프스피드 증가
            playerData.nowAtkCoolTime -= 0.0025f;   //공격속도 증가
                                                    //gameSpeed가 8.0일때까지 증가함.
        }
    }

    public void AddScore_from_SpecialAttack(int score)  //필살기로 점수 추가했을 경우
    {
        ScoreManager.instance.nowScore += score;
        UpdateScoreText(ScoreManager.instance.nowScore);
    }

    public void UpdateSpecialAttackBar()    //필살기 게이지 업데이트 함수
    {
        specialSlider.GetComponent<Slider>().value = playerData.specialAtkCount;
        specialSlider.GetComponentInChildren<Text>().text = playerData.specialAtkCount + " / " + playerData.maxSpecialAtkCount;
    }

    public void UpdateHeartText(int heart)
    {
        HeartText.text = "Heart : " + (heart == 0 ? "Dead" : heart);
    }

    public void UpdateScoreText(int score) //점수 텍스트 갱신
    {
        scoreText.text = $"Score : {score}";
        maxScoreText.text = $"Max : {ScoreManager.instance.maxScore}";
    }
}
