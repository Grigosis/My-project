using ClassLibrary1;
using ClassLibrary1.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDialogBehaviour : MonoBehaviour
{
    public Image Background;
    public TextMeshProUGUI text;
    public GameObject AnswerPrefab;

    public Dialog Dialog;

    public Question Current;
    public List<QuestAnswerBehaviour> AnswerBehaviours;



    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAnswerClicked(QuestAnswerBehaviour answerBehaviour) {
        Debug.LogWarning($"answer selected {answerBehaviour.Answer}");
    }

    private void OnQuestionChange() {
        text.text = Current.QuestionText;
        OnAnswersChange();
    }

    private void OnAnswersChange() {
        List<Answer> newAnswers = new List<Answer>(Current.answers.Length);
        List<AnswerArgs> newAnswersArgs = new List<AnswerArgs>(Current.answers.Length);
        for (int i = 0; i < Current.answers.Length; i++) {
            Answer a = Current.answers[i];
            AnswerArgs args = a.AnswerFx(Current.Xml, a.Xml);
            if (args.IsVisible()) {
                newAnswers.Add(a);
                newAnswersArgs.Add(args);
            }
        }
        int lengthForUpdate = Mathf.Min(AnswerBehaviours.Count, newAnswers.Count);
        for(int i = 0; i < lengthForUpdate; i++) {
            AnswerBehaviours[i].SetAnswer(newAnswers[i], newAnswersArgs[i]);
        }
        if(AnswerBehaviours.Count < lengthForUpdate) {
            for(int i = AnswerBehaviours.Count; i < lengthForUpdate; i++) {
                AnswerBehaviours.Add(AddAnswerBehaviour(newAnswers[i]));
            }
        } else if(AnswerBehaviours.Count > lengthForUpdate) {
            for (int i = AnswerBehaviours.Count; i < lengthForUpdate; i++) {
                DestroyAnswerBehaviour(AnswerBehaviours[i]);
            }
            AnswerBehaviours.RemoveRange(lengthForUpdate, AnswerBehaviours.Count - lengthForUpdate);
        }
    }

    private QuestAnswerBehaviour AddAnswerBehaviour(Answer answer) {
        GameObject answerObject = Instantiate(AnswerPrefab, gameObject.transform);
        QuestAnswerBehaviour target = answerObject.GetComponent<QuestAnswerBehaviour>();
        target.Parent = this;
        target.Answer = answer;
        float x = 0f;
        float y = 0;
        //TODO positionate
        return target;
    }

    private void DestroyAnswerBehaviour(QuestAnswerBehaviour behaviour) {
        behaviour.Parent = null;
        behaviour.transform.SetParent(null);
        Destroy(behaviour.gameObject);
    }
}
