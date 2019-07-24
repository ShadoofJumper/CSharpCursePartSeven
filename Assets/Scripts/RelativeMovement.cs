using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class RelativeMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    public float rotSpeed = 15.0f;
    public float moveSpeed = 6.0f;
    public float jumpSpeed = 7.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;

    private float _varSpeed;
    private ControllerColliderHit _contact; // save data about function condlict

    private CharacterController _charController;

    void Start()
    {
        _varSpeed = minFall;
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
            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            movement = target.TransformDirection(movement);
            target.rotation = tmp;

            Quaternion diraction = Quaternion.LookRotation(movement); // savee direction
            transform.rotation = Quaternion.Lerp(transform.rotation, diraction, rotSpeed * Time.deltaTime);// move from start to direction in speed rotSpeed 
                

            bool hitGround = false;
            RaycastHit hit;

            if (_varSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit)) //бросем луч из центра игрока вниз и передаем результат в хит
            {
                float check = (_charController.height + _charController.radius) / 1.9f; // растояние от центра к нижниму краю капсулі, + немного еще, потому что делим попалам "почти"
                hitGround = hit.distance <= check; //
            }

            // for jump logic
            if (hitGround) // if on ground
            {
                if (Input.GetButtonDown("Jump"))
                {
                    _varSpeed = jumpSpeed;
                }
                else
                {
                    _varSpeed = minFall;
                }
            }
            else // if in air
            {
                // use gravity until have max terminal speed
                _varSpeed += gravity * 5 * Time.deltaTime;
                if (_varSpeed < terminalVelocity)
                {
                    _varSpeed = terminalVelocity;
                }

                if (_charController.isGrounded)
                {
                    Debug.Log("AAAAAAAA");
                    if (Vector3.Dot(movement, _contact.normal) < 0) //берем скалярное умножение вектора движения и номрали плоскости, если оно отрицательное то мы толкаем в нужную сторону.
                    {
                        movement = _contact.normal * moveSpeed;
                    }
                    else
                    {
                        movement += _contact.normal * moveSpeed;
                    }
                }
            }



            movement.y = _varSpeed;
            movement *= Time.deltaTime;
            _charController.Move(movement);
        }

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            _contact = hit;
        }
    }
}
