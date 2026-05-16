using UnityEngine;

public class Monster : MonoBehaviour
{
    public int health = 100;
    public MonsterHealthBar healthBar;

    void Start()
    {
        healthBar.SetMaxHealth(health);
    }

    public void TakeDamage(int damage)
{

    health -= damage;


    healthBar.SetHealth(health);

    if (health <= 0)
    {
        Die();
    }
}

    void Die()
    {
        Destroy(gameObject);
    }
}