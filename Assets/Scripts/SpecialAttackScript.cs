using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpecialAttackScript : MonoBehaviour
{
    public float speed = 0.0f;
    public float maxXDistance = 15.0f;


    void Update()
    {
        if (GameManager.instance.isPlay)
        {
            transform.Translate(Vector2.right * Time.deltaTime * speed * GameManager.instance.gameSpeed);
        }

        if(transform.position.x >= maxXDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //�ʻ�� �浹
    {
        if (collision.CompareTag("Enemy"))  //�� Ÿ�ݽ� 1000������
        {
            collision.GetComponent<EnemyScript>().TakeDamage(1000, true);
        }
        else if (collision.CompareTag("Mob"))   //��ֹ� �ı�
        {
            GameManager.instance.AddScore(200);
            collision.gameObject.SetActive(false);
        }
    }

}
