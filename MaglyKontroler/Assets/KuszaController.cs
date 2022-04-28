using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KuszaController : MonoBehaviour
{

   
    public double Power;
    [SerializeField] private float MaxPower = 20;
    [SerializeField] public float MaxRoatation = 30;
    [SerializeField] public GameObject kusza;

    public GameObject Arrow;
    public GameObject FakeArrow;
    public GameObject FakeArrowPosition;
    public Transform Point;
    private Animator kuszaAnimator;
      private GameObject Fake;
    public double currentZrotation;
    
    DataFromPort Data;
    PointsController Points;

    public int CountArrow = 5;
    private int CurrentArrow = 0;
    private int AmountOfArrow;

    public Text EndGame;
    public Text ArrowLeft;

    // Start is called before the first frame update
    void Start()
    {
        
        kuszaAnimator = kusza.GetComponent<Animator>();
        Data = FindObjectOfType<DataFromPort>();
        Points = FindObjectOfType<PointsController>();
        AmountOfArrow = CountArrow;
    }

    // Update is called once per frame
    void Update()
    { 
        Power = MaxPower * (Data.RotationControllerX/ MaxRoatation);
        if (CurrentArrow == CountArrow)
        {
            EndGame.text = "Udało Ci się!";
        }
        else
        {
               if (Data.CreateArow)
                {
                    AmountOfArrow--;
                    Debug.Log("Stworz strzale");
                    kuszaAnimator.Play("Pull");
                    Fake = Instantiate(FakeArrow, FakeArrowPosition.transform.position, FakeArrowPosition.transform.rotation);
                    Fake.transform.SetParent(transform.GetChild(0));
                    ArrowLeft.text = AmountOfArrow.ToString();
                    Data.CreateArow = false;
            }

                if (Data.PushArow && Input.GetKeyDown("space"))
                {
                CurrentArrow++;
                Destroy(Fake);
                    kuszaAnimator.Play("Push");
                    GameObject CreatedArrow = Instantiate(Arrow, Point.position, Point.rotation);

                    CreatedArrow.GetComponent<Rigidbody>().velocity = Point.transform.up * (float)Power;

                    Data.PushArow = false;
                }
        }
       

    }
}
