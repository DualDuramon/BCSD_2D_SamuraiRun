using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    //���� ����
    private bool isJump = false;                //�÷��̾� jump ���� ����
    private bool isTop = false;                 //�÷��̾ �ִ� ���̿� �ֳ� üũ ����
    private Vector2 startPosition;              //�÷��̾� ó�� ��ġ
    public float jumpHeight = 0.0f;            //�÷��̾� jump ����
    public float nowJumpSpeed = 0.0f;            //�÷��̾� jump �ӵ�
    public float initJumpSpeed = 7.0f;

    //���� �ִϸ��̼�
    SpriteRenderer spriteRenderer;      //�÷��̾� ��������Ʈ
    public Animator animator;                  //�÷��̾� �ִϸ�����

    //���ݰ���
    [SerializeField] private float curTime;      //������ �� ���� ���ҳ�
    public Transform pos;
    public Vector2 boxSize;

    //�ʻ�� ����
    public GameObject SpecialAttack_pref;

    //���� ����
    SoundSystem mySound;

    //�÷��̾� ����
    public int maxHeart = 3;
    public int heart;
    public bool isHit = false;
    public float nowAtkCoolTime;
    public float atkCoolTime = 0.3f;    //���� ��Ÿ��
    public int specialAtkCount = 0;        //���� ���� �ʻ�� ������
    public int maxSpecialAtkCount = 100;    //�ʻ�� ������ �ѵ�

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mySound = GetComponent<SoundSystem>();
    }

    void Start()
    {
        resetAllStats();
        startPosition = transform.position;     //jump���� : ���� ������ ���� 
    }

    public void resetAllStats()
    {
        heart = maxHeart;
        nowJumpSpeed = initJumpSpeed;
        nowAtkCoolTime = atkCoolTime;
        specialAtkCount = 0;
    }

    void Update()
    {
        //���� ����
        if (Input.GetButtonDown("Jump") && GameManager.instance.isPlay)
        {
            if(!isJump) mySound.SoundPlay(2);   //�������� ���
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
                transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, jumpHeight), nowJumpSpeed * Time.deltaTime);
            }
            else
            {
                isTop = true;
                animator.SetBool("isTop", true);
            }
        }
        if (transform.position.y > startPosition.y && isTop) // jump �� ������ ��
        {
            transform.position = Vector2.MoveTowards(transform.position, startPosition, nowJumpSpeed * Time.deltaTime);
        }

        //���� ����
        if (Input.GetButtonDown("Attack") && GameManager.instance.isPlay)
        {
            if (curTime < 0)
            {
                //����
                animator.SetTrigger("attackTrigger");   //���ݸ�� ��� �� �������� �̺�Ʈ(�ִϸ��̼ǿ� ����) ����
                mySound.SoundPlay(0);   //���� ���� ���
            }
        }

        //�ʻ��
        if(Input.GetButtonDown("SpecialAttack") && GameManager.instance.isPlay)
        {
            if (curTime < 0 && specialAtkCount == maxSpecialAtkCount)
            {
                SpecialAttack();
                animator.SetTrigger("attackTrigger");   //���ݸ�� ���
                mySound.SoundPlay(1);   //�ʻ�� ���� ���
                curTime = nowAtkCoolTime;
            }
        }


        curTime -= Time.deltaTime;
    }

    public void Attack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

        foreach (Collider2D collider in hitColliders)
        {   //���� ���� �ȿ� ���� ���� ��� �ǰ�ó��
            if (collider.CompareTag("Enemy"))
            {
                collider.GetComponent<EnemyScript>().TakeDamage(1, false);
            }
        }

        curTime = nowAtkCoolTime;
    }

    public void SpecialAttack()
    {
        Instantiate(SpecialAttack_pref,
                   new Vector3(transform.position.x + 1.0f, transform.position.y, transform.position.z),
                   Quaternion.identity);

        specialAtkCount = 0;                    //�ʻ������� �ʱ�ȭ
        GameManager.instance.UpdateSpecialAttackBar();
    }

    public void Hit()   //�ǰ����� �Լ�
    {
        if (isHit) return;

        heart--;

        if (heart > 0)
        {
            isHit = true;
            StartCoroutine(UnBeatTime());
            mySound.SoundPlay(3);   //�ǰݻ��� ���;
        }
        else
        {
            animator.SetBool("isDead", true);       //�״� ��� ����
            GameManager.instance.GameOver();
            mySound.SoundPlay(4);   //���ӿ��� ���� ���
        }

        GameManager.instance.UpdateHeartText(heart);
    }

    IEnumerator UnBeatTime()    //�����Լ� �ڷ�ƾ
    {
        int countTime = 0;

        while (countTime < 10)  //�����Ÿ���
        {
            if (countTime % 2 == 0)
            {
                spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            }
            else
                spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            yield return new WaitForSeconds(0.2f);  //���� ������ ���

            countTime++;
        }

        isHit = false;

        yield return null;
    }

    private void OnDrawGizmos() //���� ��Ʈ�ڽ� �׸���
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);

    }
}
