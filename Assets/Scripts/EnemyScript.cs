using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyScript : MonoBehaviour
{
    //적 스탯
    public float EnemySpeed = 0.0f;  //이동속도
    public float endPositionX = 0.0f;   //몹 삭제 위치
    public Vector2 StartPosition;       //몹 생성 위치
    public int hp;
    public int maxHp = 1;
    public int myScore = 100;   //처치 시 얻는 점수
    
    //타격관련
    Vector2 knockBackPos;           //뒤로 밀려나갈 위치
    public float knockBackDistance = 1.5f;  //넉백거리
    bool isHit;                     //넉백중인지 여부
    Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        transform.position = StartPosition;
        hp = maxHp;
    }

    void Update()
    {
        if (GameManager.instance.isPlay)
        {
            if (isHit)
            {
                //넉백구현
                transform.position = Vector2.Lerp(transform.position, knockBackPos, EnemySpeed * Time.deltaTime * GameManager.instance.gameSpeed);
                if (transform.position.x >= knockBackPos.x - 0.05f)  isHit = false;
            }
            else
            {
                transform.Translate(Vector2.left * Time.deltaTime * EnemySpeed * GameManager.instance.gameSpeed);

                if (transform.position.x <= endPositionX)   //벽 끝에 닿으면 비활성화
                {
                    gameObject.SetActive(false);
                }

            }
        }
    }

    public void TakeDamage()
    {
        hp--;
        
        if (hp <= 0)  
        {
            GameManager.instance.AddScore(myScore);
            gameObject.SetActive(false);
        }
        else
        {
            isHit = true;
            knockBackPos = new Vector2(transform.position.x + knockBackDistance, transform.position.y);  //넉백 포지션
            animator.SetTrigger("hitTrigger");  //피격모션 재생
        }
    }

}