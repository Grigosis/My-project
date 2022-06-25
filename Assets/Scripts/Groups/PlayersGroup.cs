using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SecondCycleGame
{
    public sealed class PlayersGroup
    {
        private readonly List<Character> team;
        public const byte MAX_TEAM_SIZE = 4;
        public readonly Transform uiTransform;

        public readonly Character MainCharacter;
        public Character SelectedCharacter { get; private set; }

        public PlayersGroup(Transform UITransform)
        {
            team = new List<Character>();
            uiTransform = UITransform;

            var data = Resources.Load<CharacterData>("Data/CharacterData");

            MainCharacter = new Character(data);
            team.Add(MainCharacter);
            //MainCharacter.AddToPlayersGroup(this);

            MainCharacter.behaviour.transform.position = new Vector3(-3, 0, -8);
            SelectedCharacter = MainCharacter;


            ControlsSubscribe();
        }

        private void ControlsSubscribe()
        {
            Controls.OnGroundClick += MoveSelected;

        }
        public bool TryAddToTeam(Character character)
        {
            if (team.Count >= MAX_TEAM_SIZE)
            {
                Debug.LogWarning("max team size: 4");
                return false;
            }
            if (team.Contains(character))
            {
                Debug.LogWarning("same character cant be added");
                return false;
            }

            var subgroup = MainCharacter.GroupMember.SubGroup;
            if (subgroup == null)
            {
                subgroup = new SubGroup();
                MainCharacter.GroupMember.JoinSubGroup(subgroup);
            }
            character.AddToPlayersGroup(this);
            character.GroupMember.JoinSubGroup(subgroup);

            Sort();
            return true;
        }
        private void Sort()
        {
            var queue = new List<Character>(team);
            while (queue.Count > 0)
            {
                var character = queue[0];
                queue.RemoveAt(0);
                if(character.GroupMember.SubGroup == null)
                {
                    //character set position

                    //add double offset
                }
                else
                {
                    foreach (var item in queue)
                    {
                        if(item.GroupMember.SubGroup == character.GroupMember.SubGroup)
                        {
                            //item set position
                            queue.Remove(item);
                        }
                    }
                    //add double offset
                }
            }
        }
        private void MoveSelected(Vector3 position)
        {
            SelectedCharacter.behaviour.MoveToPosition(position);
        }
    }
}
