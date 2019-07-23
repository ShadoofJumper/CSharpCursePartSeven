using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class RelativeMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    public float rotSpeed = 15.0f;
    public float moveSpeed = 6.0f;

    private CharacterController _charController;

    void Start()
    {
        _charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.zero; // start in zero and then step by step add movement components
        float horInput = Input.GetAxis("Horizontal");
        float verInput = Input.GetAxis("Vertical");
        if (horInput != 0 || verInput != 0) // if push buttons
        {
            movement.x = horInput * moveSpeed;
            movement.z = verInput * moveSpeed;
            movement = Vector3.ClampMagnitude(movement, moveSpeed); // ограничиваем движение по диагонали

            Quaternion tmp = target.rotation; // save start orientation

            movement = target.TransformDirection(movement);
            target.rotation = tmp;

            Quaternion diraction = Quaternion.LookRotation(movement); // savee direction
            transform.rotation = Quaternion.Lerp(transform.rotation, diraction, rotSpeed * Time.deltaTime);// move from start to direction in speed rotSpeed 

            movement *= Time.deltaTime;
            _charController.Move(movement);
        }
    }
}
