using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Ice : CharactorBase
{
    public enum State
    {
        normal,
        casting,
        producing
    }

    public State state;
    public float castRange = 100f;
    public float preCastTime = 3f;
    public float produceTime = 3f;
    public Slider castingSlider;

    private GameObject amingArrow;
    private Mover iceMover;

    protected override void OnAwake()
    {
        base.OnAwake();
        state = State.normal;
        amingArrow = transform.FindChild("AimingArrow").gameObject;
        amingArrow.SetActive(false);

        // Casting slider initialization
        castingSlider.gameObject.SetActive(false);

        // Get Ice Mover
        iceMover = GetComponent<Mover>();
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        // Slider following Ice
        RectTransform rect = castingSlider.GetComponent<RectTransform>();
        rect.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.down * 1.3f);

        RectTransform healthRect = healthSlider.GetComponent<RectTransform>();
        healthRect.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.3f);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (Input.GetMouseButtonDown(0)) // Casting ice
        {
            state = State.casting;
            castingSlider.gameObject.SetActive(true);
            castingSlider.value = 0;
            StartCoroutine(PreCastIce(0));
        }
        else if (Input.GetMouseButtonDown(1)) // Producing ice
        {
            //print("press right button");
            state = State.producing;
            castingSlider.gameObject.SetActive(true);
            castingSlider.value = 0;
            StartCoroutine(ProduceIce(1));
        }

        // Can't move when casting and producing
        if (state == State.normal )
        {
            iceMover.canMove = true;
        }
        else
        {
            iceMover.canMove = false;
        }
    }

    protected void launchIce(Vector2 castPoint)
    {
        GameObject ice = Instantiate(castedIcePref);
        ice.transform.position = new Vector2(transform.position.x, transform.position.y);
        FlyingIceBlock flyingIce = ice.GetComponent<FlyingIceBlock>();
        flyingIce.FireIce(castPoint);
    }

    protected void throwIce()
    {
        GameObject ice = Instantiate(producedIcePref);
        ice.transform.position = new Vector2(transform.position.x, transform.position.y);
        Rigidbody2D iceRigid = ice.GetComponent<Rigidbody2D>();
        iceRigid.AddForce(Vector2.up * 600);
        iceRigid.AddForce(Vector2.right * Random.Range(-50f, 50f));
    }


    IEnumerator PreCastIce(int button)
    {
        float timestamp = Time.time;

        // Rotate arrow
        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x) - 90f;
        amingArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        amingArrow.SetActive(true);

        SpriteRenderer arrowSprite = amingArrow.GetComponent<SpriteRenderer>();
        castingSlider.maxValue = preCastTime;
        while (Time.time - timestamp < preCastTime && Input.GetMouseButton(button) && state == State.casting)
        {
            // Rotate arrow
            dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            angle = Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x) - 90f;
            amingArrow.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Change alpha of arrow
            Color c = arrowSprite.color;
            c.a = 0.4f * Mathf.Cos(Mathf.PI * 2f * (Time.time - timestamp)) + 0.6f;
            arrowSprite.color = c;

            castingSlider.value = Time.time - timestamp;
            yield return null;
        }
        amingArrow.SetActive(false);
        castingSlider.gameObject.SetActive(false);
        state = State.normal;
        if (Time.time - timestamp >= preCastTime)
        {
            // Start cast ice
            launchIce(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    IEnumerator ProduceIce(int button)
    {
        float timestamp = Time.time;
        castingSlider.maxValue = produceTime;
        while (Time.time - timestamp < produceTime && Input.GetMouseButton(button) && state == State.producing)
        {
            castingSlider.value = Time.time - timestamp;
            yield return null;
        }
        castingSlider.gameObject.SetActive(false);
        state = State.normal;
        if (Time.time - timestamp >= produceTime)
        {
            // Start generate a Ice
            throwIce();
        }
    }
}