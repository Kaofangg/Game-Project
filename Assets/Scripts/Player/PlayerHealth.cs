using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // เพิ่มส่วนนี้

public class PlayerHealth : Singleton<PlayerHealth>
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

    private Slider healthSlider;
    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;
    const string HEALTH_SLIDER_TEXT = "Health Slider";

    protected override void Awake() {
        base.Awake();

        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start() {
        currentHealth = maxHealth;

        UpdateHealthSlider();
    }

    private void OnCollisionStay2D(Collision2D other) {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if (enemy) {
            TakeDamage(1, other.transform);
        }
    }

    public void HealPlayer() {
        if (currentHealth < maxHealth) {
            currentHealth += 1;
            UpdateHealthSlider();
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform) {
        if (!canTakeDamage) { return; }

        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath() {
        if (currentHealth <= 0) {
            currentHealth = 0;
            Debug.Log("Player Death");
            // เรียกใช้ฟังก์ชันสำหรับโหลดฉาก Game Over
            StartCoroutine(LoadGameOverScene());
        }
    }

    private IEnumerator LoadGameOverScene() {
        yield return new WaitForSeconds(2f); // รอ 2 วินาทีเพื่อให้เห็นว่าผู้เล่นตาย
        SceneManager.LoadScene("GameOverScene"); // แทนที่ด้วยชื่อฉาก Game Over ของคุณ
    }

    private IEnumerator DamageRecoveryRoutine() {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider() {
        if (healthSlider == null) {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
    public void ResetHealth() {
    currentHealth = maxHealth;
    UpdateHealthSlider();
    canTakeDamage = true; // เปิดการรับดาเมจใหม่
}

}
