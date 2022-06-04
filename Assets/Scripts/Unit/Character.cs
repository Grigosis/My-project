using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SecondCycleGame
{
    public class Character : Unit<CharacterData>
    {
        public readonly PlayersGroup playersGroup;
        public InteractablePortrait GroupPortrait { get; private set; }

        public Character(PlayersGroup playersGroup, CharacterData characterData) : base(characterData)
        {
            this.playersGroup = playersGroup;
            model.tag = "Player";
            model.gameObject.layer = LayerMask.NameToLayer("PlayerUnit");
        }

        public SubGroup SubGroup { get; private set; }

        public void InstantiateGroupPortrait()
        {
            GroupPortrait = Object.Instantiate(data.PortraitPrefab).GetComponent<InteractablePortrait>();
        }
        public void LeaveSubGroup()
        {
            SubGroup.Remove(this);
            SubGroup = null;
        }
        public void JoinSubGroup(SubGroup subGroup)
        {
            subGroup.Add(this);
            SubGroup = subGroup;
        }
    }
}
