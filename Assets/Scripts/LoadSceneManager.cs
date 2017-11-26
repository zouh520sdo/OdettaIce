using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour {

    public List<CharactorBase> inCharactors;

    public GameObject panel;
    void Awake ()
    {
        inCharactors = new List<CharactorBase>();
        panel = GameObject.Find("TBD");
        panel.SetActive(false);
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Ice" || col.tag == "Odetta")
        {
            inCharactors.Add(col.GetComponent<CharactorBase>());
        }

        if (inCharactors.Count >= 2)
        {
            // Load next level
            GotoNextLevel();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Ice" || col.tag == "Odetta")
        {
            inCharactors.Remove(col.GetComponent<CharactorBase>());
        }
    }

    void GotoNextLevel()
    {
        print(SceneManager.GetActiveScene().buildIndex);
        print( "count" + SceneManager.sceneCountInBuildSettings);
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            panel.SetActive(true);
        }
    }
}