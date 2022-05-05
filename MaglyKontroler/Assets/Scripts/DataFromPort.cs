using System.Collections;
using WebSocketSharp;
//using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO.Ports;
using System.Globalization;
using System.Text;
public class DataFromPort : MonoBehaviour
{
    WebSocket ws;
    public GameObject Shake;
    public GameObject sphere;
    public GameObject sphereOrgin;
    Vector3 controllerRotation;
    Vector3 firstRotation;
    float firstAccx;
    public const float angleMultiplier = 1.0f;
    private bool first_data = true;
    float prev = 0;
    float max = -4;
    double dt;
    bool recalc = false;

    public Vector3 oneAxis;

    [SerializeField] public double RotationControllerX;
    [SerializeField] public bool CreateArow = false;
    [SerializeField] public bool PushArow = false;
    [SerializeField] public float powerToAcc = 3f;

    // Start is called before the first frame update
    void Start()
    {

        ws = new WebSocket("ws://localhost:3000");
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {

            string data = e.Data.ToString();
            Debug.Log(data);
            if (data.Length > 8)
            {
                string[] splitData = data.Split(',');
                float Accx = float.Parse(splitData[0], CultureInfo.InvariantCulture.NumberFormat);
                float Accy = float.Parse(splitData[1], CultureInfo.InvariantCulture.NumberFormat);
                float Accz = float.Parse(splitData[2], CultureInfo.InvariantCulture.NumberFormat);
                float Gyrx = float.Parse(splitData[3], CultureInfo.InvariantCulture.NumberFormat);
                float Gyry = float.Parse(splitData[4], CultureInfo.InvariantCulture.NumberFormat);
                float Gyrz = float.Parse(splitData[5], CultureInfo.InvariantCulture.NumberFormat);
                float Magx = float.Parse(splitData[6], CultureInfo.InvariantCulture.NumberFormat);
                float Magy = float.Parse(splitData[7], CultureInfo.InvariantCulture.NumberFormat);
                float Magz = float.Parse(splitData[8], CultureInfo.InvariantCulture.NumberFormat);
                float Qx = float.Parse(splitData[9], CultureInfo.InvariantCulture.NumberFormat);
                float Qy = float.Parse(splitData[10], CultureInfo.InvariantCulture.NumberFormat);
                float Qz = float.Parse(splitData[11], CultureInfo.InvariantCulture.NumberFormat);
                float Qw = float.Parse(splitData[12], CultureInfo.InvariantCulture.NumberFormat);
                

                float[] quats = { Qx, Qy, Qz, Qw };
                float[] acc = { Accz, Accy, Accz };
                float[] mag = { Magz, Magy, Magz };

                //Debug.Log(Accx +";"+ Accy + ";"+ Accz);
                if (first_data)
                {
                    firstRotation = calculateRotation(quats);
                    firstAccx = Accx;
                   // firstRotation.y += 90;
                    if (firstRotation.x != 0 && firstRotation.y != 0 && firstRotation.z != 0) first_data = false;
                }
                else if(recalc)
                {
                    float n = calculateRotation(quats).y;
                    firstRotation.y = n - prev;
                    recalc = false;
                }
                else
                {

                    controllerRotation = calculateRotation(quats);
                    controllerRotation.x -= firstRotation.x;
                    controllerRotation.y -= firstRotation.y;
                    controllerRotation.z -= firstRotation.z;


                    if (Math.Abs(prev - controllerRotation.y) < 1.5f)
                    {
                        controllerRotation.y = prev;
                    }


                    prev = controllerRotation.y;
                    //Ograniczenie po osi y
                    controllerRotation.y = Mathf.Clamp(controllerRotation.y, -45, 45);

                    

                    

                    //Ograniczenie po osi x
                    controllerRotation.x = Mathf.Clamp(controllerRotation.x, -40, 1);
                    //Debug.Log(controllerRotation.x);
                    RotationControllerX = -controllerRotation.x;



                    //Ładowanie strzał
                    ////if (CreateArow == false && PushArow == false && controllerRotation.z<= -90)
                    if(firstAccx- powerToAcc > Accx && CreateArow == false && PushArow == false)
                     {
                        Debug.Log("lol");
                         CreateArow = true;
                         PushArow = true;
                     }
                       
                    controllerRotation.z = Mathf.Clamp(controllerRotation.z, -75, 75);
                    oneAxis = new Vector3(controllerRotation.x, controllerRotation.y, 0);

                }
            }
            else
            {
                //kiedy kontroler jest wy��czony
                if (data == "cwo") recalc = true;
            }
           
        };

    }

    // Update is called once per frame
    void Update()
    {


        sphere.transform.eulerAngles = oneAxis;
        
        ws.Send("r");
    }

    Vector3 calculateRotation(float[] quat)
    {
        Vector3 output;
        double first = (2 * (quat[0] * quat[1] + quat[2] * quat[3]));
        double second = (1 - 2 * (Math.Pow(quat[1], 2) + Math.Pow(quat[2], 2)));
        output.z = -(float)(Math.Atan2(first, second) * 180.0f / Math.PI);
        first = (2 * (quat[0] * quat[2] - quat[3] * quat[1]));
        output.x = (float)(Math.Asin(first) * 180.0f / Math.PI);
        first = (2 * (quat[0] * quat[3] + quat[1] * quat[2]));
        second = (1 - 2 * (Math.Pow(quat[2], 2) + Math.Pow(quat[3], 2)));
        output.y = -(float)(Math.Atan2(first, second) * 180.0f / Math.PI);
        return output;
    }

}



