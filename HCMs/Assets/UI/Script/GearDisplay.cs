﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class GearDisplay : MonoBehaviour
{
    public ImageNo imageNo = null;
    public int playerNo;

    [SerializeField] private Text m_GearTex = null;
    [SerializeField] private CarController m_Car = null;
//    private int m_GearNum;
    private GameObject carObj;

    // Start is called before the first frame update
    void Start()
    {
        carObj = GameObject.FindGameObjectsWithTag("RacingCar")[playerNo].transform.parent.gameObject;

        m_Car = carObj.GetComponent<CarController>();
        m_GearTex = m_GearTex.GetComponent<Text>();

        imageNo = imageNo.GetComponent<ImageNo>();
    }

    // Update is called once per frame
    void Update()
    {
//        m_GearNum = m_Car.CurrentGearNum;
//        m_GearTex.text = $"{m_Car.CurrentGearNum + 1}";
        imageNo.SetNo(m_Car.CurrentGearNum + 1);
    }
}
