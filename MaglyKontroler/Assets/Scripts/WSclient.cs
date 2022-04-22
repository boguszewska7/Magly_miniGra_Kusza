using WebSocketSharp;
using UnityEngine;
using System.Collections;

using System.Runtime.InteropServices;
using System;
using UnityEngine.SceneManagement;


public class WSclient : MonoBehaviour
{
    [SerializeField] public Animator anim;


    WebSocket ws;
    
    long stop, start;
    long dt;
    bool msensorState;
    bool lastState= false, realState=false;




    //GameObject sphere;
    void Start()
    {

       // Application.targetFrameRate = 60;
        //sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //sphere.transform.position = new Vector3(0,0,0);

        ws = new WebSocket("ws://localhost:3000");
        ws.Connect();
        ws.Send("msensor");
        ws.OnMessage += (sender, e) =>
        {
            string data = e.Data.ToString();
            msensorState = (data == "true");


         
                if (!realState && msensorState && !lastState)
                {
                    realState = true;
                    lastState = true;
                }
                else if (realState && msensorState && lastState)
                {
                    realState = false;
                    lastState = true;
                }
                else if (lastState && !msensorState)
                {
                    realState = false;
                }
            
                lastState = msensorState;

            if (realState)
            {
                CallScene();
            }

        };
        
    }


    void Update()
    {
        anim.Play("ruch1",0);
    }

    void CallScene()
    {
        Debug.Log("LOL");
       
    }
    
   
}