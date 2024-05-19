using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public float obstacleSpeed = 0.0f;  //�̵��ӵ�
    public float endPositionX = 0.0f;   //�� ���� ��ġ
    public Vector2 StartPosition;       //�� ���� ��ġ

    private void OnEnable()
    {
        transform.position = StartPosition;

    }

    void Update()
    {
        if (GameManager.instance.isPlay)
        {
            transform.Translate(Vector2.left * Time.deltaTime * obstacleSpeed * GameManager.instance.gameSpeed);

            if (transform.position.x <= endPositionX)   //�� ���� ������ ��Ȱ��ȭ
            {
                gameObject.SetActive(false);
            }

        }
    }
}
