using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeightPadTrigger : MonoBehaviour {

    public float triggerWeight;
    public bool isOn;
    public float speed = 0.2f;

    public EnemyFireSpirite reveiver;

    public WeightPadReceiver triggerReceiver;

    public Dictionary<Rigidbody2D, Vector2> rigids;
    public List<Rigidbody2D> rigidsOnPad;
    Vector2 verticalPressure;

    Vector2 originPos;
    Vector2 triggeredPos;

    float removeDelayTime = 0.2f;

    void Awake()
    {
        rigids = new Dictionary<Rigidbody2D, Vector2>();
        rigidsOnPad = new List<Rigidbody2D>();
        isOn = false;
        originPos = transform.position;
        triggeredPos = originPos + new Vector2(0f, -0.3f);
        tag = "WeightPad";
    }

	// Use this for initialization
	void Start () {
        verticalPressure = Vector2.zero;
    }
	
	// Update is called once per frame
	void Update () {
        verticalPressure = Vector2.zero;
        List<Rigidbody2D> tempKeys = new List<Rigidbody2D>(rigids.Keys);
        foreach (Rigidbody2D rigid in tempKeys)
        {
            if (rigid)
            {
                verticalPressure += (rigid.mass * rigids[rigid]);
            }
            else
            {
                rigids.Remove(rigid);
            }
        }
        //print(verticalPressure.y);
        if (verticalPressure.y <= -triggerWeight)
        {
            isOn = true;
            transform.position = Vector2.MoveTowards(transform.position, triggeredPos, speed * Time.deltaTime);
            if (reveiver)
            {
                reveiver.isFiring = true;
            }

            if (triggerReceiver != null)
            {
                triggerReceiver.TurnOn();
            }
        }
        else
        {
            isOn = false;
            transform.position = Vector2.MoveTowards(transform.position, originPos, speed * Time.deltaTime);
            if (reveiver)
            {
                reveiver.isFiring = false;
            }

            if (triggerReceiver != null)
            {
                triggerReceiver.TurnOff();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Rigidbody2D rigid = col.collider.GetComponent<Rigidbody2D>();
        if (rigid)
        {
            rigidsOnPad.Add(rigid);
            StackWeightManager stackManager = rigid.GetComponent<StackWeightManager>();
            if (stackManager)
            {
                stackManager.isOnPad = true;
            }
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        Rigidbody2D rigid = col.collider.GetComponent<Rigidbody2D>();
        if (rigid)
        {
            rigids[rigid] = col.contacts[0].normal;
        }

    }

    void OnCollisionExit2D(Collision2D col)
    {
        Rigidbody2D rigid = col.collider.GetComponent<Rigidbody2D>();
        if (rigid)
        {
            rigidsOnPad.Remove(rigid);
            if (rigids.ContainsKey(rigid))
            {
                StartCoroutine(DelayRemove(rigid));
            }
        }
    }

    IEnumerator DelayRemove(Rigidbody2D rigid)
    {
        float timestamp = Time.time;
        while (Time.time - timestamp <= removeDelayTime && !rigidsOnPad.Contains(rigid))
        {
            yield return null;
        }
        if (rigid && rigids.ContainsKey(rigid) && !rigidsOnPad.Contains(rigid))
        {
            rigids.Remove(rigid);
            StackWeightManager stackManager = rigid.GetComponent<StackWeightManager>();
            if (stackManager)
            {
                stackManager.isOnPad = false;
                foreach (Rigidbody2D r in stackManager.stackWith)
                {
                    if (!r)
                    {
                        continue;
                    }
                    StackWeightManager s = r.GetComponent<StackWeightManager>();
                    s.stackAmount--;
                    if (s.stackAmount <= 0)
                    {
                        rigidsOnPad.Remove(r);  
                        rigids.Remove(r);
                    }
                }
                stackManager.stackWith.Clear();
            }
        }
    }

    public void DelayRemoveRigid(Rigidbody2D rigid)
    {
        StartCoroutine(DelayRemove(rigid));
    }
}