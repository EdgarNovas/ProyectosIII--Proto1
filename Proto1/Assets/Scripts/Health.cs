using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header ("Test")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int testStep = 10; 

    public int health;
    private bool isInvulnerable;

    public event Action OnTakeDamage;
    public event Action OnDie;
    public event Action OnHealthChanged; 

    public bool IsDead => health == 0;
    public int MaxHealth => maxHealth; 

    void Start()
    {
        health = maxHealth;
        OnHealthChanged?.Invoke(); 
    }

    public void SetInvulnerable(bool isInvulnerable)
    {
        this.isInvulnerable = isInvulnerable;
    }

    public void DealDamage(int damage)
    {
        if (health == 0 || isInvulnerable) { return; }

        health = Mathf.Max(health - damage, 0);

        OnHealthChanged?.Invoke(); 
        OnTakeDamage?.Invoke();

        if (health == 0)
        {
            OnDie?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        if (health == maxHealth) { return; }

        health = Mathf.Min(health + amount, maxHealth);

        OnHealthChanged?.Invoke(); 
    }

    public int GetHealth()
    {
        return health;
    }

    //Test Metode
    void Update()
    {
        TestInput();
    }

    private void TestInput()
    {
        if ((Input.GetKeyDown(KeyCode.Equals) && Input.GetKey(KeyCode.LeftShift)) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Heal(testStep);
        }

        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            DealDamage(testStep);
        }
    }
}