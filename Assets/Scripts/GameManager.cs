using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

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
    public int nowScore = 0; //현재 점수
    public int maxScore = 0; //최고기록점수
    public Text scoreText;
    public Text HeartText;

    public float gameSpeed = 4; //게임 스피드. 기본값 4
    public bool isPlay = false;
    public GameObject playbtn;

    public delegate void OnPlay(bool isplay);  //RespawnManager의 코루틴 작동
    public OnPlay onPlay;

    //플레이어
    public PlayerScript playerData;

    public void PlayButtonClick()
    {
        playbtn.SetActive(false);
        isPlay = true;
        onPlay.Invoke(isPlay);
        gameSpeed = 4;
        Time.timeScale = 1.0f;

        playerData.resetAllStats();
        UpdateHeartText(playerData.heart);
        UpdateNowScoreText(0);  //점수 초기화

        playerData.animator.SetBool("isDead",false);   //플레이어 죽는 연출 종료
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


    public void AddScore(int score) //점수 추가 함수
    {
        nowScore += score;
        UpdateNowScoreText(nowScore);


        if (gameSpeed < 8.0f)
        {
            gameSpeed += 0.1f;                 //적 처치시 스피드 증가
            playerData.nowJumpSpeed += 0.05f;   //플레이어의 점프스피드 증가
            playerData.nowAtkCoolTime -= 0.0025f;   //공격속도 증가
                                                    //gameSpeed가 8.0일때까지 증가함.
        }

    }

    public void UpdateHeartText(int heart)
    {
        HeartText.text = "Heart : " + (heart == 0 ? "Dead" : heart);
    }

    public void UpdateNowScoreText(int score) //현재점수갱신
    {
        scoreText.text = "Score : " + score;
    }
}
