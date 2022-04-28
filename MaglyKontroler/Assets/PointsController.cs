using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsController : MonoBehaviour
{
    public int Points = 0;
    public Text PointText;

    // Update is called once per frame
    void Update()
    {
        PointText.text = "PUNKTY " + Points;
    }
}
