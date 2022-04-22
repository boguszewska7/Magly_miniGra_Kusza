using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    DataFromPort Data;
    // Start is called before the first frame update
    void Start()
    {
        Data = FindObjectOfType<DataFromPort>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Data.CreateArow)
        {  
            Destroy(transform.GetChild(0).gameObject);
            Data.CreateArow = false;
        }
            
    }
}
