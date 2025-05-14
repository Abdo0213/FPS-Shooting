using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDataPersistence
{
    
    private float health;
    private float lerpTime;
    [Header("Haelt Bar")]
    public float maxHealth = 100f;
    public float chipSpeed = 2f;
    public Image bar;
    public Image backBar;

    [Header("Damage Overlay")]
    public Image overlay;
    public float duration;
    public float fadeSpeed;
    private float durationTimer;
    public GameOverManager gameOverManager;
    void Start()
    {
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.b, overlay.color.b, 0);
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        if(overlay.color.a > 0){
            if(health < 30){
                return;
            }
            durationTimer += Time.deltaTime;
            if(durationTimer > duration){
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.b, overlay.color.b, tempAlpha);
            }
        }
    }
    public void LoadData(GameData data){
        this.health = data.health;
    }
    public void SaveData(ref GameData data){
        data.health = this.health;
    }
    public void UpdateHealthUI(){
        float fillF = bar.fillAmount;
        float fillB = backBar.fillAmount;
        float hFraction = health/maxHealth;
        if(fillB > hFraction){
            bar.fillAmount = hFraction;
            backBar.color = Color.red;
            lerpTime += Time.deltaTime;
            float percentComplete = lerpTime / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillF < hFraction){
            backBar.color = Color.green;
            backBar.fillAmount = hFraction;
            lerpTime += Time.deltaTime;
            float percentComplete = lerpTime / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            bar.fillAmount = Mathf.Lerp(fillF, backBar.fillAmount, percentComplete);
        }
    }

    public void TakeDamage(float damage){
        health -= damage;
        lerpTime = 0f;
        duration = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.b, overlay.color.b, 1);
        if(health <= 0){
            health = 0;
            gameOverManager.Over();
        }
    }
    public void RestoreHealth(float healAmount){
        health += healAmount;
        lerpTime = 0f;
    }
}
