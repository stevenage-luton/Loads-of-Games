using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GunScript : MonoBehaviour
{
    Animator m_animator;

    AudioSource m_AudioSource;

    [SerializeField]
    AudioClip swingOpen;
    [SerializeField]
    AudioClip spinCylinder;
    [SerializeField]
    AudioClip closeCylinder;
    [SerializeField]
    AudioClip fire;
    [SerializeField]
    AudioClip triggerPull;

    [SerializeField]
    GameObject aboveToilet;

    [SerializeField]
    AnimationCurve lerpCurve;

    [SerializeField]
    VisualEffect m_muzzleFlash;

    [SerializeField]
    ParticleSystem m_muzzleSmoke;

    Transform barrel;

    int ammo;

    [SerializeField]
    GameObject bulletPrefab;

    public float shootForce = 20;

    Camera playerCam;

    public bool infiniteAmmo;

    void Start()
    {
        m_animator = GetComponent<Animator>();

        ContractorEventSystem.instance.onRemoveGunTrigger += RemoveFromToilet;
        barrel = transform.Find("Barrel");
        ammo = 3;

        playerCam = Camera.main;

        m_AudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Vector3 forward = barrel.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(barrel.position, forward, Color.magenta);
    }


    void RemoveFromToilet()
    {
        StartCoroutine(LerpUpwards());
    }

    IEnumerator LerpUpwards()
    {
        yield return new WaitForSeconds(3.0f);

        Vector3 targetPosition = aboveToilet.transform.position; /*+ pickUpObject.transform.forward * heldItemData.zoomDistance;*/

        float lerpDuration = lerpCurve.keys[lerpCurve.length - 1].time;

        for (float lerpElapsed = 0; lerpElapsed < lerpDuration; lerpElapsed += Time.deltaTime)
        {
            float interpolationFactor = lerpCurve.Evaluate(lerpElapsed / lerpDuration);

            transform.position = Vector3.LerpUnclamped(transform.position, targetPosition, interpolationFactor);

            yield return null;
        }

        ContractorEventSystem.instance.EquipGun(this.gameObject);
    }

    public void CheckAmmo()
    {
        m_animator.SetTrigger("CheckAmmo");
    }

    public void PullTrigger()
    {
        RotateCylinder();
        m_AudioSource.PlayOneShot(triggerPull);

        if (ammo > 0 || infiniteAmmo)
        {
            m_animator.SetTrigger("Fire");
        }
        else
        {
            m_animator.SetTrigger("EmptyFire");
        }

    }

    void Fire()
    {
        if (ammo <= 0 && !infiniteAmmo)
        {
            return;
        }
        ammo--;

        Ray bulletPath = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;

        Vector3 target;

        if (Physics.Raycast(bulletPath, out hit))
        {
            target = bulletPath.GetPoint(50);
        }
        else
        {
            target = Vector3.zero; 
        }

        target = target - barrel.position;

        GameObject bullet = Instantiate(bulletPrefab, barrel.position, barrel.rotation);

        bullet.transform.forward = target.normalized;

        bullet.GetComponent<Rigidbody>().AddForce(target.normalized * shootForce, ForceMode.Impulse);

        m_AudioSource.PlayOneShot(fire);
        m_muzzleFlash.Play();
        m_muzzleSmoke.Play();

        ContractorEventSystem.instance.GunFired();

        if (ammo <= 0)
        {
            ContractorEventSystem.instance.EnableLeaving();
        }
    }

    void SwingOpenCylinder()
    {
        m_AudioSource.PlayOneShot(swingOpen);
    }
    void RotateCylinder()
    {
        m_AudioSource.PlayOneShot(spinCylinder);
    }
    void CloseCylinder()
    {
        m_AudioSource.PlayOneShot(closeCylinder);
    }
}
