using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeMovementScript : MonoBehaviour
{
    GameObject bike;
    [SerializeField]
    GameObject bikeTarget;

    [SerializeField]
    AudioSource m_source;

    public float speed = 60;

    bool bikeEnabled = false;

    private void Start()
    {
        bike = gameObject;
        ContractorEventSystem.instance.onPlayerTeleportToBike += moveBike;
        ContractorEventSystem.instance.onPlayerTeleportToToilet += moveBike;
    }

    // Update is called once per frame
    void Update()
    {
        if (bikeEnabled)
        {
            transform.position = transform.position + (Vector3.left * (speed * Time.deltaTime));

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ContractorEventSystem.instance.ElevatorTeleport();
    }

    private void moveBike()
    {
        bikeEnabled = !bikeEnabled;
        m_source.Play();
    }
}
