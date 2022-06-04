using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SecondCycleGame
{
    public sealed class PlayersGroup
    {
        private readonly List<Character> team;
        public const byte MAX_TEAM_SIZE = 4;

        private Character MainCharacter => team[0];
        public Character SelectedCharacter { get; private set; }

        public PlayersGroup()
        {
            team = new List<Character>();

            var data = Resources.Load<CharacterData>("Data/CharacterData");
            team.Add(new Character(this, data));

            MainCharacter.model.transform.position = new Vector3(-3, 0, -8);
            SelectedCharacter = MainCharacter;
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

            var subgroup = MainCharacter.SubGroup;
            if (subgroup == null)
            {
                subgroup = new SubGroup();
                MainCharacter.JoinSubGroup(subgroup);
            }
            character.JoinSubGroup(subgroup);

            //create interactableportrait
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
                if(character.SubGroup == null)
                {
                    //character set position

                    //add double offset
                }
                else
                {
                    foreach (var item in queue)
                    {
                        if(item.SubGroup == character.SubGroup)
                        {
                            //item set position
                            queue.Remove(item);
                        }
                    }
                    //add double offset
                }
            }
        }
    }
}
