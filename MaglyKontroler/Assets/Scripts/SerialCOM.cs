using System.Collections;
//using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO.Ports;
using System.Text;

public class SerialCOM : MonoBehaviour
{


    public SerialPort sp = new SerialPort("/dev/tty.usbmodem11101",
            9600);
    public string recievingdata;
    public GameObject sphere;



    
    public Quaternion Qnew;

    // Start is called before the first frame update
    void Start()
    {

        try
        {
                sp.ReadTimeout = 21;
                sp.DtrEnable = true;
                sp.StopBits = StopBits.One;
                sp.DataBits = 8;


            sp.Open();
                Debug.Log("port was opened seccesfully");
        }
        catch{
            Debug.Log("port was NOOOT opened seccesfully");
        }
        
    }

    

     // Update is called once per frame
     void Update()
     {
        if (sp.IsOpen == false)
        {
            sp.Open();
        }

        try
        {
            if (sp.BytesToRead > 0)
            {
                
                recievingdata = sp.ReadTo("\n");
                Debug.Log(recievingdata);
                string[] SplitData = recievingdata.Split(',');

                try
                {
                float AccX = float.Parse(SplitData[0]) / 100 ;
                                float AccY = float.Parse(SplitData[1]) / 100;
                                float AccZ = float.Parse(SplitData[2]) / 100;
                                float GyroX = float.Parse(SplitData[3]) / 100;
                                float GyroY = float.Parse(SplitData[4]) / 100;
                                float GyroZ = float.Parse(SplitData[5]) / 100;
                                float MagX = float.Parse(SplitData[6]);
                                float MagY = float.Parse(SplitData[7]);
                                float MagZ = float.Parse(SplitData[8]);
                                float Qw = float.Parse(SplitData[9]) / 100;
                                float Qx = float.Parse(SplitData[10]) / 100;
                                float Qy = float.Parse(SplitData[11]) / 100;
                                float Qz = float.Parse(SplitData[12]) / 100;

                                Qnew = new Quaternion(Qx, Qy, Qz, Qy);


                                sphere.transform.rotation = Quaternion.Lerp(sphere.transform.rotation, Qnew, 0.1f);
                }
                catch
                {

                }
}
            else
            {
               
            }
        }
        catch(TimeoutException)
        {
            Debug.Log("Timeout");
        }
     }

   
}
