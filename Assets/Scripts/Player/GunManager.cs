using System.Collections;
using UnityEngine;

public class GunManager : MonoBehaviour, IDataPersistence
{
    public PlayerUI playerUI;
    public float damage = 10f;
    public float range = 100f;
    public Camera fpsCam;
    public GameObject muzzlePrefab;
    public Transform muzzlePoint;
    public GameObject impactEffect;
    public float magazineSize;
    public AudioClip bullet;
    public AudioClip emptyWeapon;
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume =PlayerPrefs.GetFloat("musicVolume");
        audioSource.clip = bullet;
        playerUI.UpdateAmmo(magazineSize);
    }
    void Update()
    {
        audioSource.volume =PlayerPrefs.GetFloat("musicVolume");
        if (magazineSize <= 0){
            audioSource.clip = emptyWeapon;
        }
    }
    public void LoadData(GameData data){
        this.magazineSize = data.ammo;
    }
    public void SaveData(ref GameData data){
        data.ammo = this.magazineSize;
    }

    public void Shoot()
    {
        RaycastHit hit;
        if (fpsCam == null)
        {
            Debug.LogWarning("FPS Camera is not assigned!");
            return;
        }
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            StartCoroutine(FlashMuzzle());
            Target target = hit.transform.GetComponent<Target>();
            if (target != null && magazineSize > 0)
            {
                target.TakeDamage(damage);
            }

            // Safe check for impactEffect
            if (impactEffect != null && magazineSize > 0)
            {
                GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGo, 2f); // Destroy after a delay to see effect
            }
            else
            {
                Debug.LogWarning("Impact Effect is not assigned!");
            }
            magazineSize = (magazineSize - 1) >= 0 ? magazineSize - 1 : 0;
            playerUI.UpdateAmmo(magazineSize);
            audioSource.Play();
        }
        
    }
    public void Reload(){
        magazineSize = 50;
        playerUI.UpdateAmmo(magazineSize);
    }

    IEnumerator FlashMuzzle()
    {
        if (muzzlePrefab != null && magazineSize > 0)
        {
            muzzlePrefab.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            muzzlePrefab.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Muzzle Prefab is not assigned!");
        }
    }
}
