using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SecondCycleGame
{
    public class SubGroup
    {
        private readonly List<Character> _characters;
        public Action<Character> OnAutoClear;

        public int Count => _characters.Count;

        public SubGroup()
        {
            _characters = new List<Character>();
        }

        public void Add(Character character)
        {
            if (!_characters.Contains(character))
                _characters.Add(character);
        }
        public void Remove(Character character)
        {
            _characters.Remove(character);
            if(_characters.Count == 1)
            {
                //OnAutoClear?.Invoke(_characters[0]);
                _characters[0].LeaveSubGroup();
            }
        }
        public void Clear()
        {
            foreach (var character in _characters)
            {
                OnAutoClear?.Invoke(character);
            }
            _characters.Clear();
        }
        public void Follow(Character character, Vector3 position)
        {

        }
    }
}
