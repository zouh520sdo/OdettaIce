using UnityEngine;
using System.Collections;
using System;

public class TriggerDoor : WeightPadReceiver {

    public float liftingSpeed = 3f;
    public Vector3 moveDelta;
    public bool isOn;

    protected Vector3 targetPos;
    protected Vector3 originPos;

    public override void TurnOff()
    {
        if (isOn)
        {
            isOn = false;
            StartCoroutine(LifeDonw());
        }
    }

    public override void TurnOn()
    {
        if (!isOn)
        {
            isOn = true;
            StartCoroutine(LiftUp());
        }
    }

    void Awake ()
    {
        originPos = transform.position;
        targetPos = transform.position + moveDelta;
        isOn = false;
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator LiftUp()
    {
        while (isOn && transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, liftingSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator LifeDonw()
    {
        while (!isOn && transform.position != originPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, originPos, 10f * liftingSpeed * Time.deltaTime);
            yield return null;
        }
    }
}