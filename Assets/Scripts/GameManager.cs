using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //싱글톤 적용
    #region instance
    public static GameManager instance;
    public int Score;
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

    public float gameSpeed = 1;
    public bool isPlay = false;
    public GameObject playbtn;

    public delegate void OnPlay(bool isplay);  //RespawnManager의 코루틴 작동
    public OnPlay onPlay;

    public void PlayButtonClick()
    {
        playbtn.SetActive(false);
        isPlay = true;
        onPlay.Invoke(isPlay);

        playerData.heart = 3;   //플레이어 하트 초기화
        playerData.UpdateHeartAndText();
    }


    public void GameOver()
    {
        //GameOver
        playbtn.SetActive(true);
        isPlay = false;
        onPlay.Invoke(isPlay);

        Debug.Log("GameOver");
    }


}
