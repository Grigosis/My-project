using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
}