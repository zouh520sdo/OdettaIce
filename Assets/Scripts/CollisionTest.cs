using UnityEngine;
using System.Collections;

public class CollisionTest : MonoBehaviour {

    public GameObject player;
	// Use this for initialization
	void Start () {
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
