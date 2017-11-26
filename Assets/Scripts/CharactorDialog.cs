using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharactorDialog : MonoBehaviour {

    public Text dialog;

    void Awake()
    {
        // Dialog panel
        dialog.gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        dialogPanelFollowing();
    }

    protected virtual void dialogPanelFollowing()
    {
        Vector3 dialogPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2.3f);
        dialog.transform.position = dialogPos;
    }

    protected virtual IEnumerator dialogPopup(string content, float duration)
    {
        if (content != null)
        {
            dialog.text = content;
        }

        dialog.gameObject.SetActive(true);
        /*float timestamp = Time.time;
        while (Time.time - timestamp <= duration)
        {
            yield return null;
        }*/
        yield return new WaitForSeconds(duration);
        dialog.gameObject.SetActive(false);
    }

    public void PopupDialog(string content, float duration)
    {
        StartCoroutine(dialogPopup(content, duration));
    }
}
