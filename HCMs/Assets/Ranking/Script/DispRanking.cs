﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ランキング表示データ作成・表示
public class DispRanking : MonoBehaviour
{
    public Text rankingTimeText = null;         // ランキングのタイム表示テキスト
    public Text rankingOutText = null;          // タイムがランキング外だった際に使用

    public Canvas dispCanvas = null;            // 表示するキャンバス
    public Canvas hiddenCanvas = null;          // 非表示にするキャンバス

    public DataStorage rankingStorage = null;   // 表示用ランキングの取得用

    private float[] _dispRanking;   // ランキング保存用
    private float[] _dispLapTime;   // ラップタイム保存用

    private string _rankingKey;     // ゴールタイムのランキング呼び出しキー
    private string _lapRankKey;     // ラップタイムのランキング呼び出しキー
    private int _rankingMax;        // ランキングの表示数(=プレイヤー人数)
    private int _lapRankMax;        // ラップタイムランキングの表示数(=人数*周回数)
    private bool _rankOutActive;    // true:ランキング外のタイムを表示する false:表示しない
    private float _defaultTime;     // タイムの初期値もとい限界値

    void Start()
    {
        rankingTimeText = rankingTimeText.GetComponent<Text>();
        rankingOutText = rankingOutText.GetComponent<Text>();

        dispCanvas = dispCanvas.GetComponent<Canvas>();
        hiddenCanvas = hiddenCanvas.GetComponent<Canvas>();

        rankingStorage = rankingStorage.GetComponent<DataStorage>();
    }

    // @course string型：走行コース名 バトルモードはBattleでいい
    // @indicateRanks int型：表示するランキング数
    // @lapMax int型：最大周回数
    // @rankOutActive bool型：ランク外を表示するならtrue しないならfalse
    // @defaultTime float型：記録が無かった場合に設定するデフォルト値
    public void SetUpDispRanking(string gameMode, int indicateRanks, int lapMax, bool rankOutActive, float defaultTime)
    {
        Debug.Log("DispRankingセットアップ");

        _rankingKey = gameMode;
        _rankingMax = indicateRanks;
        _lapRankMax = indicateRanks * lapMax;
        _lapRankKey = (gameMode == "Battle" ? "BTLap" : "TALap");
        _rankOutActive = rankOutActive;
        _defaultTime = defaultTime;

        dispCanvas.gameObject.SetActive(true);
        hiddenCanvas.gameObject.SetActive(false);

        // ランキングデータを取得し、表示用データに反映する
        // バトルとタイムアタックで違うデ―タを取得する
        _dispRanking = new float[_rankingMax];
        _dispRanking = rankingStorage.GetData(_rankingKey, _rankingMax, _defaultTime);

        _dispLapTime = new float[_lapRankMax];
        _dispLapTime = rankingStorage.GetData(_lapRankKey, _lapRankMax, _defaultTime);
    }

    void Update()
    {
        string ranking_string = "";  // ランキング表示用
        string rankOut_string = "";  // ランキング外表示用

        // ランキング表示
        for (int idx = 0; idx < _rankingMax; idx++)
        {
            string time;
            if (_dispRanking[idx] < _defaultTime)
            {
                float second = _dispRanking[idx] % 60.0f;
                int minute = Mathf.FloorToInt(_dispRanking[idx] / 60.0f);
                time = string.Format("{0:00}.", minute) + string.Format("{0:00.000}", second) + "\n";
            }
            else
            {
                // デフォルト(1000.0f)ならタイム表記を表示しない
                time = "--.--.---\n";
            }

            if (idx == (_rankingMax - 1) && _rankOutActive)
            {
                // ランキング外のタイムを表示する場合、一番下の記録をランキング外として表示する
                rankOut_string += time;

                // ランキング内には表示しないのでbreakする
                break;
            }

            ranking_string += time;
        }

        // ラップタイム表示
        // 保存されているラップタイムの確認のために使用（ラップタイムデバッグ用）
        // １位：〇〇秒　〇〇秒　〇〇秒　　みたいに表示する
        // タイムが無い場合は
        // １位：－－秒　－－秒　－－秒　　といった感じで表示する
        //for (int idx = 0; idx < _lapRankMax; idx++)
        //{
        //    string lap;
        //    if (_dispLapTime[idx] < _defaultTime)
        //    {
        //        float second = _dispLapTime[idx] % 60.0f;
        //        int minute = Mathf.FloorToInt(_dispLapTime[idx] / 60.0f);
        //        lap = string.Format("{0:00}.", minute) + string.Format("{0:00.000}", second) + "\n";
        //    }
        //    else
        //    {
        //        // デフォルト(1000.0f)ならタイム表記を表示しない
        //        lap = "--.--.---\n";
        //    }

        //    if (idx == (_rankingMax - 1) && _rankOutActive)
        //    {
        //        // ランキング外のタイムを表示する場合、一番下の記録をランキング外として表示する
        //        rankOut_string += lap;

        //        // ランキング内には表示しないのでbreakする
        //        break;
        //    }

        //    ranking_string += lap;
        //}

        // テキストの表示を入れ替える
        rankingTimeText.text = ranking_string;
        rankingOutText.text = rankOut_string;
    }
}
