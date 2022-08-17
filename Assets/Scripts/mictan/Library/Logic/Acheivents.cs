using Assets.Scripts.Slime.Sugar;
using System.Collections.Generic;

namespace ClassLibrary1.Logic
{
    /// <summary>
    /// Открывает те или иные диалоги \ квесты. Хранит в себе значения, на которые можно подписаться.
    /// Тут находятся все переменные которые могут тригерить выдачу квеста.
    /// </summary>
    public class Acheivents
    {
        public Dictionary<string, Subscribable<double>> DoubleValues = new Dictionary<string, Subscribable<double>>();

        public void RegisterDouble(string name, double value)
        {
            var subscribable = new Subscribable<double>();
            subscribable.Set(value);
            DoubleValues.AddOnce(name, subscribable);
        }

        public static Acheivents Instance;


    }
}