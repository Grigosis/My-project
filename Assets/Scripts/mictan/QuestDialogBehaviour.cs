using ClassLibrary1.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDialogBehaviour : MonoBehaviour
{
    public Image Background;
    public TextMeshProUGUI text;

    public Dialog Dialog;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAnswerClicked(QuestAnswerBehaviour answerBehaviour) {
        Debug.LogWarning($"answer selected {answerBehaviour.Answer}");
    }
}
