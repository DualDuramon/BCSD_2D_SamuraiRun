using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    //���� ����
    private bool isJump = false;                //�÷��̾� jump ���� ����
    private bool isTop = false;                 //�÷��̾ �ִ� ���̿� �ֳ� üũ ����
    private Vector2 startPosition;              //�÷��̾� ó�� ��ġ
    public float jumpHeight = 0.0f;     //�÷��̾� jump ����
    public float jumpSpeed = 0.0f;      //�÷��̾� jump �ӵ�

    //���� �ִϸ��̼�
    Animator animator;      //�÷��̾� �ִϸ�����

    void Start()
    {
        startPosition = transform.position;     //jump���� : ���� ������ ���� 
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
        //���� ����
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
                animator.SetBool("isTop", true);
            }
        }

        if (transform.position.y > startPosition.y && isTop) // jump �� ������ ��
        {
            transform.position = Vector2.MoveTowards(transform.position, startPosition, jumpSpeed * Time.deltaTime);
        }
    }
}
