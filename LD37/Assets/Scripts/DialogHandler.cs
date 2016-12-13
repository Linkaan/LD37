using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogHandler : MonoBehaviour {

    public enum DialogState
    {
        do_nothing,
        showing_dialog,

        show_dialog1,
        show_dialog2,
        show_dialog3,
        show_dialog4,

        shown_dialog1,
        shown_dialog2,
        shown_dialog3,
        shown_dialog4,

        show_dialog5,
        show_dialog6,

        shown_dialog5,
        shown_dialog6,

        show_enddialog,
        shown_enddialog
    }

    public CanvasGroup dialog1;
    public CanvasGroup dialog2;
    public CanvasGroup dialog3;
    public CanvasGroup dialog4;

    public CanvasGroup dialog5;
    public CanvasGroup dialog6;

    public CanvasGroup endDialog;

    private Coroutine fader;
    private float startTime;

    public DialogState dialogState;

    private void FadeInventoryGroup(CanvasGroup group, float alpha, float delay, DialogState stateToBeSet)
    {
        dialogState = DialogState.showing_dialog;
        if (fader != null)
            StopCoroutine(fader);

        fader = StartCoroutine(EFadeOutInventory(group, alpha, delay, stateToBeSet));
    }

    private IEnumerator EFadeOutInventory(CanvasGroup group, float alpha, float delay, DialogState stateToBeSet)
    {
        yield return new WaitForSeconds(delay);

        float progress = 1f - Mathf.Abs(group.alpha - alpha);
        float start = group.alpha;
        while (progress < 1f)
        {
            progress += Time.deltaTime;
            group.alpha = Mathf.Lerp(start, alpha, progress);
            yield return 0;
        }
        dialogState = stateToBeSet;
    }

    void Start () {
        dialogState = DialogState.show_dialog1;
    }
	
	void Update () {
	    switch (dialogState)
        {
            case DialogState.show_dialog1:
                FadeInventoryGroup(dialog1, 1, 1.0f, DialogState.shown_dialog1);
                break;
            case DialogState.shown_dialog1:
                FadeInventoryGroup(dialog1, 0, 1.0f, DialogState.show_dialog2);
                break;
            case DialogState.show_dialog2:
                FadeInventoryGroup(dialog2, 1, 0.5f, DialogState.shown_dialog2);
                break;
            case DialogState.shown_dialog2:
                FadeInventoryGroup(dialog2, 0, 1.0f, DialogState.show_dialog3);
                break;
            case DialogState.show_dialog3:
                FadeInventoryGroup(dialog3, 1, 0.5f, DialogState.shown_dialog3);
                break;
            case DialogState.shown_dialog3:
                FadeInventoryGroup(dialog3, 0, 1.0f, DialogState.show_dialog4);
                break;
            case DialogState.show_dialog4:
                FadeInventoryGroup(dialog4, 1, 0.5f, DialogState.shown_dialog4);
                break;
            case DialogState.shown_dialog4:
                FadeInventoryGroup(dialog4, 0, 1.0f, DialogState.do_nothing);
                break;

            case DialogState.show_dialog5:
                FadeInventoryGroup(dialog5, 0.5f, 0.5f, DialogState.shown_dialog5);
                break;
            case DialogState.shown_dialog5:
                FadeInventoryGroup(dialog5, 0, 1.0f, DialogState.show_dialog6);
                break;
            case DialogState.show_dialog6:
                FadeInventoryGroup(dialog6, 1, 0.5f, DialogState.shown_dialog6);
                break;
            case DialogState.shown_dialog6:
                FadeInventoryGroup(dialog6, 0, 1.0f, DialogState.do_nothing);
                break;
            case DialogState.show_enddialog:
                FadeInventoryGroup(endDialog, 1, 1.0f, DialogState.shown_enddialog);
                endDialog.transform.Find("Play again").gameObject.SetActive(true);
                endDialog.transform.Find("Quit").gameObject.SetActive(true);
                break;
            default:
                break;
        }
	}

    public void LoadLevel()
    {
        SceneManager.LoadScene(0);    
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SetState (DialogState state)
    {
        this.dialogState = state;
    }

    public void ShowEndDialog()
    {
        dialogState = DialogState.shown_enddialog;
    }
}
