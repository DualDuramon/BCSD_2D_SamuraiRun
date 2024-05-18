using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    //점프 관련
    bool isJump = false;                //플레이어 jump 상태 변수
    bool isTop = false;                 //플레이어가 최대 높이에 있나 체크 변수
    public float jumpHeight = 0.0f;     //플레이어 jump 높이
    public float jumpSpeed = 0.0f;      //플레이어 jump 속도

    Vector2 startPosition;

    void Start()
    {
        startPosition = transform.position;     //jump관련 : 현재 포지션 저장 
    }

    void Update()
    {
        //점프 관련
        if (Input.GetButtonDown("Jump"))
        {
            isJump = true;

        }
        else if (transform.position.y <= startPosition.y) {
            isJump = false;
            isTop = false;
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
            }
        }

        if (transform.position.y > startPosition.y && isTop) // jump 후 내려올 때
        {
            transform.position = Vector2.MoveTowards(transform.position, startPosition, jumpSpeed * Time.deltaTime);
        }
    }
}
