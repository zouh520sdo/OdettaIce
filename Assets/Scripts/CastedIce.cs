using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class CastedIce : MonoBehaviour {

    public float duration = 30f;

    protected SpriteRenderer sprite;

    void Awake ()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

	// Use this for initialization
	void Start () {
        StartCoroutine(Melting());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator Melting()
    {
        Color c = sprite.color;
        float timestamp = Time.time;
        while (Time.time - timestamp < duration)
        {
            c.a = 1f - (Time.time - timestamp) / duration;
            sprite.color = c;
            yield return null;
        }

        Destroy(gameObject);
    }
}
