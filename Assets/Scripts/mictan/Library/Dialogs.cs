using System;
using System.Collections.Generic;
using ClassLibrary1.Logic;

namespace ClassLibrary1
{
    public class Dialogs
    {
        public static Dictionary<string, Dialog> VisibleDialogs = new Dictionary<string, Dialog>();
        public static Dictionary<string, Dialog> InvisibleDialogs = new Dictionary<string, Dialog>();

        public static Dictionary<string, Answer> Answers = new Dictionary<string, Answer>();
        public static Dictionary<string, Question> Questions = new Dictionary<string, Question>();

        public static event Action<Dialog> OnVisibilityChanged;

        public static void UpdateState(Dialog dialog){
            if(dialog.IsVisible){
                VisibleDialogs[dialog.Xml.Id] = dialog;
                InvisibleDialogs.Remove(dialog.Xml.Id);
            } else {
                InvisibleDialogs[dialog.Xml.Id] = dialog;
                VisibleDialogs.Remove(dialog.Xml.Id);
            }

            OnVisibilityChanged.Invoke(dialog);
        }
    }
}