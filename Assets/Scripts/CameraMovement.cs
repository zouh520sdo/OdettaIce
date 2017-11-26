using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    public GameObject odetta, ice;

    private float leftDis, rightDis;
    void Awake ()
    {
        /*
        leftDis = leftBound.transform.position.x - transform.position.x;
        rightDis = rightBound.transform.position.x - transform.position.x;
        */
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        // Update left and rigth bounds of camera
        /*
        Vector3 leftPos = leftBound.transform.position;
        leftPos.x = transform.position.x + leftDis;
        leftPos.y = transform.position.y;
        leftBound.transform.position = leftPos;
        Vector3 rightPos = rightBound.transform.position;
        rightPos.x = transform.position.x + rightDis;
        rightPos.y = transform.position.y;
        rightBound.transform.position = rightPos;
        */

        // Update camera's position
        Vector3 camPos = transform.position;
        if (odetta && ice)
        {
            camPos.x = (odetta.transform.position.x + ice.transform.position.x) / 2.0f;
        }
        else if (!ice && odetta)
        {
            if (Vector3.Distance(camPos, odetta.transform.position) > 0.01f)
            {
                camPos.x = Mathf.MoveTowards(camPos.x, odetta.transform.position.x, 8f * Time.deltaTime);
            }
            else
            {
                camPos.x = odetta.transform.position.x;
            }
        }
        else if (!odetta && ice)
        {
            if (Vector3.Distance(camPos, ice.transform.position) > 0.01f)
            {
                camPos.x = Mathf.MoveTowards(camPos.x, ice.transform.position.x, 8f * Time.deltaTime);
            }
            else
            {
                camPos.x = ice.transform.position.x;
            }
        }
        transform.position = camPos;
    }
}