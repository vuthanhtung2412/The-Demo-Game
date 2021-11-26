using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Properties : MonoBehaviour
{
    public float temperature;
    public string element;
    public Vector3 rot;
    public bool visibility;
    private Quaternion Rotation;
    // Start is called before the first frame update
    public void Start()
    {
        Rotation.eulerAngles = rot;
    }
    public Quaternion getRot()
    {
        return Rotation;
    }
}
