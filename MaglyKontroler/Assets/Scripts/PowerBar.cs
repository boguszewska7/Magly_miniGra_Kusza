using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    private Image Power;
    public double currentZrotation;
    public double MaxPower = 90;
    DataFromPort Data;

    private void Start()
    {
        Power = GetComponent<Image>();
        Data = FindObjectOfType<DataFromPort>();

    }

    private void Update()
    {
        currentZrotation = Data.RotationControllerX;
        Power.fillAmount = (float)(currentZrotation / MaxPower);
    }



}

