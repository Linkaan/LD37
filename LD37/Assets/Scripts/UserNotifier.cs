using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserNotifier : MonoBehaviour {

    private Coroutine textCoroutine;

    private CanvasGroup group;

    private Text textComponent;

    void Start () {
        this.group = this.GetComponent<CanvasGroup>();
        this.textComponent = this.transform.GetComponentInChildren<Text>();

    }
	
	void Update () {
		
	}

    public void ShowText (string text, float time)
    {
        if (textCoroutine != null)
            StopCoroutine(textCoroutine);

        group.alpha = 1;
        textComponent.text = text;
        textCoroutine = StartCoroutine(EShowText(time));
    }

    private IEnumerator EShowText(float time)
    {
        yield return new WaitForSeconds(time);

        float progress = 1f - Mathf.Abs(group.alpha - 0);
        float start = group.alpha;
        while (progress < 1f)
        {
            progress += Time.deltaTime;
            group.alpha = Mathf.Lerp(start, 0, progress);
            yield return 0;
        }
        textComponent.text = "";
    }
}
