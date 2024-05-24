using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public List<GameObject> mobPool = new List<GameObject>();   //오브젝트풀링
    public GameObject[] mobs;
    public int objCount = 1;        //종류별 최대 수용 몹 오브젝트 수


    private void Awake()
    {
        for (int idx = 0; idx < mobs.Length; idx++)   //오브젝트 풀에 프리팹들 저장.
        {
            for(int nowCount = 0; nowCount < objCount; nowCount++)
            {
                mobPool.Add(CreateObj(mobs[idx], transform));
            }
        }
    }

    private void Start()
    {
        GameManager.instance.onPlay += StartGame;   //delegate 등록
    }

    void StartGame(bool isplay)
    {
        if (isplay) //실행되면 코루틴들 실행
        {
           DeactivateAllMobs();            //생성된 몹들 초기화
           StartCoroutine(CreateMob());    //몹생성 코루틴
        }
        else //아니면 다 종료
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
        yield return new WaitForSeconds(0.5f);  //초반 약간의 딜레이 넣기

        while (GameManager.instance.isPlay)
        {
            mobPool[DetectMobs()].SetActive(true);    //랜덤으로 몹 생성(활성화시키기)
            if (GameManager.instance.gameSpeed < 7.0f)
                yield return new WaitForSeconds(Random.Range(1f, 1.5f));  //랜덤주기로 몹 생성 코루틴 실행
            else
                yield return new WaitForSeconds(1.0f);
        }
    }

    int DetectMobs()   //비활성화 된 몹의 idx를 검색후 랜덤으로 하나 반환하는 함수.
    {
        int maxMobIdx = (int)GameManager.instance.gameSpeed < mobs.Length ? (int)GameManager.instance.gameSpeed : mobs.Length;

        List<int> num = new List<int>();
        for (int i = 0; i < maxMobIdx * objCount; i++) 
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

    GameObject CreateObj(GameObject obj, Transform parent)  //오브젝트 풀링할 때 obj 만드는 함수, 자신에게 연결함.
    {
        GameObject copy = Instantiate(obj);
        copy.transform.SetParent(parent);
        copy.SetActive(false);

        return copy;

    }
}
