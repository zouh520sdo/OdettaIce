using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Button restartButton;
    public Vector3 offset = new Vector3(0f, 3.3f, 0f);

    Vector3 origin;
    Vector3 target;

    bool isButtonShow;

	// Use this for initialization
	void Start () {
        /*
        restartButton = GameObject.Find("RestartButton").GetComponent<Button>();
        origin = restartButton.transform.position;
        Vector3 originWorld = Camera.main.ScreenToWorldPoint(restartButton.transform.position);
        target = Camera.main.WorldToScreenPoint(originWorld + offset);

        isButtonShow = false;

        restartButton.transform.position = target;
        */
    }
	
	// Update is called once per frame
	void Update () {
        /*
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        float dis = Vector3.Distance(Camera.main.ScreenToWorldPoint( mousePos), Camera.main.ScreenToWorldPoint(restartButton.transform.position));
        if (dis < 7f)
        {
            StopCoroutine(HideRestart());
            StartCoroutine(ShowRestart());
        }
        else
        {
            StopCoroutine(ShowRestart());
            StartCoroutine(HideRestart());
        }

        */
	}

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator ShowRestart()
    {
        while (restartButton.transform.position != origin)
        {
            restartButton.transform.position = Vector3.MoveTowards(restartButton.transform.position, origin, 9f * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator HideRestart()
    {
        while (restartButton.transform.position != target)
        {
            restartButton.transform.position = Vector3.MoveTowards(restartButton.transform.position, target, 9f * Time.deltaTime);
            yield return null;
        }
    }
}