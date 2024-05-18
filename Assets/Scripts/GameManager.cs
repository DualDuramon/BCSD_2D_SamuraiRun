using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //�̱��� ����
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

    public delegate void OnPlay(bool isplay);  //RespawnManager�� �ڷ�ƾ �۵�
    public OnPlay onPlay;

    public void PlayButtonClick()
    {
        playbtn.SetActive(false);
        isPlay = true;
        onPlay.Invoke(isPlay);

        playerData.heart = playerData.maxHeart;   //�÷��̾� ��Ʈ �ʱ�ȭ
        playerData.UpdateHeartAndText();

        playerData.animator.SetBool("isDead",false);   //�÷��̾� �״� ���� ����
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
