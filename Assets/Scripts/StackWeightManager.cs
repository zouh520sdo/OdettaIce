using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class StackWeightManager : MonoBehaviour {

    public static List<WeightPadTrigger> weightPads;
    public bool isOnPad;
    public List<Rigidbody2D> stackWith;
    public int stackAmount;

    Rigidbody2D rigid;

    void Awake ()
    {
        if (weightPads == null)
        {
            weightPads = new List<WeightPadTrigger>();
            GameObject[] weightPadObjs = GameObject.FindGameObjectsWithTag("WeightPad");
            foreach (GameObject obj in weightPadObjs)
            {
                weightPads.Add(obj.GetComponent<WeightPadTrigger>());
            }
        }
        rigid = GetComponent<Rigidbody2D>();
        stackWith = new List<Rigidbody2D>();
        isOnPad = false;
        stackAmount = 0;
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

        // Check if the stacked object is no longer on the weight pad
        

        // This object is not on the surface of weight pad,
        // 
	    if (!isOnPad && stackWith.Count == 0)
        {

        }
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        Rigidbody2D colRigid = col.collider.GetComponent<Rigidbody2D>();
        if (colRigid)
        {
            foreach (WeightPadTrigger pad in weightPads)
            {
                if (pad.rigids.ContainsKey(colRigid))
                {
                    print("Weight pad stay in " + pad.gameObject.name);
                    if (!pad.rigidsOnPad.Contains(rigid))
                    {
                        StackWeightManager stackedPad = colRigid.GetComponent<StackWeightManager>();
                        stackedPad.stackWith.Add(rigid);
                        stackAmount++;
                        pad.rigidsOnPad.Add(rigid);
                        pad.rigids[rigid] = Vector3.down;
                    }
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        
    }

    void OnCollisionExit2D(Collision2D col)
    {
        Rigidbody2D colRigid = col.collider.GetComponent<Rigidbody2D>();
        if (colRigid)
        {
            foreach (WeightPadTrigger pad in weightPads)
            {
                if (pad.rigids.ContainsKey(colRigid))
                {
                    print("Weight pad exit from " + pad.gameObject.name);
                    pad.rigidsOnPad.Remove(rigid);
                    if (pad.rigids.ContainsKey(rigid))
                    {
                        pad.DelayRemoveRigid(rigid);
                    }
                }
            }
        }
    }

    void OnDestroy()
    {
        foreach (WeightPadTrigger pad in weightPads)
        {
            if (pad && pad.rigids.ContainsKey(rigid))
            {
                print("Weight pad exit from " + pad.gameObject.name);
                pad.rigidsOnPad.Remove(rigid);
                pad.rigids.Remove(rigid);

                foreach (Rigidbody2D r in stackWith)
                {
                    if (!r)
                    {
                        continue;
                    }

                    StackWeightManager s = r.GetComponent<StackWeightManager>();
                    s.stackAmount--;
                    if (s.stackAmount <= 0)
                    {
                        pad.rigidsOnPad.Remove(r);
                        pad.rigids.Remove(r);
                    }
                }
                stackWith.Clear();
            }
        }
    }
}
