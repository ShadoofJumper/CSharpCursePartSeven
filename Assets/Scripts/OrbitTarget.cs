using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitTarget : MonoBehaviour
{
    [SerializeField] private Transform target;

    public float rotSpeed = 1.5f; // rotate speed

    private float _rotY; // variable for camera y position
    private Vector3 _offset; // offset between camera and player 


    // Start is called before the first frame update
    void Start()
    {
        _rotY = transform.eulerAngles.y; // get camera euler y position
        _offset = target.position - transform.position;
    }

    void LateUpdate()
    {
        float horInput = Input.GetAxis("Horizontal");
        if (horInput != 0) // slow move using arrows buttons
        {
            _rotY += horInput * rotSpeed;
        }
        else
        {
            _rotY += Input.GetAxis("Mouse X") * rotSpeed * 3;// or faste move using mouse
        }
        Quaternion rotation = Quaternion.Euler(0, _rotY, 0);
        // transform.position = target.position - (rotation * _offset); 
        transform.position = target.position - (rotation * _offset) ; // поворачиваем положение камеры вокруг персонажа. 

        transform.LookAt(target);
    }
}
