using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrzalaCollider : MonoBehaviour
{

    private Rigidbody Rb; 

    private void Start()
    {
        Rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.other.CompareTag("Tarcza"))
        {
            Rb.isKinematic = true;
        }
    }
}
