using UnityEngine;
using System.Collections;

public class FlyingIceBlock : MonoBehaviour {

    public float iceFlyingRange = 5f;
    public float iceFlyingSpeed = 10f;
    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void FireIce(Vector2 castPoint)
    {
        StartCoroutine(LaunchIce(castPoint));
    }

    IEnumerator LaunchIce(Vector2 castPoint)
    {
        Rigidbody2D iceRigid = GetComponent<Rigidbody2D>();
        Vector2 origin = iceRigid.position;
        Vector2 dir = (castPoint - origin).normalized;
        iceRigid.AddTorque(50f);
        iceRigid.velocity = iceFlyingSpeed * dir;

        while (Vector2.Distance(origin, iceRigid.position) < iceFlyingRange)
        {
            print(iceRigid.velocity);
            yield return null;
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Odetta")
        {
            print("Hit Odetta");
        }

        print("Casted ice hit " + col.collider.name);

        Destroy(gameObject);
    }
}
