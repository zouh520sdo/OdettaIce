using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharactorDialog))]
public class CharactorBase : MonoBehaviour {

    public float maxHealth;
    public GameObject producedIcePref;
    public GameObject castedIcePref;
    public float healthChangeSpeed = 50f;
    public Slider healthSlider;
    public float invulnerableTime;
    public int dir;

    protected bool isInvulnerable;

    protected float targetHealth;
    protected SpriteRenderer sr;
    protected Rigidbody2D rigid;

    protected CharactorDialog _charDialog;
    public CharactorDialog charDialog { get { return _charDialog; } }

    void Awake()
    {
        OnAwake();
    }

	// Use this for initialization
	void Start () {
        OnStart();
    }
	
	// Update is called once per frame
	void Update () {
        OnUpdate();
    }

    void FixedUpdate()
    {
        OnFixedUpdate();
    }

    void OnDestroy()
    {
        _charDialog.dialog.enabled = false;
        _charDialog.StopAllCoroutines();
    }

    protected virtual void OnAwake()
    {
        // Health slider initialization
        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        targetHealth = maxHealth;
        isInvulnerable = false;

        //Spirte renderer
        sr = GetComponent<SpriteRenderer>();

        // Direction
        dir = 1;

        // RigidBody 2d
        rigid = GetComponent<Rigidbody2D>();


        // Charactor Dialog
        _charDialog = GetComponent<CharactorDialog>();
    }

    protected virtual void OnStart()
    {

    }

    protected virtual void OnUpdate()
    {

    }

    protected virtual void OnFixedUpdate()
    {
        
    }


    public virtual void DamageTaken(float amount)
    {
        if (!isInvulnerable)
        {
            healthChange(amount);
            StartCoroutine(Invulnerable());
        }
    }

    /// <summary>
    /// Change health
    /// </summary>
    /// <param name="amount">Negative number for damage taken, positive number for healing</param>
    public void healthChange(float amount)
    {
        targetHealth += amount;
        targetHealth = Mathf.Max(healthSlider.minValue, Mathf.Min(targetHealth, healthSlider.maxValue));
        // Make sure only one SmoothHealthChange coroutine running
        // But be careful, do not 
        StopCoroutine(SmoothHealthChange());
        StartCoroutine(SmoothHealthChange());
    }

    IEnumerator SmoothHealthChange()
    {
        do
        {
            healthSlider.value = Mathf.MoveTowards(healthSlider.value, targetHealth, healthChangeSpeed * Time.deltaTime);
            if (healthSlider.value <= 0)
            {
                healthSlider.gameObject.SetActive(false);
                Destroy(gameObject);
            }
            yield return null;
        }
        while (healthSlider.value != targetHealth && healthSlider.value != healthSlider.minValue
            && healthSlider.value != healthSlider.maxValue);
    }

    IEnumerator Invulnerable()
    {
        isInvulnerable = true;
        float timestamp = Time.time;
        Color c;
        while (Time.time - timestamp <= invulnerableTime)
        {
            c = sr.color;
            c.a = 0.25f * Mathf.Cos(2f * Mathf.PI / 0.4f * (Time.time - timestamp)) + 0.75f;
            sr.color = c;
            yield return null;
        }
        c = sr.color;
        c.a = 1f;
        sr.color = c;
        isInvulnerable = false;
    }
}
