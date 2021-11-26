using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    public Transform head;
    public Transform groundCheck;
    private Vector3 FinalForce;
    private float ForceMag;
    private Vector3 ForceDir;
    public bool isGrounded;
    private Vector3 normal;
    private float distToGround=0.5f;
    private RaycastHit hit;
    public float MouseSensitivity;
    // the tilt angle of the camera according to the x axis
    private float tiltAngle;
    private float speedMax=10f;
    public bool DebugActivation;
    public float g;
    public float jumpHeight;
    private float x;
    private float z;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        MouseControl();
        findNOrmal();
        PLayerMovement();
        gravity();
        Jump();
        debug();
    }
    private void FixedUpdate()
    {
        rb.velocity = FinalForce;
        //rb.AddForce(FinalForce);
    }
    // add gravity
    void gravity()
    {
        FinalForce = FinalForce -  g* transform.up;
    }
    // find the normal of the surface 
    void findNOrmal()
    {
        if (Physics.Raycast(groundCheck.position, -transform.up, out hit, distToGround ))
        {
            normal = hit.normal;
            // check if the object is on the ground or not 
            isGrounded = true;
        }
        else { normal = transform.up;
            isGrounded = false;
        }
    }
    void MouseControl()
    {
        //control the looking direction 
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity*Time.deltaTime ;
        transform.Rotate(new Vector3(0, mouseX, 0));
        //control the camera
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;
        tiltAngle -= mouseY;
        tiltAngle = Mathf.Clamp(tiltAngle, -35f, 60f);
        head.localEulerAngles = new Vector3(tiltAngle, 0, 0);
    }
    void PLayerMovement()
    {
        // get the input for the player movement 
        x = Input.GetAxis("Horizontal") ;
        z = Input.GetAxis("Vertical") ;
        //calculate the force direction
        ForceDir = (Vector3.Cross(transform.right, normal) * z + transform.right * x).normalized;
        float speed = projectionMag(rb.velocity, ForceDir);
        // Limit the speed
        Debug.Log("speed"+speed);
        if ( speed >= speedMax)
        {
            ForceMag = 0f;
        }
        else if (!isGrounded)
        {
            ForceMag = 0f;
        }
        else if (x==0 & z==0)
        {
            ForceMag = 0f;
            rb.velocity = Vector3.zero;
        }
        else
        {
            ForceMag = 20f;
        }
        //add up the force according to the force direction
        FinalForce = ForceDir * ForceMag;
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump") & isGrounded )
        {
            Debug.Log("vlon");
            FinalForce = FinalForce + transform.up * 100f;
        }
    }
    void debug()
    {
        if (DebugActivation)
        {
            Debug.Log(normal);
            Debug.Log(ForceMag);
        }
    }
    // get the magnitude from the projection
    float projectionMag(Vector3 toBeProjected,Vector3 dir)
    {
        return toBeProjected.magnitude * Mathf.Abs(Mathf.Cos(Vector3.Angle(dir, toBeProjected) * Mathf.PI / 180)); 
    }
}
