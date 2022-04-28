using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPoints : MonoBehaviour
{
    public GameObject kusza;
    //public PointsController PointsController;
    
    public int PointsToAdd;


    public void OnCollisionEnter(Collision collision)
    {
        kusza.GetComponent<PointsController>().Points += PointsToAdd;
        Debug.Log("tarcza");
    }
}
