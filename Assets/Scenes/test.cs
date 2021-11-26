using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Transform target;
    [Range(1,1000)]
    public float rotSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(target.position, Vector3.up, rotSpeed * Time.deltaTime);
        transform.RotateAround(transform.position, Vector3.up, rotSpeed * Time.deltaTime);
    }
}
