using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Text optionText;
    Button btn;

    DialoguePiece currentDialogue;

    string nextPieceID;

    void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnOptionClicked);
    }

    public void UpdateOption(DialoguePiece piece, DialogueOption option)
    {
        currentDialogue = piece;
        optionText.text = option.text;
        nextPieceID = option.targetID;
    }

    public void OnOptionClicked()
    {
        if (nextPieceID == "")
        {
            DialogueUI.Instance.dialoguePanel.SetActive(false);
            return;
        }
        else
        {
            DialogueUI.Instance.UpdateMainDialogue(DialogueUI.Instance.currentDialogue.dialogueDict[nextPieceID]);
        }
    }
}
