using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ROR.Core
{
    public class FloatingText : MonoBehaviour
    {
        private Vector2 m_originalPosition;
        private Vector2 m_randomDirection;
        public Vector2 Move;

        void OnDidApplyAnimationProperties()
        {
            gameObject.transform.position = new Vector3(
                m_originalPosition.x + m_randomDirection.x * Move.x, 
                m_originalPosition.y + m_randomDirection.y * Move.y, 0);
        }

        public static FloatingText Create(Vector3 position, string text, float textSize = 4)
        {
            return Create(position, text, Color.red, textSize);
        }
        
        public static FloatingText Create(Vector3 position, string text, Color textColor, float textSize = 4)
        {
            var damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, position, Quaternion.identity);

            var damagePopup = damagePopupTransform.GetComponent<FloatingText>();
            damagePopup.m_originalPosition = position;
            damagePopup.m_randomDirection = Random.insideUnitCircle;
            damagePopup.m_randomDirection.Normalize();
            
            var textMeshPro = damagePopup.GetComponentInChildren<TextMeshPro>();
            
            textMeshPro.color = textColor;
            textMeshPro.SetText(text);
            textMeshPro.fontSize = textSize;
            
            var animator = damagePopup.gameObject.GetComponent<Animator>();
            animator.Play("Fade");
            
            return damagePopup;
        }
    }
}