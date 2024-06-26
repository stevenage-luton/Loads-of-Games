using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController characterController;

    public float moveSpeed = 8f;
    Vector3 velocity;
    public float gravity = -3f;


    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask ground;

    bool isGrounded;

    public bool remainStationary = false;

    bool invertControls = false;

    private void Start()
    {
        GameEventSystem.instance.onBeginScoliosisMode += ToggleInvertControls;
        GameEventSystem.instance.onEndScoliosisMode += ToggleInvertControls;
        GameEventSystem.instance.onDayBegin += SetInvertedControlsOff;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, ground);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (remainStationary == false)
        {
            
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            if (invertControls)
            {
                x = -x;
                z = -z;
            }

            Vector3 movement = transform.right * x + transform.forward * z;

            characterController.Move(movement * moveSpeed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }

    void ToggleInvertControls()
    {
        invertControls = !invertControls;
    }

    void SetInvertedControlsOff(int day)
    {
        invertControls = false;
    }
}
