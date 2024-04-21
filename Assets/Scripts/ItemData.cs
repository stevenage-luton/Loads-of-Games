using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/ItemData")]
public class ItemData : ScriptableObject
{
    public float rotationX = 0.0f;
    public float rotationY = 0.0f;
    public float rotationZ = 0.0f;

    public float clampX;
    public float clampY;

    [Range(-1f, 1f)]
    public float zoomDistance;

    public AudioClip pickupSound;
    public AudioClip dropSound;
}
