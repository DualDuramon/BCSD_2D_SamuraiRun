using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayStartBtn : MonoBehaviour
{
    public void StartFirst()
    {
        gameObject.SetActive(false);
        GameManager.instance.PlayStart();
    }
}
