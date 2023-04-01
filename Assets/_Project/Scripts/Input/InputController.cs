using UnityEngine;

namespace SecondCycleGame
{
    public class InputController : Singleton<InputController>
    {
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private bool _isSelecting;
        public bool IsSelecting => _isSelecting;

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                _isSelecting = true;
                _startPosition = Input.mousePosition;
            }
            else if(Input.GetMouseButtonUp(0))
            {
                _isSelecting = false;
                SquadHandler.Instance.ClearSquad();

                foreach (var unit in SquadHandler.Instance.AllUnits)
                {
                    if(Utils.IsWithinSelectionBounds(unit.gameObject, SelectionSqare.Instance.SelectionRect))
                    {
                        SquadHandler.Instance.AddToSquad(unit);
                    }
                }
            }
            else if(Input.GetMouseButton(0))
            {
                _endPosition = Input.mousePosition;
                SelectionSqare.Instance.UpdateSelectionRect(_startPosition, _endPosition);
            }
        }
    }
}
