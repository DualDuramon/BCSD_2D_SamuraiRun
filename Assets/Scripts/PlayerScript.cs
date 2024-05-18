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
    public float jumpSpeed = 0.0f;            //�÷��̾� jump �ӵ�

    //���� �ִϸ��̼�
    SpriteRenderer spriteRenderer;      //�÷��̾� ��������Ʈ
    public Animator animator;                  //�÷��̾� �ִϸ�����
    public Text HeartText;

    //���ݰ���
    [SerializeField] private float curTime;      //������ �� ���� ���ҳ�
    public Transform pos;
    public Vector2 boxSize;

    //�÷��̾� ����
    public int maxHeart = 3;
    public int heart;
    public bool isHit = false;
    public float atkCoolTime = 0.3f;    //���� ��Ÿ��

    void Start()
    {
        heart = maxHeart;
        startPosition = transform.position;     //jump���� : ���� ������ ���� 
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //���� ����
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
        if (isJump) //jump �� ��
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
        if (transform.position.y > startPosition.y && isTop) // jump �� ������ ��
        {
            transform.position = Vector2.MoveTowards(transform.position, startPosition, jumpSpeed * Time.deltaTime);
        }

        //���� ����
        if (Input.GetButtonDown("Attack") && GameManager.instance.isPlay)
        {
            if (curTime < 0)
            {
                Collider2D[] hitColliders = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

                foreach(Collider2D collider in hitColliders){   //���� ���� �ȿ� ���� ���� ��� �ǰ�ó��
                    if (collider.CompareTag("Enemy"))
                    {
                        collider.GetComponent<EnemyScript>().TakeDamage();
                    }
                }

                   
                //����
                animator.SetTrigger("attackTrigger");   //���ݸ�� ���
                curTime = atkCoolTime;
            }
        }
        curTime -= Time.deltaTime;

    }
    
    private void OnTriggerEnter2D(Collider2D collision) //�÷��̾� Ÿ��
    {
        if (!isHit && (collision.CompareTag("Mob") || collision.CompareTag("Enemy")))
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
                HeartText.text = "Heart : Dead";        //ü�°���
                animator.SetBool("isDead", true);       //�״� ��� ����
                GameManager.instance.GameOver();
            }

        }
    }

    public void UpdateHeartAndText()
    {
        HeartText.text = "Heart : " + heart;        //ü�°���
    }

    IEnumerator UnBeatTime()    //�����Լ� �ڷ�ƾ
    {
        int countTime = 0;

        while (countTime < 10)  //�����Ÿ���
        {
            if (countTime % 2 == 0)
            {
                spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.8f);
            }
            else
                spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            yield return new WaitForSeconds(0.2f);  //���� ������ ���

            countTime++;
        }

        isHit = false;

        yield return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);

    }
}
