﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    public DataStorage rankingStorage = null;

    void Start()
    {
        rankingStorage = rankingStorage.GetComponent<DataStorage>();
    }

    bool isCalledOnce = false;
    bool isDeleteOnce = false;
    void Update()
    {
        if (!isCalledOnce)
        {
            ///ここを任意のボタンにしましょう。
            if (Input.GetKeyDown("space"))
            {
                isCalledOnce = true;
                FadeManager.Instance.LoadScene("MenuScene", 2.0f);
                Debug.Log("Menuへ");
            }
        }

        if (!isDeleteOnce)
        {
            ///ここを任意のボタンにしましょう。
            if (Input.GetKeyDown(KeyCode.F1))
            {
                isDeleteOnce = true;
                rankingStorage.DeleteData("ranking");
                Debug.Log("デリート");
            }
        }

    }
}
