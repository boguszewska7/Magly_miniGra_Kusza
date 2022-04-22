using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaking : MonoBehaviour
{
    public bool start = false;
    public float strenght = 0.5f;
    public Vector3 startPosition;
    private void Start()
    {
        startPosition = transform.position;
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartShake()
    {
            StartCoroutine(Shake());
    }
   public IEnumerator Shake()
    {
        Vector3 startPosition = transform.position;
        transform.position = startPosition + Random.insideUnitSphere;
        yield return null;
    }
    public void BackToNormalPosition()
    {
        transform.position = startPosition;
    }
   
}
