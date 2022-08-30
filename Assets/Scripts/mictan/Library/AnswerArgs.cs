using UnityEngine;

namespace ClassLibrary1
{
    public interface AnswerArgs
    {
        /// <summary>
        /// Виден ли ответ для пользователя
        /// </summary>
        /// <returns></returns>
        bool IsVisible();
        
        /// <summary>
        /// "Bla lba {MONEY}g? {Nickname}" -> "Bla lba 100g. Slime"
        /// </summary>
        /// <returns></returns>
        string GenerateString();

        /// <summary>
        /// Вызывается когда игрок выбирает этот ответ
        /// </summary>
        void OnSelected();
    }
    
    public class MockAnswerArgs : AnswerArgs
    {
        private bool visible;
        private string text;

        public MockAnswerArgs(bool visible, string text)
        {
            this.visible = visible;
            this.text = text;
        }

        public bool IsVisible()
        {
            return visible;
        }

        public string GenerateString()
        {
            return text;
        }

        public void OnSelected()
        {
            Debug.Log($"MockAnswerArgs:OnSelected:{visible}:{text}");
        }
    }
}