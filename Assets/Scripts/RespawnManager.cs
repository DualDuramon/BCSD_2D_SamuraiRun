using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public List<GameObject> mobPool = new List<GameObject>();   //������ƮǮ��
    public GameObject[] mobs;
    public int objCount = 1;        //�ִ� ���� �� ������Ʈ ��


    private void Awake()
    {
        for (int idx = 0; idx < mobs.Length; idx++)   //������Ʈ Ǯ�� �����յ� ����.
        {
            for(int nowCount = 0; nowCount < objCount; nowCount++)
            {
                mobPool.Add(CreateObj(mobs[idx], transform));
            }
        }
    }

    private void Start()
    {
        GameManager.instance.onPlay += StartGame;   //delegate ���
    }

    void StartGame(bool isplay)
    {
        if (isplay) //����Ǹ� �ڷ�ƾ�� ����
        {
           DeactivateAllMobs();            //������ ���� �ʱ�ȭ
           StartCoroutine(CreateMob());    //������ �ڷ�ƾ
        }
        else //�ƴϸ� �� ����
        {
            StopAllCoroutines();
        }
    }

    void DeactivateAllMobs()
    {
        for (int i = 0; i < mobPool.Count; i++)
        {
            mobPool[i].gameObject.SetActive(false);
        }
    }

    IEnumerator CreateMob()
    {
        yield return new WaitForSeconds(0.5f);  //�ణ�� ������ �ֱ�

        while (GameManager.instance.isPlay)
        {
            mobPool[DetectMobs()].SetActive(true);    //�������� �� ����(Ȱ��ȭ��Ű��)
            yield return new WaitForSeconds(Random.Range(1f, 3f));  //�����ֱ�� �� ���� �ڷ�ƾ ����
        }
    }

    int DetectMobs()   //��Ȱ��ȭ �� ���� idx�� �˻��� �������� �ϳ� ��ȯ�ϴ� �Լ�.
    {
        List<int> num = new List<int>();
        for(int i = 0; i< mobPool.Count; i++)
        {
            if (!mobPool[i].activeSelf)
            {
                num.Add(i);
            }
        }

        int randomNum = 0;
        if (num.Count > 0)
        {
            randomNum = num[Random.Range(0, num.Count)];
        }

        return randomNum;
    }

    GameObject CreateObj(GameObject obj, Transform parent)  //������Ʈ Ǯ���� �� obj ����� �Լ�, �ڽſ��� ������.
    {
        GameObject copy = Instantiate(obj);
        copy.transform.SetParent(parent);
        copy.SetActive(false);

        return copy;

    }
}
