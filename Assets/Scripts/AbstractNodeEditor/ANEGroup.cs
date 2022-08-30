using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Assets.Scripts.AbstractNodeEditor
{
    public class ANEGroup : Group
    {
        public int ID;
        public ANEGroup(string groupTitle, int id, Vector2 position)
        {
            ID = id;
            title = groupTitle;
            SetPosition(new Rect(position, Vector2.zero));
        }

        protected override void OnElementsAdded(IEnumerable<GraphElement> elements)
        {
            base.OnElementsAdded(elements);

            foreach (var element in elements)
            {
                if (element is ANENode node)
                {
                    node.Group = ID;
                }
            }
        }
    }
}