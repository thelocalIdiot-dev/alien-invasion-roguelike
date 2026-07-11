using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyHealth : MonoBehaviour
{
    [Header("stun stuff")]
    public bool stuned;   
    public float stunDuration;   
    public float stunTimer;      

    [Header("Flash")]
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashDuration = 0.1f;

    [Header("drops")]
    public GameObject miniHealOrb;
    public GameObject[] drops;
    public int XPorbs;
    [Range(0, 100)] public float propability;
    public float offset = 5;

    [Header("Health")]
    public float maxHealth = 300f;
    public float currentHealth;
    public float deathShake;

    [Header("VFX")]
    public GameObject deahBlood;
    public GameObject blood;
    public Transform bloodPosition;
    public GameObject stunPartical;

    private Renderer[] renderers;
    private Material[][] originalMaterials;
    private Coroutine flashRoutine;

    EnemyReferences EF;

    void Awake()
    {
        EF = GetComponent<EnemyReferences>();

        currentHealth = maxHealth;

        // Get ALL renderers (MeshRenderer + SkinnedMeshRenderer)
        renderers = GetComponentsInChildren<Renderer>();

        // Cache original materials
        originalMaterials = new Material[renderers.Length][];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].materials;
        }
    }

    public void TakeDamage(Vector3 hitPos,float amount)
    {       
        currentHealth -= amount;

        Flash();
        Instantiate(blood, hitPos, Quaternion.identity);
        Vector3 FinalOffset = new Vector3(Random.Range(-offset, offset), Random.Range(-offset, offset), Random.Range(-offset, offset));
        
        if (currentHealth <= 0)
        {           
            Die();
        }
    }

    void Update()
    {
        stunPartical.SetActive(stuned);
        if (stuned)
        {
            if(stunTimer >= stunDuration)
            {
                stuned = false;
            }
            else
            {
                stunTimer += Time.deltaTime;
            }
        }
    }

    public void GetStunned(float duration)
    {
        stuned = true;
        stunDuration = duration;
    }    
    
    void Die()
    {
        if (bloodPosition != null)
        {
            GameObject gib = Instantiate(deahBlood, bloodPosition.position, Quaternion.identity);
            Destroy(gib, 1f);
        }
        else if(bloodPosition == null)
        {
            GameObject gib = Instantiate(deahBlood, transform.position, Quaternion.identity);
            Destroy(gib, 1f);
        }
        
        droploot(drops[Random.Range(0, drops.Length)]);
        Destroy(gameObject);
    }

    public void spawnOrbs()
    {
        Vector3 FinalOffset = new Vector3(Random.Range(-offset, offset), Random.Range(-offset, offset), Random.Range(-offset, offset));

        if (bloodPosition != null)
        {
            Instantiate(miniHealOrb, bloodPosition.position + FinalOffset, Quaternion.identity);
        }
        else
        {
            Instantiate(miniHealOrb, transform.position + FinalOffset, Quaternion.identity);
        }
    }

    public void Flash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashRoutine());
    }



    IEnumerator FlashRoutine()
    {
        // Apply flash material to all renderers
        foreach (Renderer r in renderers)
        {
            Material[] flashMats = new Material[r.materials.Length];
            for (int i = 0; i < flashMats.Length; i++)
                flashMats[i] = flashMaterial;

            r.materials = flashMats;
        }

        yield return new WaitForSeconds(flashDuration);

        // Restore original materials
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].materials = originalMaterials[i];
        }

        flashRoutine = null;
    }

    void droploot(GameObject lootDrop)
    {
        float digit = Random.Range(1, 101);

        if (digit <= propability)
        {
            Instantiate(lootDrop, transform.position, Quaternion.identity);
        }       
    }
    private void OnDrawGizmosSelected()
    {
       // Gizmos.color = Color.yellow;
       // if (bloodPosition != null)
       // {
       //     Gizmos.DrawWireSphere(bloodPosition.position, offset);
       // }
       // else
       // {
       //     Gizmos.DrawWireSphere(transform.position, offset);
       // }
    }
}
