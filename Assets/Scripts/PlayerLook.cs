using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float sensitivity = 200f;
    public Transform playerBody;
    public float playerRotation = 0f;

    public PlayerMovement playerMovement;

    public Canvas canvas;

    public Transform destination;
    public Transform originalHead;

    public float movementTime = 1;
    public float rotationSpeed = 5f;
    public float movementSpeed = 5f;

    float interactionRange = 2f;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private string interactTag = "Interact";

    [SerializeField] private Transform gameScreen;

    private GameObject selectedObject;
    private Transform selectedTransform;

    public enum State
    {
        Movement,
        Computer,
        DayInterlude,
    }

    public State state;

    void Start()
    {
        state = State.Movement;
        GameEventSystem.instance.onComputerInteract += ComputerMode;
        GameEventSystem.instance.onEndInteract += EndComputerMode;
        GameEventSystem.instance.onDayEnd += BeginInterlude;
        GameEventSystem.instance.onDayBegin += EndInterlude;

    }
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        switch (state)
        {
            case State.Movement:
                Cursor.lockState = CursorLockMode.Locked;

                SlerpCameraToRotation(transform, originalHead);
                //transform.position = Vector3.Lerp(transform.position, originalHead.position, movementSpeed * Time.deltaTime);
                LerpCameraToPosition(transform, transform.position, originalHead.position);


                playerRotation -= mouseY;
                playerRotation = Mathf.Clamp(playerRotation, -90f, 90f);

                transform.localRotation = Quaternion.Euler(playerRotation, 0f, 0f);
                playerBody.Rotate(Vector3.up * mouseX);

                RaycastHit hit;

                if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, layerMask))
                {
                    selectedTransform = hit.transform;
                    selectedObject = selectedTransform.gameObject;
                    if (selectedObject.GetComponent<Interactable>() != null)
                    {

                    }
                    //selectedObject.GetComponent<ItemDataSO>().itemName = "test";
                    if (Input.GetMouseButtonDown(0))
                    {
                        //RaycastHit hit;
                        if (selectedObject.CompareTag(interactTag))
                        {
                            selectedObject.GetComponent<Interactable>().OnInteract();
                        }
                    }
                }


                break;

            case State.Computer:
                Cursor.lockState = CursorLockMode.Confined;
                
                if (!gameScreen)
                {
                    return;
                }


                //Interpolate Rotation;
                //transform.rotation = Quaternion.Slerp(transform.rotation, gameScreen.rotation, rotationSpeed * Time.deltaTime);
                SlerpCameraToRotation(transform, gameScreen);
                //transform.position = Vector3.Lerp(transform.position, destination.position, movementSpeed * Time.deltaTime);
                LerpCameraToPosition(transform, transform.position, destination.position);

                if (Input.GetButtonDown("Cancel"))
                {
                    GameEventSystem.instance.EndInteractTrigger();
                }

                break;

            case State.DayInterlude:
                Cursor.lockState = CursorLockMode.Confined;

                break;

                //case State.Dialogue:
                //    Cursor.lockState = CursorLockMode.Confined;


                //    if (!dialogueTarget)
                //        return;
                //    //Interpolate Rotation
                //    transform.localRotation = Quaternion.Slerp(transform.localRotation, dialogueTarget.rotation, rotationSpeed * Time.deltaTime);


                //    break;

                //case State.Menu:
                //    Cursor.lockState = CursorLockMode.Locked;
                //    break;
        }
    }

    void ComputerMode()
    {
        state = State.Computer;
        playerMovement.remainStationary = true;
        
    }
    void EndComputerMode()
    {
        state = State.Movement;
        playerMovement.remainStationary = false;
        
    }
    void BeginInterlude()
    {
        state = State.DayInterlude;
        playerMovement.remainStationary = true;

    }

    void EndInterlude(int day)
    {
        state = State.Movement;
        playerMovement.remainStationary = false;

    }

    void LerpCameraToPosition(Transform transformToMove, Vector3 startPosition, Vector3 endPosition)
    {
        transformToMove.position = Vector3.Lerp(startPosition, endPosition, movementSpeed * Time.deltaTime);
    }
    void SlerpCameraToRotation(Transform transformToMove, Transform targetTransform)
    {
        transformToMove.rotation = Quaternion.Slerp(transformToMove.rotation, targetTransform.rotation, rotationSpeed * Time.deltaTime);
    }
}
