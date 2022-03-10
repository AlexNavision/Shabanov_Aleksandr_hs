using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIT : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D Rigidbody;
    void Start()
    {
        Invoke("Destroy",3f);
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.AddForce(transform.right);
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
