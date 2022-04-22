using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KuszaController : MonoBehaviour
{

   
    public double Power;
    [SerializeField] private float MaxPower = 20;
    [SerializeField] public float MaxRoatation = 30;

    public GameObject Arrow;
    public Transform Point;

    public double currentZrotation;
    
    DataFromPort Data;

    // Start is called before the first frame update
    void Start()
    {
        Data = FindObjectOfType<DataFromPort>();
    }

    // Update is called once per frame
    void Update()
    { 
        Power = MaxPower * (Data.RotationControllerX/ MaxRoatation);

        if (Data.CreateArow)
        {
            Debug.Log("Stworz strzale");
        }

        if (Data.PushArow && Input.GetKeyDown("space"))
        {    
            GameObject CreatedArrow = Instantiate(Arrow, Point.position, Point.rotation);

            CreatedArrow.GetComponent<Rigidbody>().velocity = Point.transform.up * (float)Power;

            Data.PushArow = false;
        }

    }
}
