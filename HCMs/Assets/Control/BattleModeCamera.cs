﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BattleModeCamera : MonoBehaviour
{
    void Start()
    {
        if(GameObject.FindGameObjectsWithTag("CPU").Length > 0)
        {
            // CPUがいるなら画面分割なしのカメラに切り替える
            this.transform.Find("PerfectCamera01").gameObject.SetActive(false);
            this.transform.Find("PerfectCamera11").gameObject.SetActive(true);
        }
        else
        {
            // CPUがいない(＝2Pがいる)なら画面分割ありのカメラに切り替える
            this.transform.Find("PerfectCamera01").gameObject.SetActive(true);
            this.transform.Find("PerfectCamera11").gameObject.SetActive(false);
        }
    }
}
