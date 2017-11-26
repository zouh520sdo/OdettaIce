using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyFireSpirite : Enemy {

    public GameObject fireBulletPref;
    public float reFireDuration;
    public float fireDistance;
    public float fireSpeed;
    public Vector3 bulletOffset;
    public bool isFiring;

    protected Rigidbody2D rigid;
    protected int dir;
    protected float switchDelay;
    protected List<GameObject> bullets;

    protected override void OnStart()
    {
        base.OnStart();
        bullets = new List<GameObject>();
        dir = 0;
        switchDelay = 0.8f;
        rigid = GetComponent<Rigidbody2D>();
        StartCoroutine(fire_CR());
        if (state == EnemyState.roam)
        {
            StartCoroutine(roaming());
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        LayerMask layer = 1 << LayerMask.NameToLayer("Ice") | 1 << LayerMask.NameToLayer("Odetta");
        RaycastHit2D hit = Physics2D.Raycast(transform.position - dir * new Vector3(10, 0,0) + new Vector3(0, -1f, 0), dir * Vector3.right, 30f, layer);
        // Debug.DrawLine(transform.position - dir * new Vector3(10, 0, 0) + new Vector3(0, -1f, 0), transform.position + dir * new Vector3(20, 0, 0) + new Vector3(0, -1f, 0), Color.blue, 0.1f);
        if (hit.collider)
        {
            if (state == EnemyState.roam)
            {
                state = EnemyState.attack;
                StartCoroutine(attacking(hit.collider.gameObject));
            }
        }
        else
        {
            if (state == EnemyState.attack)
            {
                state = EnemyState.roam;
                StartCoroutine(delayRoaming());
            }
        }
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (rigid.velocity.x < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
            dir = -1;
        }
        else if (rigid.velocity.x > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
            dir = 1;
        }
    }

    void OnDestroy()
    {
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
        bullets.Clear();
    }

    protected void fire()
    {
        GameObject tempBullet = Instantiate(fireBulletPref);
        tempBullet.layer = LayerMask.NameToLayer("Enemy");
        tempBullet.transform.position = transform.position + bulletOffset;
        Vector3 scale = tempBullet.transform.localScale;
        scale.y *= Mathf.Sign(transform.localScale.x);
        scale *= transform.localScale.y;
        tempBullet.transform.localScale = scale;
        bullets.Add(tempBullet);

        Vector3 dir = -transform.right * Mathf.Sign(transform.localScale.x);
        StartCoroutine(moveFireBullet_CR(tempBullet.transform.position, dir.normalized, tempBullet));
    }

    IEnumerator moveFireBullet_CR(Vector2 originPos, Vector2 dir, GameObject bullet)
    {
        while (bullet && Vector2.Distance(originPos, bullet.transform.position) <= fireDistance)
        {
            bullet.transform.position += (Vector3)dir * fireSpeed * Time.deltaTime;
            yield return null;
        }

        if (bullet)
        {
            bullets.Remove(bullet);
            Destroy(bullet);
        }
    }

    IEnumerator fire_CR()
    {
        float timestamp = Time.time;
        while (Time.time - timestamp <= reFireDuration)
        {
            yield return null;
        }
        if (isFiring)
        {
            fire();
        }
        StartCoroutine(fire_CR());
    }

    IEnumerator delayRoaming()
    {
        float timestamp = Time.time;
        while (Time.time - timestamp <= switchDelay && state == EnemyState.roam)
        {
            yield return null;
        }
        
        if (state == EnemyState.roam)
        {
            isFiring = false;
            StartCoroutine(roaming());
        }
    }

    IEnumerator roaming()
    {
        float roamingTime = Random.Range(1f, 5f);
        int dir = Random.Range(0f, 1f) <= 0.5f ? 1 : -1;
        float timestamp = Time.time;
        mover.enemyDir = dir;
        mover.canMove = true;
        while (Time.time - timestamp <= roamingTime && state == EnemyState.roam)
        {
            yield return null;
        }
        mover.canMove = false;
        mover.enemyDir = 0;
        float idleTime = Random.Range(2f, 6f);
        timestamp = Time.time;
        while (Time.time - timestamp <= idleTime && state == EnemyState.roam)
        {
            yield return null;
        }

        if (state == EnemyState.roam)
        {
            StartCoroutine(roaming());
        }
    }

    IEnumerator attacking(GameObject target)
    {
        isFiring = true;
        Vector3 rel = target.transform.position - transform.position;
        mover.canMove = true;
        if (rel.x < 0)
        {
            mover.enemyDir = -1;
        }
        else if (rel.x > 0)
        {
            mover.enemyDir = 1;
        }
        yield return null;
        
        if (state == EnemyState.attack)
        {
            StartCoroutine(attacking(target));
        }
    }
}
