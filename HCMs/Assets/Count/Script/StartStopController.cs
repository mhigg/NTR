﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class RigidbodyVelocity
{
    public Vector3 velocity;//速度
    public Vector3 angularVelocity;//角速度
    public RigidbodyVelocity(Rigidbody rigidbody)
    {
        velocity = rigidbody.velocity;
        angularVelocity = rigidbody.angularVelocity;
    }
}

public class StartStopController : MonoBehaviour
{
    public bool startWait;
    bool prevWaiting;
    public GameObject[] ignoreGameObjects;
    RigidbodyVelocity[] rigidbodyVelocity;
    Rigidbody[] waitingRigidbody;
    MonoBehaviour[] waitingMonoBehaviour;

    public Text countText;
    public Image startImage;
    private float startCount = 0f;

    void Update()
    {
        startWait = (startCount < (400f/60f));
        if (prevWaiting != startWait)
        {
            if (startWait)
            {
                StartCounting();
            }
            if (!startWait)
            {
                EscapingCount();
            }
            prevWaiting = startWait;
        }
        startCount += Time.deltaTime;

        StartText();
    }

    ///特定のオブジェクトの座標情報を保存し、停止させる
    void StartCounting()
    {
        Predicate<Rigidbody> rigidbodyPredicate =
            obj => !obj.IsSleeping()
            && Array.FindIndex(ignoreGameObjects, gameObject => gameObject == obj.gameObject) < 0;
        waitingRigidbody = Array.FindAll(transform.GetComponentsInChildren<Rigidbody>(), rigidbodyPredicate);
        rigidbodyVelocity = new RigidbodyVelocity[waitingRigidbody.Length];
        for (int i = 0; i < waitingRigidbody.Length; i++)
        {
            //速度、角度を保存
            rigidbodyVelocity[i] = new RigidbodyVelocity(waitingRigidbody[i]);
            waitingRigidbody[i].Sleep();
        }

        Predicate<MonoBehaviour> monoBehaviourPredicate =
            obj => obj.enabled && obj != this
            && Array.FindIndex(ignoreGameObjects, gameObject => gameObject == obj.gameObject) < 0;
        waitingMonoBehaviour = Array.FindAll(
            transform.GetComponentsInChildren<MonoBehaviour>(),
            monoBehaviourPredicate);
        foreach (var monoBehaviour in waitingMonoBehaviour)
        {
            monoBehaviour.enabled = false;
        }
    }

    ///停止からの復帰
    void EscapingCount()
    {
        for(int i = 0; i < waitingRigidbody.Length; i++)
        {
            waitingRigidbody[i].WakeUp();
            waitingRigidbody[i].velocity = rigidbodyVelocity[i].velocity;
            waitingRigidbody[i].angularVelocity = rigidbodyVelocity[i].angularVelocity;
            
        }
        foreach (var monoBehaviour in waitingMonoBehaviour)
        {
            monoBehaviour.enabled = true;
            if (monoBehaviour.name == "RacingCarCPU")
            {
                for (int i = 0; i < 5; i++)
                {
                    var it = this.transform.GetChild(i);///.Find("AutoRun");
                    if(it.name == "RacingCarCPU")
                    {
                        for(int index = 0;index < it.childCount; index++)
                        {
                            var chi = it.GetChild(index);
                            if (chi.gameObject.activeSelf)
                            {
                                chi.GetComponent<CarAIPoint>()._startFlag = true;
                            }
                        }
                    }
                }
            }
        }
    }

    ///スタートカウント
    void StartText()
    {
        if(startCount <= 3f)
        {
            if (startCount >= 2.4f) countText.text = null;
        }
        else
        {
            if (startCount >= 3f) countText.text = "３";
            if (startCount >= 4f) countText.text = "２";
            if (startCount >= 5f) countText.text = "１";
            if (startCount >= 6f)
            {
                countText.gameObject.SetActive(false);
                startImage.gameObject.SetActive(true);
            }
            if (startCount >= 7f) startImage.gameObject.SetActive(false);
        }
    }
}
