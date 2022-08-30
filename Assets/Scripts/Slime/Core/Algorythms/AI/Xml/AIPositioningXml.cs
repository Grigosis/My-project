using System;
using UnityEngine;

namespace Assets.Scripts.Slime.Core.Algorythms
{
    [Serializable]
    public struct AIPositioningXml
    {
        [SerializeField]
        public AIPositioningLayerXml[] Layers;
    }
}