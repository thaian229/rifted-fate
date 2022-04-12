using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public Camera fpsCam;
    public Transform muzzle;
    public Text ammoText;
    public float damage = 10f;
    public float shootRange = 100f;
    public int ammoCapacity = 6;
    public float reloadTime = 2f;
    public float fireRate = 1.5f;

    private int currentAmmo = 6;
    private float fireInterval;
    private float fireTimer = 0f;
    private float reloadTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        fireInterval = 1 / fireRate;
        currentAmmo = ammoCapacity;
        ammoText.text = "Ammo: " + currentAmmo;
    }

    void FixedUpdate()
    {
        if (fireTimer > 0f)
        {
            fireTimer -= Time.deltaTime;
        }

        if (reloadTimer > 0f)
        {
            reloadTimer -= Time.deltaTime;
        }
    }

    public void Shoot()
    {
        if (fireTimer <= 0.01f && reloadTimer <= 0.01f)
        {
            if (currentAmmo <= 0)
            {
                Reload();
                return;
            }

            fireTimer = fireInterval;
            currentAmmo--;

            ammoText.text = "Ammo: " + currentAmmo;

            RaycastHit hit;

            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, shootRange))
            {
                Debug.DrawRay(muzzle.transform.position, (hit.point - muzzle.transform.position).normalized, Color.blue);
                Hitable target = hit.transform.GetComponent<Hitable>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }
            }
            else
            {
                Debug.DrawRay(muzzle.transform.position, fpsCam.transform.forward, Color.blue);
            }
        }
    }

    public void Reload()
    {
        if (reloadTimer <= 0.01f && currentAmmo < ammoCapacity)
        {
            reloadTimer = reloadTime;
            StartCoroutine(_Reload());
        }
    }

    private IEnumerator _Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = ammoCapacity;
        ammoText.text = "Ammo: " + currentAmmo;
    }
}
