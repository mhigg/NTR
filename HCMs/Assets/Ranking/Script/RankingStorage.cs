﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ランキングデータの取得
public class RankingStorage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // PlayerPrefsに保存したランキングをranking配列に格納
    // float型配列
    // @KEY string型：データ読み出しのキー
    // @DATA_MAX int型：ランキングデータを保存する配列の最大サイズ
    public float[] GetRankingData(string KEY, int DATA_MAX)
    {
        string _ranking = PlayerPrefs.GetString(KEY);
        float[] retRanking = new float[DATA_MAX];
        if (_ranking.Length > 0)
        {
            string[] data = _ranking.Split(","[0]);
            
            for (int idx = 0; idx < data.Length && idx < DATA_MAX; idx++)
            {
                retRanking[idx] = float.Parse(data[idx]);
            }
            Debug.Log("ランキングデータ取得");
        }
        else
        {
            Debug.Log("ランキングデータなし");

            // DeleteRanking呼び出し後など、
            // 何も値を入れないと返り値として渡す配列の中がすべて0になってしまうので、
            // 一番大きい数値を入れて初期化しておく
            for (int idx = 0; idx < DATA_MAX; idx++)
            {
                retRanking[idx] = 1000.0f;
            }
        }

        return retRanking;
    }

    // ランキングの削除
    // @KEY string型：削除するランキングのキー
    public void DeleteRanking(string KEY)
    {
        string _ranking = PlayerPrefs.GetString(KEY);
        if (_ranking.Length > 0)
        {
            Debug.Log("ランキング削除");
            PlayerPrefs.DeleteKey(KEY);
        }
    }

}
