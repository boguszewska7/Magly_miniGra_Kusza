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
    [SerializeField] public GameObject ArrowController;

    // Start is called before the first frame update
    void Start()
    {

        ws = new WebSocket("ws://localhost:3000");
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {

            string data = e.Data.ToString();
            if (data.Length > 8)
            {
                string[] splitData = data.Split(',');
                float Qx = float.Parse(splitData[0], CultureInfo.InvariantCulture.NumberFormat);
                float Qy = float.Parse(splitData[1], CultureInfo.InvariantCulture.NumberFormat);
                float Qz = float.Parse(splitData[2], CultureInfo.InvariantCulture.NumberFormat);
                float Qw = float.Parse(splitData[3], CultureInfo.InvariantCulture.NumberFormat);
                float[] quats = { Qx, Qy, Qz, Qw };
              
                if (first_data)
                {
                    firstRotation = calculateRotation(quats);
                    firstRotation.y += 90;
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

                    //Ograniczenie po osi y
                    controllerRotation.y = Mathf.Clamp(controllerRotation.y, -180, 0);

                    

                    oneAxis = new Vector3(0, controllerRotation.y, 0);

                    //Ograniczenie po osi x
                    controllerRotation.x = Mathf.Clamp(controllerRotation.x, -40, 0);
                    //Debug.Log(controllerRotation.x);
                    RotationControllerX = -controllerRotation.x;
                    prev = controllerRotation.y;
                    //Debug.Log(dt);

                    Debug.Log(controllerRotation.z);
                    //controllerRotation.x = Mathf.Clamp(controllerRotation.x, -92, 0);
                    if (CreateArow == false && PushArow == false && controllerRotation.z<= -90)
                    {
                        CreateArow = true;
                        PushArow = true;
                    }
                        

                }
            }
            else
            {
                //kiedy kontroler jest wy³¹czony
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



