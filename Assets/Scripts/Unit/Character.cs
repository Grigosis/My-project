using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SecondCycleGame
{
    public class Character : Unit<CharacterData>
    {
        public GroupMember GroupMember { get; private set; }

        public Character(CharacterData characterData) : base(characterData)
        {
            model.tag = "Player";
            model.gameObject.layer = LayerMask.NameToLayer("PlayerUnit");
        }

        public void AddToPlayersGroup(PlayersGroup group)
        {
            if (GroupMember == null)
                GroupMember = new GroupMember(this, group);
        }
    }
}
