using UnityEngine;
using System.Collections;

public class JumpDetection : MonoBehaviour {

    public bool isOnGround { get { return _isOnGround; } }
    private bool _isOnGround;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay2D(Collider2D col)
    {
        _isOnGround = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {  
        _isOnGround = false;
    }
}
