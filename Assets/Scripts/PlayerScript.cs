using System.Collections;
using System.Collections.Generic;
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
    Animator animator;      //플레이어 애니메이터

    void Start()
    {
        startPosition = transform.position;     //jump관련 : 현재 포지션 저장 
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
        //점프 관련
        if (Input.GetButtonDown("Jump"))
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
                transform.position = Vector2.Lerp(transform.position, 
                    new Vector2(transform.position.x, jumpHeight), jumpSpeed * Time.deltaTime);
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
}
