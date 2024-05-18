using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    //���� ����
    bool isJump = false;                //�÷��̾� jump ���� ����
    bool isTop = false;                 //�÷��̾ �ִ� ���̿� �ֳ� üũ ����
    public float jumpHeight = 0.0f;     //�÷��̾� jump ����
    public float jumpSpeed = 0.0f;      //�÷��̾� jump �ӵ�

    Vector2 startPosition;

    void Start()
    {
        startPosition = transform.position;     //jump���� : ���� ������ ���� 
    }

    void Update()
    {
        //���� ����
        if (Input.GetButtonDown("Jump"))
        {
            isJump = true;

        }
        else if (transform.position.y <= startPosition.y) {
            isJump = false;
            isTop = false;
            transform.position = startPosition;
        }

        if (isJump) //jump �� ��
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

        if (transform.position.y > startPosition.y && isTop) // jump �� ������ ��
        {
            transform.position = Vector2.MoveTowards(transform.position, startPosition, jumpSpeed * Time.deltaTime);
        }
    }
}
