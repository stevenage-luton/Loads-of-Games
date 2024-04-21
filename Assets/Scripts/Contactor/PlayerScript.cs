using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    Camera playerCamera;
    CharacterController characterController;

    [SerializeField]
    AudioSource footstepSource;

    float acceleration;
    float deceleration;

    // speed
    public float walkSpeed;

    float targetSpeed;

    Vector3 velocity;

    /// headbob stuff
    public AnimationCurve headbob;
    float strideSample;
    float headbobScale;

    /// camera Clamp Vals
    float clampX = 90.0f;
    float clampY = 90.0f;

    float cameraY;
    float localRotation;

    float rotationX;
    float rotationY;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        footstepSource = GetComponent<AudioSource>();
        playerCamera = Camera.main;

        cameraY = playerCamera.transform.localPosition.y;

        float timeToAccelerate = 0.05f;
        acceleration = walkSpeed / timeToAccelerate;

        float timeToDecelerate = 0.05f;
        deceleration = walkSpeed / timeToDecelerate;

        velocity = Vector3.zero;

        ContractorEventSystem.instance.onPlayerTeleportToToilet += ToiletClamp;
    }

    private void OnEnable()
    {
        HideCursor();
    }
    Vector3 GetInputVector()
    {
        return new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    }

    void UpdateHeadbob(float deltaHeadbob)
    {
        headbobScale = Mathf.Clamp01(headbobScale + deltaHeadbob);

        if (headbobScale <= 0) strideSample = 0;

        playerCamera.transform.localPosition += Vector3.up * Mathf.SmoothStep(headbob.Evaluate(0), headbob.Evaluate(strideSample), headbobScale);
    }

    void Accelerate(Vector3 direction)
    {
        /// apply acceleration
        Vector3 deltaV = direction * acceleration * Time.deltaTime;
        velocity += deltaV;

        /// limit speed to target
        float speed = Mathf.Min(velocity.magnitude, targetSpeed);
        velocity = velocity.normalized * speed;
    }

    void Decelerate()
    {
        /// reduce speed / apply deceleration
        float deltaSpeed = deceleration * Time.deltaTime;
        float speed = Mathf.Max(velocity.magnitude - deltaSpeed, 0);
        velocity = velocity.normalized * speed;
    }

    void updateFootstep(float deltaDistance)
    {
        float strideDistance = headbob.keys[headbob.length - 1].time;

        float contactTime = strideDistance - headbob.keys[1].time;

        float sampleBefore = strideSample - deltaDistance;

        bool isFootstep = (strideSample + contactTime) % strideDistance < (sampleBefore + contactTime) % strideDistance;

        if (isFootstep)
        {
            footstepSource.pitch = Random.Range(0.7f, 1);
            footstepSource.Play();
        }
    }

    public void UpdateClampValues(float newClamp)
    {
        clampX = newClamp;
        clampY = newClamp;
    }

    public void UpdateCameraRotation()
    {
        playerCamera.transform.localPosition = Vector3.up * cameraY;
        playerCamera.transform.localEulerAngles = Vector3.zero;

        transform.Rotate(new Vector3(0, Input.GetAxisRaw("Mouse X"), 0), Space.World);

        localRotation -= Input.GetAxisRaw("Mouse Y");
        localRotation = Mathf.Clamp(localRotation, -clampY, clampY);

        playerCamera.transform.localEulerAngles += Vector3.right * localRotation;
    }

    public void ClampCameraRotation()
    {
        playerCamera.transform.localPosition = Vector3.up * cameraY;
        playerCamera.transform.localEulerAngles = Vector3.zero;


        rotationX += Input.GetAxis("Mouse X");
        rotationX = Mathf.Clamp(rotationX, -clampX, clampX);

        rotationY += Input.GetAxis("Mouse Y");
        rotationY = Mathf.Clamp(rotationY, -clampY, clampY);

        playerCamera.transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
    }

    void UpdatePlayerGravity()
    {
        if (!characterController.isGrounded)
        {
            characterController.Move(new Vector3(0, Constants.GRAVITATIONAL_CONSTANT * Time.deltaTime, 0));
        }
    }

    void ToiletClamp()
    {
        UpdateClampValues(20.0f);
    }

    public void UpdatePlayerMovement()
    {

        UpdatePlayerGravity();


        /// get direction of input
        Vector3 inputVector = transform.TransformDirection(GetInputVector());

        /// accelerate or decelerate based on input
        if (inputVector.magnitude == 0)
        {
            targetSpeed = 0;
        }
        else
        {
            targetSpeed = walkSpeed;

        }

        if (velocity.magnitude - targetSpeed <= deceleration * Time.deltaTime)
        {
            Accelerate(inputVector);
        }
        else
        {
            Decelerate();
        }

        /// apply velocity
        Vector3 deltaPosition = velocity * Time.deltaTime;
        characterController.Move(deltaPosition);


        /// progress stride and weight headbob
        strideSample += deltaPosition.magnitude;


        float deltaLerp = Time.deltaTime / ((targetSpeed == 0) ? -0.4f : 0.2f);
        UpdateHeadbob(deltaLerp);

        /// footstep sound process
        updateFootstep(deltaPosition.magnitude);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UpdateClampX(float newVal)
    {
        clampX = newVal;
    }
    void UpdateClampY(float newVal)
    {
        clampY = newVal;
    }
}
