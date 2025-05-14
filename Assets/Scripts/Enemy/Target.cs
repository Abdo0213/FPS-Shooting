using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Target : MonoBehaviour, IDataPersistence
{
    public string id;
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid(){
        id = System.Guid.NewGuid().ToString();
    }
    public float health = 50f;
    public GameObject player;
    public AudioSource audioSource;
    [SerializeField] private float soundDistance = 30f;
    [SerializeField] private float volumeValueChanger;
    public bool IsDead ;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        volumeValueChanger = PlayerPrefs.GetFloat("musicVolume");
    }

    void Update()
    {
        VolumeChanger();
    }
    private void VolumeChanger(){
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if(distance < soundDistance && distance > 0){
            audioSource.volume = Mathf.Clamp(volumeValueChanger / distance, 0f, volumeValueChanger);
        } else {
            audioSource.volume = 0;
        }
    }
    public void TakeDamage(float amount ){
        health -= amount;
        if(health <= 0 ){
            Die();
        }
    }

    public void LoadData(GameData data){
        data.enemyKilled.TryGetValue(id, out IsDead);
        if(IsDead){
            Destroy(gameObject);
        }
    }
    public void SaveData(ref GameData data){
        if(data.enemyKilled.ContainsKey(id)){
            data.enemyKilled.Remove(id);
        }
        data.enemyKilled.Add(id, IsDead);
    }
    void Die() {
        IsDead = true;
        StartCoroutine(DieAfterAnimation());
    }

    IEnumerator DieAfterAnimation() {
        Animator animator = GetComponent<Animator>();
        
        if (animator != null) {
            animator.SetTrigger("hit");
            
            // Wait until the "Hit" animation starts
            yield return null;
            
            // Wait for the animation to finish
            float animLength = animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(animLength);
            
            // Extra 2-second delay
            yield return new WaitForSeconds(1f);
        }
        
        Destroy(gameObject);
    }
}
