using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SecondCycleGame
{
    public class SubGroup
    {
        private List<Character> _characters;

        public SubGroup()
        {
            _characters = new List<Character>();
        }

        public void Add(Character character)
        {
            _characters.Add(character);
        }
        public void Remove(Character character)
        {
            _characters.Remove(character);
            if(_characters.Count == 1)
            {

            }
        }
        public void Follow(Character character, Vector3 position)
        {

        }
    }
}
