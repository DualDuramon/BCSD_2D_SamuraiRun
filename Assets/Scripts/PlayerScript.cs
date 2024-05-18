using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    //점프 관련
    private bool isJump = false;                //플레이어 jump 상태 변수
    private bool isTop = false;                 //플레이어가 최대 높이에 있나 체크 변수
    private Vector2 startPosition;              //플레이어 처음 위치
    public float jumpHeight = 0.0f;     //플레이어 jump 높이
    public float jumpSpeed = 0.0f;      //플레이어 jump 속도

    //점프 애니메이션
    Animator animator;                  //플레이어 애니메이터
    SpriteRenderer spriteRenderer;      //플레이어 스프라이트
    public Text HeartText;

    //플레이어 스탯
    public int heart = 3;
    public bool isHit = false;

    void Start()
    {
        startPosition = transform.position;     //jump관련 : 현재 포지션 저장 
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //점프 관련
        if (Input.GetButtonDown("Jump") && GameManager.instance.isPlay)
        {
            isJump = true;
            animator.SetBool("isJump", true);

        }
        else if (transform.position.y <= startPosition.y) {
            isJump = false;
            animator.SetBool("isJump", false);

            isTop = false;
            animator.SetBool("isTop", false);

            transform.position = startPosition;
        }

        if (isJump) //jump 할 때
        {
            if (transform.position.y <= jumpHeight - 0.1f && !isTop)
            {
                transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, jumpHeight), jumpSpeed * Time.deltaTime);
                //transform.position = Vector2.LerpUnclamped(transform.position, new Vector2(transform.position.x, jumpHeight), jumpSpeed * Time.deltaTime);

            }
            else
            {
                isTop = true;
                animator.SetBool("isTop", true);
            }
        }

        if (transform.position.y > startPosition.y && isTop) // jump 후 내려올 때
        {
            transform.position = Vector2.MoveTowards(transform.position, startPosition, jumpSpeed * Time.deltaTime);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision) //플레이어 타격
    {
        if (!isHit && collision.CompareTag("Mob"))
        {
            heart--;
            
            if (heart > 0)
            {
                isHit = true;
                StartCoroutine(UnBeatTime());
                UpdateHeartAndText();
                return;
            }
            else
            {
                HeartText.text = "Heart : Dead";        //체력갱신
                GameManager.instance.GameOver();
            }

        }
    }

    public void UpdateHeartAndText()
    {
        HeartText.text = "Heart : " + heart;        //체력갱신
    }

    IEnumerator UnBeatTime()    //무적함수 코루틴
    {
        int countTime = 0;

        while (countTime < 10)  //깜빡거리기
        {
            if (countTime % 2 == 0)
            {
                spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.8f);
            }
            else
                spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            yield return new WaitForSeconds(0.2f);  //다음 프레임 대기

            countTime++;
        }

        isHit = false;

        yield return null;
    }


}
