using System;
using System.Collections.Generic;
using ClassLibrary1;
using ClassLibrary1.Logic;
using ROR.Core.Serialization;
using SecondCycleGame.Assets.Scripts.AbstractNodeEditor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDialogBehaviour : MonoBehaviour {
    private static string[] Delimiter = { "\r\n###\r\n" };

    private string[] Texts = new string[0];
    private int CurrentText = 0;

    private bool _ShowAnswers = false;
    private bool ShowAnswers {
        get => _ShowAnswers;
        set {
            if(_ShowAnswers != value) {
                _ShowAnswers = value;
                foreach(var b in AnswerBehaviours) {
                    ControlAnswerVisible(b);
                }
                NextText.gameObject.SetActive(!value);
            }
        }
    }

    public Image Background;
    public TextMeshProUGUI text;
    public GameObject AnswersContainer;
    public Button NextText;
    public GameObject AnswerPrefab;

    public QuestDialog Dialog;
    public QuestContext QuestContext;
    
    private List<QuestAnswerBehaviour> AnswerBehaviours = new List<QuestAnswerBehaviour>();



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

    public void OnNextTextClick() {
        int newText = CurrentText + 1;
        if(newText >= Texts.Length) {
            Debug.LogError($"Text Idx Owerflow {CurrentText} -> {newText} (length = {Texts.Length})");
            return;
        }
        CurrentTextChange(newText);
    }

    private void OnQuestionChange() {
        Texts = Dialog.Text.Split(Delimiter, StringSplitOptions.None);
        CurrentTextChange(0);
        OnAnswersChange();
    }

    private void CurrentTextChange(int currentText) {
        CurrentText = currentText;
        text.text = Texts[CurrentText];
        ShowAnswers = CurrentText >= Texts.Length - 1;
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
                QuestAnswerBehaviour behaviour = AddAnswerBehaviour(newAnswers[i]);
                AnswerBehaviours.Add(behaviour);
                ControlAnswerVisible(behaviour);
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

    private void ControlAnswerVisible(QuestAnswerBehaviour behaviour) {
        behaviour.gameObject.SetActive(ShowAnswers);
    }
}
