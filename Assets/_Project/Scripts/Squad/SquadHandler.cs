using System.Collections.Generic;
using UnityEngine;

namespace SecondCycleGame
{
    public class SquadHandler : Singleton<SquadHandler>
    {

        private List<UnitHandler> _squad = new List<UnitHandler>();

        public IReadOnlyList<UnitHandler> Squad => _squad;

        public IReadOnlyCollection<UnitHandler> AllUnits => FindObjectsOfType<UnitHandler>();

        public void AddToSquad(UnitHandler unit)
        {
            if(_squad.Contains(unit))
                return;

            _squad.Add(unit);
        }
        public void ClearSquad()
        {
            _squad.Clear();
        }

        //private void OnGUI()
        //{
        //    if (Input.GetMouseButtonUp(0))
        //    {
        //        Rect selectionRect = Utils.GetScreenRect(_startInputposition, Input.mousePosition);
        //        ClearSquad();

        //        foreach (UnitHandler unit in FindObjectsOfType<UnitHandler>())
        //        {
        //            if (Utils.IsWithinSelectionBounds(unit.gameObject, selectionRect))
        //            {
        //                AddToSquad(unit);
        //            }
        //        }
        //    }

        //    if(Squad.Count > 0)
        //    { 
        //        foreach(UnitHandler unit in _squad)
        //        {
        //            //Utils.DrawScreenSpaceRect(rect, ); на подумать
        //        }
        //    }
        //}
    }
}
