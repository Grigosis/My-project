using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SecondCycleGame
{
    public sealed class PlayersGroup
    {
        public readonly List<Character> team;
        public const byte MAX_TEAM_SIZE = 4;
        public readonly List<Character> group1;
        public readonly List<Character> group2;

        public PlayersGroup(List<Character> characters)
        {
            //team = new Character[MAX_TEAM_SIZE];
            team = characters;
            group1 = new List<Character>();
            group2 = new List<Character>();

            foreach (var character in team)
            {
                group1.Add(character);
                //character.groupIndex = 1;
            }
        }

        //public void Place(Character character, List<Character> newGroup, int index)
        //{
        //    newGroup.Insert(index, character);
        //    character.group = newGroup;
        //}
        //public void Remove(Character character)
        //{
        //    character.group.Remove(character);
        //    character.group = null;
        //}

        private void Sort()
        {
            var queue = new List<Character>(team);
            while (queue.Count > 0)
            {
                var character = queue[0];
                queue.RemoveAt(0);
                if(character.group == null)
                {
                    //character set position

                    //add double offset
                }
                else
                {
                    foreach (var item in queue)
                    {
                        if(item.group == character.group)
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
