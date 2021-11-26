using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSMovement : MonoBehaviour
{
    private CapsuleCollider _cl;
    public Rigidbody _rb;
    public Transform head;
    private Vector3 _groundCheck ;
    private Vector3 _Finalv;
    private float _vMag=10f;
    private Vector3 _vDir;
    public bool isGrounded;
    private Vector3 normal;
    public float angleLim;
    private RaycastHit hit;
    public float MouseSensitivity;
    // the tilt angle of the camera according to the x axis
    private float _tiltAngle;
    // the accelerator of the gravity 
    public float g;
    public float jumpHeight;
    //velocity the affect the x axis
    private float _x;
    //velocity the affect the z axis 
    private float _z;
    //the additional velocity that cause by gravity
    private float _vByG;
    private bool isJumping;
    private float timer;
    void Start()
    {
        _cl = GetComponent<CapsuleCollider>();
        _rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        _groundCheck = transform.position - transform.up*(_cl.height/2 - 0.1f);
        MouseControl();
        PLayerMovement();
        gravity();
        jump();
        //Debug.Log(Vector3.Cross(transform.right, normal));
        Debug.Log(_rb.velocity);
    }
    private void FixedUpdate()
    {
        _rb.velocity = _Finalv;
    }


    void MouseControl()
    {
        //control the looking direction 
        float m_mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        transform.Rotate(new Vector3(0, m_mouseX, 0));
        //control the camera
        float m_mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;
        _tiltAngle -= m_mouseY;
        _tiltAngle = Mathf.Clamp(_tiltAngle, -35f, 60f);
        head.localEulerAngles = new Vector3(_tiltAngle, 0, 0);
    }


    void PLayerMovement()
    {
        findNOrmal();   
        // get the input for the player movement
        _x = Input.GetAxis("Horizontal");
        _z = Input.GetAxis("Vertical");
        if(Vector3.Angle(normal,transform.up)>= angleLim)
        {
            _z = 0f;
            _x = 0f;
        }
        //calculate the force direction
        _vDir = (Vector3.Cross(transform.right, normal) * _z + Vector3.Cross(normal,transform.forward) * _x).normalized;
        //_vDir= (transform.forward * _z + transform.right * _x).normalized;
        _Finalv = _vDir * _vMag;
    }


    void findNOrmal()
    {
        if (Physics.Raycast(_groundCheck, -transform.up, out hit, _cl.radius/Mathf.Cos(angleLim*Mathf.PI/180)+0.1f-_cl.radius))
        {
            normal = hit.normal;
            // check if the object is on the ground or not 
            isGrounded = true;
        }
        else
        {
            //normal = Vector3.zero;
            isGrounded = false;
        }
    }


    void gravity()
    {
        if (!isGrounded)
        {
            _vByG += g * Time.deltaTime;
        }
        else if (normal==transform.up)
        {
            _vByG = 0f;
        }
        else if (Vector3.Angle(normal, transform.up) >= angleLim)
        {
            _vByG = 20f;
        }
        _Finalv =_Finalv-_vByG * transform.up;
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") & isGrounded)
        {
            isJumping = true;
        }
        if (isJumping)
        {
            _Finalv = _Finalv + Mathf.Sqrt(2 * jumpHeight * g) * transform.up;
            timer += Time.deltaTime;
        }
        if (timer > 0.5f & isGrounded)
        {
            isJumping = false;
            timer = 0f;
        }
    }
}
