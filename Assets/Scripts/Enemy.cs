using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(DemageCarrier))]
public class Enemy : MonoBehaviour {

    public enum EnemyState
    {
        roam,
        attack
    }

    public enum Type
    {
        normal,
        fire
    }

    public Type type;
    public EnemyState state;
    public float health;
    public float currentHealth;
    public Slider healthBar;

    protected Mover mover;

    void Awake()
    {
        OnAwake();
    }

    void FixedUpdate()
    {
        OnFixedUpdate();
    }

	// Use this for initialization
	void Start () {
        OnStart();
	}
	
	// Update is called once per frame
	void Update () {
        OnUpdate();
	}

    protected virtual void OnAwake()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        state = EnemyState.roam;
        mover = GetComponent<Mover>();
        currentHealth = health;
        if (healthBar)
        {
            healthBar.maxValue = health;
            healthBar.value = health;
        }
    }

    protected virtual void OnFixedUpdate()
    {
        if (healthBar)
        {
            RectTransform healthRect = healthBar.GetComponent<RectTransform>();
            healthRect.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.3f);
        }
    }
    protected virtual void OnStart()
    {

    }
    protected virtual void OnUpdate()
    {

    }

    public virtual void DamageTaken(float amount, float fireAmount) {
        if (type == Type.fire)
        {
            currentHealth -= fireAmount;
        }
        else
        {
            currentHealth -= amount;
        }
        currentHealth = Mathf.Min(health, Mathf.Max(currentHealth, 0));

        if (healthBar)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            if (healthBar)
            {
                healthBar.gameObject.SetActive(false);
            }
            Destroy(gameObject);
        }
    }
}