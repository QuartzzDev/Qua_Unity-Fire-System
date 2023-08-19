////////////////////////////////////////////////////////////////////////////
//         -- Quartzz Fire System // QuartzzDev // quartzz.dll            //
////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.Collections;

public class FireController : MonoBehaviour
{
    [Header("Gerekliler - Qua Fire Controller")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public ParticleSystem muzzleFlash;
    public AudioSource fireSource;
    public AudioSource suppfireSource;
    public Animator gunAnimator;

    public float bulletSpeed = 10f;
    public float fireRate = 0.1f;

    public bool CanFire = true;
    private bool isFiring = false;

    public GameObject flashLight;
    public GameObject suppressor;

    public bool IsFlashlightAdd;
    private bool IsflashOpen = false;    
    public bool IsSuppressorAdd;
    private bool IsSuppressorOpen = false;


    public int ammo = 30;


    void Update()
    {
        if (ammo <= 0)                          // Mermi Yokken Ateş Edilemez
        {
            CanFire = false;
            
            if (ammo < 0)           // Mermi Bugunu Çözüyoruz
            {
                ammo++;
            }
        }
        else
        {
            CanFire = true;
        }



        if (Input.GetButton("Fire1") && !isFiring && CanFire)           // Atış yeri
        {
            isFiring = true;
            StartCoroutine(AutoFire());
        }



        if (!CanFire)                                                   // Ateş etme kapalıyken mermi azalmasın diye bir fonksiyon
        {
            // Hiçbir şey yapma
        }


        if (Input.GetKeyDown(KeyCode.L)&& IsFlashlightAdd && !IsflashOpen)      // Silaha Bağlı Flashlight ve Susturucu Yeri (İsteğe Bağlı Ekleniyor)
        {
            IsflashOpen = true;
            flashLight.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.L) && IsFlashlightAdd && IsflashOpen)
        {
            IsflashOpen = false;
            flashLight.SetActive(false);
        }


        if (Input.GetKeyDown(KeyCode.K) && IsSuppressorAdd && !IsSuppressorOpen)      
        {
            IsSuppressorOpen = true;
            suppressor.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.K) && IsSuppressorAdd && IsSuppressorOpen)
        {
            IsSuppressorOpen = false;
            suppressor.SetActive(false);
        }



    public IEnumerator AutoFire()
    {
        while (Input.GetButton("Fire1") && ammo > 0)
        {
            Fire();
            yield return new WaitForSeconds(fireRate);
        }

        isFiring = false;
        gunAnimator.SetBool("isFiring", false); 
    }

    void Fire()
    {
        ammo--;
        fireSource.Play();

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * bulletSpeed;

        gunAnimator.Play("fire", 0, 0);
    }
}
