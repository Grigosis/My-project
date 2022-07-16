using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace SecondCycleGame
{
    /// <summary>
    /// Easy but slow
    /// </summary>
    public class AttrText : MonoBehaviour
    {
        private static Regex Regex = new Regex("{([^}]+)}");
        private TextMeshPro Text;
        public string AttributeName = "{HP_NOW}";
        public bool ShowName = false;
        // Start is called before the first frame update
        void Start()
        {
            Text = gameObject.GetComponent<TextMeshPro>();
            
        }

        // Update is called once per frame
        void Update()
        {
            var provider = GetComponentInParent<IAttributeProvider>();
            if (provider == null)
            {
                Text.text = "NoProvider";
            }

            var attrs = provider.GetAttributes();
            if (attrs == null)
            {
                Text.text = "Attrs null";
            }

            var txt = Regex.Replace(AttributeName, (match) =>
            {
                try
                {
                    var byName = attrs.GetByName(match.Groups[1].Value);
                    var txt = ShowName ? (match.Groups[1].Value + ":") : "";
                    return $"{txt}{byName:F2}";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            });
            Text.text = txt;
        }
    }
}
