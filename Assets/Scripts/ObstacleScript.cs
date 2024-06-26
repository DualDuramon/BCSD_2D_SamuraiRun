using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public float obstacleSpeed = 0.0f;  //이동속도
    public float endPositionX = 0.0f;   //몹 삭제 위치
    public Vector2 StartPosition;       //몹 생성 위치

    private void OnEnable()
    {
        transform.position = StartPosition;
    }

    void Update()
    {
        if (GameManager.instance.isPlay)
        {
            transform.Translate(Vector2.left * Time.deltaTime * obstacleSpeed * GameManager.instance.gameSpeed);

            if (transform.position.x <= endPositionX)   //벽 끝에 닿으면 비활성화
            {
                gameObject.SetActive(false);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //플레이어와 충돌했을 경우
    {
        if (!collision.CompareTag("Player")) return;

        collision.gameObject.GetComponent<PlayerScript>().Hit();    //플레이어 피격 재생
    }
}
