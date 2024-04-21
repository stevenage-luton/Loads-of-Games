using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour
{
    bool liftMoving = false;
    [SerializeField]
    private float speed = 1.0f;

    Vector3 direction;

    [SerializeField] private GameObject destination;

    Vector3 oldPosition;

    Animator m_animator;

    bool hasRecievedExitSignal;

    private void Start()
    {
        direction = Vector3.up;
        ContractorEventSystem.instance.onPlayerTeleportToElevator += MoveLift;
        ContractorEventSystem.instance.onPlayerTeleportToElevator += ChangeDirection;
        ContractorEventSystem.instance.onPlayerTeleportToBike += StopMovingLift;


        ContractorEventSystem.instance.onPlayerTeleportToToilet += StopMovingLift;
        ContractorEventSystem.instance.onPlayerTeleportToToilet += SetPositionToTop;

        ContractorEventSystem.instance.onEnableLeaveTrigger += RecievedExitSignal;

        oldPosition = transform.position;
        hasRecievedExitSignal = false;
        m_animator = transform.Find("LiftDoors").GetComponent<Animator>();
    }

    void Update()
    {
        if (liftMoving)
        {
            transform.position = transform.position + (direction * (speed * Time.deltaTime));

        }
    }
    private void MoveLift()
    {
        liftMoving = true;
    }

    private void StopMovingLift()
    {
        liftMoving = false;
    }

    void ChangeDirection()
    {
        if (direction == Vector3.up)
        {
            direction = Vector3.down;
        }
        else
        {
            direction = Vector3.up;
        }

    }

    void SetPositionToTop()
    {
        transform.position = destination.transform.position;
    }

    void RecievedExitSignal()
    {
        if (!hasRecievedExitSignal)
        {
            ToggleDoors();
            hasRecievedExitSignal = true;
        }
    }

    void ToggleDoors()
    {
        m_animator.SetTrigger("ToggleDoors");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasRecievedExitSignal)
        {
            ToggleDoors();
            ContractorEventSystem.instance.ExitLevel();
        }
    }

}
