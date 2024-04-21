using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    PlayerScript movementController;
    PickupScript pickupScript;

    public AnimationCurve collectLerpCurve;

    private Vector3 _oldPosition;
    private Quaternion _oldRotation;

    private GameObject selectedObject;

    private GameObject heldObject;
    private ItemData heldItemData;

    [SerializeField]
    GameObject pickUpObject;
    [SerializeField]
    GameObject gunPosition;

    private Camera playerCamera;

    [SerializeField]
    AudioSource eatSource;
    [SerializeField]
    AudioSource crumpleSource;
    [SerializeField]
    AudioSource itemSource;

    GunScript m_gun;

    bool firstNote;

    public enum PlayerState
    {
        Movement,
        Examining,
        Stationary,
        Menu,
    }

    public PlayerState m_playerState;

    private void Awake()
    {

    }

    void Start()
    {
        firstNote = true;
        characterController = GetComponent<CharacterController>();
        movementController = GetComponent<PlayerScript>();
        pickupScript = pickUpObject.GetComponent<PickupScript>();

        playerCamera = Camera.main;

        ContractorEventSystem.instance.onPickUpItemTrigger += PickUpObject;
        ContractorEventSystem.instance.onPickUpGeneric += SwitchExamineMode;
        ContractorEventSystem.instance.onDropItemTrigger += DropObject;
        ContractorEventSystem.instance.onDropGeneric += SwitchMovementMode;
        ContractorEventSystem.instance.onPlayerRemainStationary += SwitchStationaryMode;
        ContractorEventSystem.instance.onPlayerBeginMoving += SwitchMovementMode;
        ContractorEventSystem.instance.eatItemTrigger += EatNote;
        ContractorEventSystem.instance.onPlayerTeleportToToilet += SwitchStationaryMode;
        ContractorEventSystem.instance.onExitLevelTrigger += SwitchStationaryMode;
        ContractorEventSystem.instance.onPickUpGunTrigger += PickUpGun;

    }

    // Update is called once per frame
    void Update()
    {

        switch (m_playerState)
        {
            case PlayerState.Movement:
                movementController.UpdateCameraRotation();
                movementController.UpdatePlayerMovement();

                if (selectedObject && Input.GetButtonDown("Fire1")) {                   
                    selectedObject.GetComponent<Interactable>().OnInteract();
                    return;
                }
                

                //handle gun stuff below
                if (m_gun == null)
                {
                    return;
                }

                if (Input.GetButtonDown("Fire1"))
                {
                    m_gun.PullTrigger();
                }

                break;

            case PlayerState.Examining:
                if (heldObject != null)
                {
                    pickupScript.RotateObject(heldObject, playerCamera.transform);
                    if (Input.GetButtonDown("Fire2")) heldObject.GetComponent<Interactable>().OnDrop();
                    
                }
                break;

            case PlayerState.Stationary:
                movementController.ClampCameraRotation();
                break;
            case PlayerState.Menu:

                break;


        }
            

        
    }

    void FixedUpdate()
    {

        if (selectedObject) ContractorEventSystem.instance.StopHovering();

        selectedObject = GetTargetedInteractable();

        if (selectedObject)
        {
            ContractorEventSystem.instance.HoverOverInteractable();
        }
    }

    void PickUpObject(GameObject targetObject)
    {
        StopAllCoroutines();
        if (heldObject != null)
        {
            heldObject.transform.position = _oldPosition;
            heldObject.transform.rotation = _oldRotation;
            heldObject.transform.parent = null;
            heldObject.GetComponent<BoxCollider>().enabled = true;
            heldObject = null;
        }

        heldObject = targetObject;
        ItemInteract tempInteractData = heldObject.GetComponent<ItemInteract>();
        heldItemData = tempInteractData.itemDataSO;
        itemSource.PlayOneShot(heldItemData.pickupSound);
        //targetObject.GetComponent<MeshCollider>().enabled = false;
        targetObject.GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(LerpToMe(targetObject));
    }

    void DropObject()
    {
        StopAllCoroutines();
        heldObject.layer = 7;
        itemSource.PlayOneShot(heldItemData.dropSound);
        heldItemData = null;
        //heldObject.GetComponent<MeshCollider>().enabled = false;
        heldObject.GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(LerpBack(heldObject));
    }

    IEnumerator LerpToMe(GameObject target)
    {

        //equip();
        _oldPosition = target.transform.position;
        _oldRotation = target.transform.rotation;

        Vector3 targetPosition = pickUpObject.transform.position; /*+ pickUpObject.transform.forward * heldItemData.zoomDistance;*/

        target.transform.position = targetPosition;
        target.transform.LookAt(Camera.main.transform.position, Vector3.up);
        target.transform.position += target.transform.forward * heldItemData.zoomDistance;
        target.transform.Rotate(-heldItemData.rotationX, -heldItemData.rotationY, -heldItemData.rotationZ);

        targetPosition = target.transform.position;
        Quaternion targetRotation = target.transform.rotation;

        target.transform.position = _oldPosition;
        target.transform.rotation = _oldRotation;

        float lerpDuration = collectLerpCurve.keys[collectLerpCurve.length - 1].time;

        for (float lerpElapsed = 0; lerpElapsed < lerpDuration; lerpElapsed += Time.deltaTime)
        {
            float interpolationFactor = collectLerpCurve.Evaluate(lerpElapsed / lerpDuration);

            target.transform.position = Vector3.LerpUnclamped(_oldPosition, targetPosition, interpolationFactor);
            target.transform.rotation = Quaternion.LerpUnclamped(_oldRotation, targetRotation, interpolationFactor);
            yield return null;
        }
        heldObject.layer = 12;
        target.transform.parent = pickUpObject.transform;
    }

    IEnumerator LerpBack(GameObject target)
    {

        //equip();
        Vector3 initialPosition = target.transform.position;
        Quaternion initialRotation = target.transform.rotation;

        float lerpDuration = collectLerpCurve.keys[collectLerpCurve.length - 1].time;




        for (float lerpElapsed = 0; lerpElapsed < lerpDuration; lerpElapsed += Time.deltaTime)
        {
            float interpolationFactor = collectLerpCurve.Evaluate(lerpElapsed / lerpDuration);

            target.transform.position = Vector3.LerpUnclamped(initialPosition, _oldPosition, interpolationFactor);
            target.transform.rotation = Quaternion.LerpUnclamped(initialRotation, _oldRotation, interpolationFactor);
            yield return null;
        }

        target.transform.parent = null;
        //heldObject.GetComponent<MeshCollider>().enabled = true;
        heldObject.GetComponent<BoxCollider>().enabled = true;
        heldObject = null;
    }

    IEnumerator LerpGunToPosition(GameObject gun)
    {
        ChangeAllLayerMasks(gun, 12);

        gun.layer = 12;
        Vector3 initialPosition = gun.transform.position;
        Quaternion initialRotation = gun.transform.rotation;

        float lerpDuration = collectLerpCurve.keys[collectLerpCurve.length - 1].time;


        for (float lerpElapsed = 0; lerpElapsed < lerpDuration; lerpElapsed += Time.deltaTime)
        {
            float interpolationFactor = collectLerpCurve.Evaluate(lerpElapsed / lerpDuration);

            gun.transform.position = Vector3.LerpUnclamped(initialPosition, gunPosition.transform.position, interpolationFactor);
            gun.transform.rotation = Quaternion.LerpUnclamped(initialRotation, gunPosition.transform.rotation, interpolationFactor);
            yield return null;
        }

        gun.transform.parent = gunPosition.transform;
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localEulerAngles = Vector3.zero;

        ContractorEventSystem.instance.OnlyGenericDropItem();

        m_gun.CheckAmmo();
    }
    void PickUpGun(GameObject gun)
    {
        m_gun = gun.GetComponent<GunScript>();
        StartCoroutine(LerpGunToPosition(gun));
    }



    GameObject GetTargetedInteractable()
    {

        Vector3 cameraDirection = playerCamera.transform.TransformDirection(Vector3.forward);

        int interactLayer = 7;
        int layerMask = 1 << interactLayer;

        RaycastHit hit;

        bool isHit = Physics.Raycast(playerCamera.transform.position, cameraDirection, out hit, 2, layerMask);

        if (isHit)
        {
            return hit.collider.gameObject;
        }


        return null;
    }

    void SwitchMovementMode()
    {
        m_playerState = PlayerState.Movement;
        characterController.enabled = true;
        movementController.UpdateClampValues(90.0f);
    }

    void SwitchExamineMode()
    {
        m_playerState = PlayerState.Examining;
    }

    void SwitchStationaryMode()
    {
        characterController.enabled = false;
        m_playerState = PlayerState.Stationary;
    }

    void EatNote(GameObject note)
    {
        m_playerState = PlayerState.Menu;
        StartCoroutine(NoteEatAnimation(note));
    }


    IEnumerator NoteEatAnimation(GameObject note)
    {
        _oldPosition = note.transform.position;
        _oldRotation = note.transform.rotation;

        Vector3 targetPosition = pickUpObject.transform.position; /*+ pickUpObject.transform.forward * heldItemData.zoomDistance;*/

        note.transform.position = targetPosition;
        note.transform.LookAt(Camera.main.transform.position, Vector3.up);
        note.transform.position += note.transform.forward * (heldItemData.zoomDistance * 2);
        note.transform.Rotate(-heldItemData.rotationX, -heldItemData.rotationY, -heldItemData.rotationZ);

        targetPosition = note.transform.position;
        Quaternion targetRotation = note.transform.rotation;

        note.transform.position = _oldPosition;
        note.transform.rotation = _oldRotation;


        float lerpDuration = collectLerpCurve.keys[collectLerpCurve.length - 1].time;

        for (float lerpElapsed = 0; lerpElapsed < lerpDuration; lerpElapsed += Time.deltaTime)
        {
            float interpolationFactor = collectLerpCurve.Evaluate(lerpElapsed / lerpDuration);
            note.transform.rotation = Quaternion.LerpUnclamped(_oldRotation, targetRotation, interpolationFactor);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        eatSource.Play();
        crumpleSource.Play();

        for (float lerpElapsed = 0; lerpElapsed < lerpDuration; lerpElapsed += Time.deltaTime)
        {
            float interpolationFactor = collectLerpCurve.Evaluate(lerpElapsed / lerpDuration);
            note.transform.position = Vector3.LerpUnclamped(_oldPosition, targetPosition, interpolationFactor);
            yield return null;
        }

        note.SetActive(false);

        if (firstNote)
        {
            ContractorEventSystem.instance.OnlyGenericDropItem();
            firstNote = false;
        }
    }

    void ChangeAllLayerMasks(GameObject objectToChange, int layerMask)
    {
        objectToChange.layer = layerMask;
        foreach (Transform child in objectToChange.transform)
        {
            child.gameObject.layer = layerMask;

            Transform _HasChildren = child.GetComponentInChildren<Transform>();
            if (_HasChildren != null)
                ChangeAllLayerMasks(child.gameObject, layerMask);

        }
    }

}
