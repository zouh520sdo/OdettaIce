using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DemageCarrier : MonoBehaviour {

    public float damage = 0f;
    public float fireDamage = 0f;
    public bool consumable;

    protected List<Enemy> enemies;

	// Use this for initialization
	void Start () {
        enemies = new List<Enemy>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            Enemy e = col.GetComponent<Enemy>();
            if (e && !enemies.Contains(e))
            {
                enemies.Add(e);
                e.DamageTaken(damage, fireDamage);
                if (transform.root.tag == "Odetta" && e.type == Enemy.Type.fire)
                {
                    CharactorBase c = transform.root.GetComponent<CharactorBase>();
                    c.healthChange(-40f);
                }
                if (consumable)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Enemy")
        {
            Enemy e = col.collider.GetComponent<Enemy>();
            if (e && !enemies.Contains(e))
            {
                enemies.Add(e);
                e.DamageTaken(damage, fireDamage);

                if (transform.root.tag == "Odetta" && e.type == Enemy.Type.fire)
                {
                    CharactorBase c = transform.root.GetComponent<CharactorBase>();
                    c.healthChange(-40f);
                }

                if (consumable)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.collider.tag == "Ice")
        {
            CharactorBase c = col.collider.GetComponent<CharactorBase>();
            if (c)
            {
                if (damage > 0)
                {
                    c.DamageTaken(-damage);
                }
                if (consumable)
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (col.collider.tag == "Odetta")
        {
            CharactorBase c = col.collider.GetComponent<CharactorBase>();
            if (c)
            {
                if (fireDamage > 0)
                {
                    c.DamageTaken(-fireDamage);
                }
                if (consumable)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void clearEnemies()
    {
        if (enemies != null) 
            enemies.Clear();
    }
}
