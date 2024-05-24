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
    public bool isDead = false;
    
    //Ÿ�ݰ���
    Vector2 knockBackPos;           //�ڷ� �з����� ��ġ
    public float knockBackDistance = 1.5f;  //�˹�Ÿ�
    bool isHit;                     //�˹������� ����
    Animator animator;

    //���� ����
    SoundSystem mySound;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        mySound = GetComponent<SoundSystem>();
    }
    private void OnEnable()
    {
        transform.position = StartPosition;
        hp = maxHp;
        isDead = false;
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

    private void OnTriggerEnter2D(Collider2D collision) //�÷��̾�� �浹���� ���
    {
        if (isDead || !collision.CompareTag("Player")) return;
        
        collision.gameObject.GetComponent<PlayerScript>().Hit();    //�÷��̾� �ǰ� ���
    }


    public void TakeDamage(int dmg, bool isSpecialAttack)
    {
        hp -= dmg;
        
        if (hp <= 0)  
        {
            if (isSpecialAttack) GameManager.instance.AddScore_from_SpecialAttack(myScore);
            else GameManager.instance.AddScore(myScore);

            isDead = true;
            animator.SetTrigger("deadTrigger");
        }
        else
        {
            isHit = true;
            knockBackPos = new Vector2(transform.position.x + knockBackDistance, transform.position.y);  //�˹� ������
            animator.SetTrigger("hitTrigger");  //�ǰݸ�� ���
        }

        mySound.SoundPlay(0);   //�ǰ� ���� ���
    }

    public void OnDead()
    {
        gameObject.SetActive(false);
    }
}