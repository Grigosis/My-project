using ClassLibrary1;
using ClassLibrary1.Logic;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestAnswerBehaviour : MonoBehaviour
{
    public QuestDialogBehaviour Parent;
    public Image Background;
    public TextMeshProUGUI Text;
    public Button Button;

    public Answer Answer;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAnswer(Answer answer, AnswerArgs answerArgs) {
        Answer = answer;
        Text.text = answerArgs.GenerateString();
    }

    public void OnClick() {
        Debug.LogWarning($"answer selected {Answer}");
        Parent.OnAnswerClicked(this);
    }
}
