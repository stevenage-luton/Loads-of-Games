using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    float clampX;
    float clampY;


    public void RotateObject(GameObject heldObject, Transform cameraTransform)
    {
        //float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        //float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        Vector3 right = Vector3.Cross(cameraTransform.up, heldObject.transform.position - cameraTransform.position);
        Vector3 up = Vector3.Cross(heldObject.transform.position - cameraTransform.position, right);




        heldObject.transform.rotation = Quaternion.AngleAxis(-Input.GetAxisRaw("Mouse X"), up) * heldObject.transform.rotation;
        heldObject.transform.rotation = Quaternion.AngleAxis(Input.GetAxisRaw("Mouse Y"), right) * heldObject.transform.rotation;
    }

    void UpdateClampVals(float valX, float valY)
    {
        clampX = valX;
        clampY = valY;
    }
}
