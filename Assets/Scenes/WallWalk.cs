using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWalk : MonoBehaviour
{
    private float t;
    private GameObject g;
    public float rotSpeed;
    public bool rot;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "RotOrb")
        {
            GetComponent<TPSMovement>().enabled = false;
            g=collision.gameObject;
           
            rot = true;
        }
    }
    public void Update()
    {
        if (rot)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, g.GetComponent<Properties>().getRot(), t);
            t = t + Time.deltaTime;
            //g.GetComponent<TPSMovement>()._rb.velocity = Vector3.zero;
        }
        if (t > 1f)
        {
            rot = false;
            GetComponent<TPSMovement>().enabled = true;
            t = 0f;
        }
    }
}
