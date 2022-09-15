using ClassLibrary1;
using ClassLibrary1.Logic;
using ROR.Core.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestAnswerBehaviour : MonoBehaviour
{
    public QuestDialogBehaviour Parent;
    public Image Background;
    public TextMeshProUGUI Text;
    public Button Button;

    public QuestAnswer Answer;



    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAnswer(QuestAnswer answer) {
        Answer = answer;
        Text.text = answer.Text;
    }

    public void OnClick() {
        Debug.LogWarning($"answer selected1 {Answer}");
        Parent.OnAnswerClicked(this); 
    }
}
