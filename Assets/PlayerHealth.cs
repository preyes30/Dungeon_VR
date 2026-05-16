using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Slider healthBar;

    private bool isDead = false;
    public GameObject loseText;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; 

        currentHealth -= damage;
        healthBar.value = currentHealth;

        Debug.Log("Player took damage: " + damage + " | Current HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
void Die()
{
    if (isDead) return;
    isDead = true;

    Debug.Log("Player died");

    loseText.SetActive(true);
    Time.timeScale = 0f;      
}

}