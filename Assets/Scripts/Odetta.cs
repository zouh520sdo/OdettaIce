using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Odetta : CharactorBase
{
    public enum State
    {
        normal,
        attack,
        carry
    }

    public float meltDamage = 1f;

    public State state;

    public float baseAttackTime;
    public GameObject weapon;
    public float attackRange;

    protected DemageCarrier dc;
    protected Rigidbody2D carryingRigid;
    protected GameObject copyCarryingRigid;

    private Mover odettaMover;
    private DialogManager dialogManager;

    protected override void OnAwake()
    {
        base.OnAwake();
        // Health slider initialization
        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        targetHealth = maxHealth;

        // State
        state = State.normal;

        // odettaMover
        odettaMover = GetComponent<Mover>();

        // Weapon
        weapon.SetActive(false);
        dc = weapon.GetComponent<DemageCarrier>();

        // Dialog Manager
        dialogManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DialogManager>();
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        // Slider follows Odetta
        RectTransform healthRect = healthSlider.GetComponent<RectTransform>();
        healthRect.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.4f);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        // Dialog flag checking
        //if (dialogManager.)

        healthChange(-meltDamage * Time.deltaTime);

        if (state == State.attack)
        {
            odettaMover.canMove = false;
        }
        else if (state == State.normal)
        {
            odettaMover.canMove = true;
            if (Input.GetKey(KeyCode.J))
            {
                StartCoroutine(baseAttack());
            }

            if (Input.GetKey(KeyCode.K))
            {
                checkForCarrying();
            }
            if (Input.GetKeyUp(KeyCode.K))
            {
                dropCarrying();
            }
        }

        // Update moving speed for odetta
        odettaMover.speed = Mathf.Lerp(odettaMover.minSpeed, odettaMover.maxSpeed, healthSlider.value / healthSlider.maxValue);
    }

    protected void checkForCarrying()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * dir, 1f, 1<<LayerMask.NameToLayer("Movable"));
        if (hit.collider)
        {
            print(hit.collider.name);
            //Grab this movable object
            if (carryingRigid != hit.collider.GetComponent<Rigidbody2D>() && !copyCarryingRigid)
            {
                carryingRigid = hit.collider.GetComponent<Rigidbody2D>();
                //carryingRigid.isKinematic = true;
                carryingRigid.transform.SetParent(transform);
                rigid.mass += (carryingRigid.mass / 2f);
                Vector3 localPos = carryingRigid.transform.localPosition;
                localPos.y = 0f;
                localPos.x = Mathf.Min(localPos.x + 0.1f);
                carryingRigid.transform.localPosition = localPos;

                // Create copy of carrying object
                copyCarryingRigid = Instantiate(carryingRigid.gameObject);
                copyCarryingRigid.GetComponent<BoxCollider2D>().sharedMaterial.friction = 0f;
                Destroy(copyCarryingRigid.GetComponent<StackWeightManager>());
                Destroy(copyCarryingRigid.GetComponent<Rigidbody2D>());
                copyCarryingRigid.transform.SetParent(transform);
                copyCarryingRigid.transform.localPosition = localPos;

                carryingRigid.gameObject.SetActive(false);
            }
        }
    }

    protected void dropCarrying()
    {
        if (carryingRigid)
        {
            Destroy(copyCarryingRigid);
            carryingRigid.gameObject.SetActive(true);
            //carryingRigid.isKinematic = false;
            carryingRigid.transform.SetParent(null);
            rigid.mass -= (carryingRigid.mass / 2f);
            carryingRigid = null;
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "IceBlockProduced")
        {
            healthChange(20f);
            Destroy(col.gameObject);
        }
        else if (col.gameObject.tag == "IceBlockCasted")
        {
            healthChange(7f);
            Destroy(col.gameObject);
        }
    }

    IEnumerator baseAttack()
    {
        state = State.attack;

        // Lose health every attack
        healthChange(-5f);

        weapon.transform.localPosition = Vector3.zero;
        dc.clearEnemies();
        weapon.SetActive(true);

        float timestamp = Time.time;

        while (Time.time - timestamp <= baseAttackTime)
        {
            float x = attackRange * Mathf.Sin(Mathf.PI / baseAttackTime * (Time.time - timestamp));
            weapon.transform.localPosition = new Vector3(x, 0, 0);
            yield return null;
        }
        weapon.SetActive(false);
        state = State.normal;
    }
}