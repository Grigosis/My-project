using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SecondCycleGame
{
    public class GroupMember
    {
        public readonly Character character;
        public readonly PlayersGroup playersGroup;
        public readonly InteractablePortrait groupPortrait;
        public SubGroup SubGroup { get; private set; }

        public GroupMember(Character member, PlayersGroup group)
        {
            character = member;
            playersGroup = group;
            groupPortrait = Object.Instantiate(character.data.PortraitPrefab, group.uiTransform).GetComponent<InteractablePortrait>();
            groupPortrait.Initialize(character);
        }

        public void LeaveSubGroup()
        {
            SubGroup.Remove(character);
            SubGroup = null;
        }
        public void JoinSubGroup(SubGroup subGroup)
        {
            subGroup.Add(character);
            SubGroup = subGroup;
        }
    }
}
