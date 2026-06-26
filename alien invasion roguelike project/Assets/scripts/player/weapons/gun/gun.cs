using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class gun : MonoBehaviour
{
    /* ─────────────── WEAPON STATS ─────────────── */
    [Header("Weapon Stats")]
    public float damage;
    public float knockBack;
    public float range;
    public float fireRate;
    public float bulletsPerShot;
    public float spread;

    /* ─────────────── AMMO ─────────────── */
    [Header("Ammo")]
    public float ammo;
    [HideInInspector] public float bulletsLeft;
    public float reloadTime;
    private bool reloading;

    /* ─────────────── REFERENCES ─────────────── */
    [Header("References")]
    public Camera cam;
    public Transform[] gunTips;
    public Slider ammoSlider;

    /* ─────────────── VISUAL EFFECTS ─────────────── */
    [Header("Visual Effects")]
    public GameObject muzzleFlash;
    public GameObject impactEffect;
    public GameObject bloodEffect;
    public GameObject hitFlash;
    public float hitFlashDuration;

    /* ─────────────── CAMERA SHAKE ─────────────── */
    [Header("Camera Shake")]
    public float shootShakePower;
    public float hitShakePower;
    public float shootShakeDuration;
    public float hitShakeDuration;

    /* ─────────────── PRIVATE STATE ─────────────── */
    private float nextTimeToFire;
    private int currentGunTip;

    /* ─────────────── UNITY METHODS ─────────────── */
    private void Awake()
    {
        currentGunTip = 0;
        bulletsLeft = ammo;
        hitFlash.SetActive(false);
    }

    private void Update()
    {
        //if (GetComponentInParent<PlayerHealth>().alive == false) return;

        UpdateAmmoUI();

        if (CanShoot())
        {
            Shoot();
        }

        if ((Input.GetKeyDown(KeyCode.R) || bulletsLeft <= 0) && !reloading)
        {
            StartReload();
        }
    }

    /* ─────────────── SHOOTING ─────────────── */
    private bool CanShoot()
    {
        return Input.GetMouseButton(0)
            && Time.time >= nextTimeToFire
            && bulletsLeft > 0
            && !reloading;
    }

    private void Shoot()
    {
        if(bulletsLeft > ammo) { bulletsLeft = ammo; }

        nextTimeToFire = Time.time + 1f / fireRate;


        //SoundManager.PlaySound(SoundType.shoot);        

        PlayMuzzleFlash();
        CycleGunTip();

        for (int i = 0; i < bulletsPerShot; i++)
        {
            FireRay();
        }
    }

    private void FireRay()
    {
        Vector3 bulletSpread = cam.transform.forward +
            new Vector3(
                Random.Range(-spread, spread),
                Random.Range(-spread, spread),
                0f
            );

        if (Physics.Raycast(cam.transform.position, bulletSpread, out RaycastHit hit, range))
        {
            
            if (hit.transform.TryGetComponent(out EnemyHealth enemy))
            {
                enemy.TakeDamage(hit.point, damage);
                //CameraShaker.Instance.ShakeOnce(hitShakePower, 0.5f, 0, hitShakeDuration);
                StartCoroutine(HitFlashRoutine());
            }
            else
            {
                SpawnImpact(impactEffect, hit);
                //CameraShaker.Instance.ShakeOnce(shootShakePower, 0.5f, 0, shootShakeDuration);
            }

            if (hit.transform.TryGetComponent(out Rigidbody EnemyRB))
            {
                SpawnImpact(impactEffect, hit);
                EnemyRB.AddForce(bulletSpread * knockBack, ForceMode.Impulse);
            }

            //if (hit.transform.TryGetComponent(out bomb bomb))
            //{
            //    CameraShaker.Instance.ShakeOnce(4, 15, 0, 3);
            //    bomb.explode(redExplosion);
            //}
        }
    }

    /* ─────────────── RELOAD ─────────────── */
    private void StartReload()
    {
        reloading = true;
        Invoke(nameof(EndReload), reloadTime);
    }

    private void EndReload()
    {
        bulletsLeft = ammo;
        reloading = false;
    }

    /* ─────────────── EFFECTS ─────────────── */
    private void PlayMuzzleFlash()
    {
        GameObject flash = Instantiate(muzzleFlash, gunTips[currentGunTip].position, gunTips[currentGunTip].rotation);
        flash.transform.parent = gunTips[currentGunTip].transform;
        Destroy(flash, 0.1f);
    }

    private void SpawnImpact(GameObject effect, RaycastHit hit)
    {
        GameObject impact = Instantiate(effect, hit.point, Quaternion.LookRotation(hit.normal));
        Debug.Log(hit.transform.name);
        Destroy(impact, 0.7f);
    }

    private IEnumerator HitFlashRoutine()
    {
        hitFlash.SetActive(true);
        yield return new WaitForSeconds(hitFlashDuration);
        hitFlash.SetActive(false);
    }

    /* ─────────────── UTILITIES ─────────────── */
    private void CycleGunTip()
    {
        currentGunTip++;
        if (currentGunTip >= gunTips.Length)
            currentGunTip = 0;
    }

    private void UpdateAmmoUI()
    {
        float targetValue = bulletsLeft / ammo;
        ammoSlider.value = Mathf.Lerp(ammoSlider.value, targetValue, 0.25f);
    }

    /* ─────────────── Upgrade ─────────────── */

  // public void upgrade(UpGradeSO UPGSO)
  // {
  //     if (UPGSO.weaponID != 0) { return; }
  //
  //     if(UPGSO.upGradeID == 0)
  //     {
  //         damage *= UPGSO.upGradeAmount;
  //     }
  //     if (UPGSO.upGradeID == 1)
  //     {
  //         fireRate *= UPGSO.upGradeAmount;
  //     }
  //     if (UPGSO.upGradeID == 2)
  //     {
  //         knockBack *= UPGSO.upGradeAmount;
  //     }
  //     if (UPGSO.upGradeID == 3)
  //     {
  //         ammo *= UPGSO.upGradeAmount;
  //         bulletsLeft *= UPGSO.upGradeAmount;
  //     }
  //     if (UPGSO.upGradeID == 4)
  //     {
  //         reloadTime *= UPGSO.upGradeAmount;
  //     }
  // }
}
