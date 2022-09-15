using System.Collections.Generic;
using ClassLibrary1;
using ClassLibrary1.Logic;
using ROR.Core.Serialization;
using SecondCycleGame.Assets.Scripts.AbstractNodeEditor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDialogBehaviour : MonoBehaviour
{
    public Image Background;
    public TextMeshProUGUI text;
    public GameObject AnswersContainer;
    public GameObject AnswerPrefab;

    public QuestDialog Dialog;
    public QuestContext QuestContext;
    
    public List<QuestAnswerBehaviour> AnswerBehaviours;



    // Update is called once per frame
    void Update()
    {
    }

    public void SetDialog(QuestDialog dialog) {
        Dialog = dialog;
        OnQuestionChange();
    }

    public void OnAnswerClicked(QuestAnswerBehaviour answerBehaviour) {
        Debug.LogWarning($"answer selected2 {answerBehaviour.Answer}");
        var next = answerBehaviour.Answer.NextQuestionDialog;
        if(next != null) {
            SetDialog(next);
        }
    }

    private void OnQuestionChange() {
        text.text = Dialog.Text;
        OnAnswersChange();
    }

    private void OnAnswersChange() {
        List<QuestAnswer> newAnswers = new List<QuestAnswer>(Dialog.Answers.Count);
        for (int i = 0; i < Dialog.Answers.Count; i++) {
            QuestAnswer a = Dialog.Answers[i];
            var isVisibleCombinator = a.BuildCombinator(QuestContext);
            Debug.Log($"{a.Text} is {isVisibleCombinator.NodeDebugName} = {isVisibleCombinator.Value}");
            if (isVisibleCombinator.Value) {
                newAnswers.Add(a);
            }
        }
        int lengthForUpdate = Mathf.Min(AnswerBehaviours.Count, newAnswers.Count);
        for(int i = 0; i < lengthForUpdate; i++) {
            AnswerBehaviours[i].SetAnswer(newAnswers[i]);
        }
        if(newAnswers.Count > lengthForUpdate) {
            for(int i = lengthForUpdate; i < newAnswers.Count; i++) {
                AnswerBehaviours.Add(AddAnswerBehaviour(newAnswers[i]));
            }
        } else if(AnswerBehaviours.Count > lengthForUpdate) {
            for (int i = AnswerBehaviours.Count - 1; i >= lengthForUpdate; i--) {
                DestroyAnswerBehaviour(AnswerBehaviours[i]);
            }
            AnswerBehaviours.RemoveRange(lengthForUpdate, AnswerBehaviours.Count - lengthForUpdate);
        }
        
    }

    private QuestAnswerBehaviour AddAnswerBehaviour(QuestAnswer answer) {
        GameObject answerObject = Instantiate(AnswerPrefab, AnswersContainer.transform);
        QuestAnswerBehaviour target = answerObject.GetComponent<QuestAnswerBehaviour>();
        target.Parent = this;
        target.SetAnswer(answer);
        return target;
    }

    private void DestroyAnswerBehaviour(QuestAnswerBehaviour behaviour) {
        behaviour.Parent = null;
        behaviour.transform.SetParent(null);
        Destroy(behaviour.gameObject);
    }
}
