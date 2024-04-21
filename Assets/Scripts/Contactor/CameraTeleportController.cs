using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTeleportController : MonoBehaviour
{
    [SerializeField]
    CameraTeleport BedTeleport;

    [SerializeField]
    CameraTeleport BikeTeleport;

    [SerializeField]
    CameraTeleport ElevatorTeleport;

    [SerializeField]
    GameObject ToiletTeleportPlayer;

    [SerializeField]
    GameObject head;

    [SerializeField]
    Camera PlayerCamera;

    [SerializeField]
    GameObject Player;

    [SerializeField]
    GameObject Toilet;

    bool gameStart = true;

    bool BikeTarget = true;

    void Start()
    {
        ContractorEventSystem.instance.onPlayerTeleportToToilet += TeleportToToilet;
        ContractorEventSystem.instance.onPlayerTeleportToElevator += TeleportToElevator;
        ContractorEventSystem.instance.onPlayerTeleportToBike += TeleportToBike;
    }

    private void Update()
    {
        //if (gameStart)
        //{
        //    ContractorEventSystem.instance.RemainStationary();
        //    BikeTeleport.TeleportCameraToObject(PlayerCamera, Player, BikeTeleport.gameObject);
        //    gameStart = false;
        //}
    }

    void TeleportToToilet()
    {

        Player.transform.position = ToiletTeleportPlayer.transform.position;
        Player.transform.rotation = ToiletTeleportPlayer.transform.rotation;
        PlayerCamera.transform.parent = head.transform;
        PlayerCamera.transform.localRotation = head.transform.localRotation;
        PlayerCamera.transform.localPosition = Vector3.zero;
        PlayerCamera.transform.localEulerAngles = Vector3.zero;
        PlayerCamera.transform.LookAt(Toilet.transform);
    }

    void TeleportToBike()
    {
        BikeTeleport.TeleportCameraToObject(PlayerCamera, Player, BikeTeleport.gameObject);
    }

    void TeleportToElevator()
    {
        ContractorEventSystem.instance.RemainStationary();
        ElevatorTeleport.TeleportCameraToObject(PlayerCamera, Player, ElevatorTeleport.gameObject);
        StartCoroutine(ElevatorDelay());
    }

    IEnumerator ElevatorDelay()
    {
        yield return new WaitForSeconds(4.0f);
        if (BikeTarget)
        {
            ContractorEventSystem.instance.BikeTeleport();
            BikeTarget = !BikeTarget;
        }
        else
        {
            ContractorEventSystem.instance.ToiletTeleport();
        }

    }
}
