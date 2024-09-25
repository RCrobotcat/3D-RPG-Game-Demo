using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueUI : Singleton<DialogueUI>
{
    [Header("Basic Elements")]
    public Image icon;
    public Text MainText;
    public Button NextBtn;
    public GameObject dialoguePanel;

    [Header("Data")]
    public DialogueData_SO currentDialogue;
    int currentIndex = 0;

    protected override void Awake()
    {
        base.Awake();
        NextBtn.onClick.AddListener(ContinueDialogue);
    }

    void ContinueDialogue()
    {
        if (currentIndex < currentDialogue.dialoguePieces.Count)
            UpdateMainDialogue(currentDialogue.dialoguePieces[currentIndex]);
        else dialoguePanel.SetActive(false);
    }

    public void UpdateDialogueData(DialogueData_SO data)
    {
        currentDialogue = data;
        currentIndex = 0;
    }

    public void UpdateMainDialogue(DialoguePiece piece)
    {
        dialoguePanel.SetActive(true);

        if (piece.image != null)
        {
            icon.enabled = true;
            icon.sprite = piece.image;
        }
        else icon.enabled = false;

        MainText.text = "";
        // MainText.text = piece.text;
        MainText.DOText(piece.text, 1f);

        if (piece.dialogueOption.Count == 0 && currentDialogue.dialoguePieces.Count > 0)
        {
            NextBtn.gameObject.SetActive(true);
            currentIndex++;
        }
        else NextBtn.gameObject.SetActive(false);
    }
}
