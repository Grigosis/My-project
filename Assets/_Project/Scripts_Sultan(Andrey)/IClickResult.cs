using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SecondCycleGame
{
    public interface IClickResult
    {
        public bool OnUIClick { get; set; }

        public bool IsObjectClicked { get; set; }

        public bool UIClicked();

        public void ObjectClick(InputAction.CallbackContext cntx);

        
    }
}
