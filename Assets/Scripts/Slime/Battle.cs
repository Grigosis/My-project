using System.Collections;
using System.Collections.Generic;
using ROR.Core;
using UnityEngine;

namespace SecondCycleGame
{
    public class Battle : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void NextTurn()
        {
            FloatingText.Create(Vector3.zero, "Hello World", Color.red, 30f);
        }
    }
}
