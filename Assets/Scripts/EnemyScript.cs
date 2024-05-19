using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyScript : MonoBehaviour
{
    //�� ����
    public float EnemySpeed = 0.0f;  //�̵��ӵ�
    public float endPositionX = 0.0f;   //�� ���� ��ġ
    public Vector2 StartPosition;       //�� ���� ��ġ
    public int hp;
    public int maxHp = 1;
    public int myScore = 100;   //óġ �� ��� ����
    
    //Ÿ�ݰ���
    Vector2 knockBackPos;           //�ڷ� �з����� ��ġ
    public float knockBackDistance = 1.5f;  //�˹�Ÿ�
    bool isHit;                     //�˹������� ����
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
                //�˹鱸��
                transform.position = Vector2.Lerp(transform.position, knockBackPos, EnemySpeed * Time.deltaTime * GameManager.instance.gameSpeed);
                if (transform.position.x >= knockBackPos.x - 0.05f)  isHit = false;
            }
            else
            {
                transform.Translate(Vector2.left * Time.deltaTime * EnemySpeed * GameManager.instance.gameSpeed);

                if (transform.position.x <= endPositionX)   //�� ���� ������ ��Ȱ��ȭ
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
            knockBackPos = new Vector2(transform.position.x + knockBackDistance, transform.position.y);  //�˹� ������
            animator.SetTrigger("hitTrigger");  //�ǰݸ�� ���
        }
    }

}