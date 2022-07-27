using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ROR.Core
{
    public class FloatingText : MonoBehaviour
    {
        private Vector3 m_originalPosition;
        private Vector3 m_randomDirection;
        public Vector3 Move;

        void OnDidApplyAnimationProperties()
        {
            gameObject.transform.position = new Vector3(
                m_originalPosition.x + m_randomDirection.x * Move.x, 
                m_originalPosition.y + m_randomDirection.y * Move.y, 
                m_originalPosition.z + m_randomDirection.z * Move.z);
        }

        public static FloatingText Create(Vector3 position, string text, float textSize = 4)
        {
            return Create(position, text, Color.red, textSize);
        }

        public static FloatingText CreateStaticText(Vector3 position, string text, Color textColor, float textSize = 14)
        {
            var damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, position, Quaternion.identity);
            var damagePopup = damagePopupTransform.GetComponent<FloatingText>();
            damagePopup.transform.position = position;
            
            var textMeshPro = damagePopup.GetComponentInChildren<TextMeshPro>();
            textMeshPro.color = textColor;
            textMeshPro.SetText(text);
            textMeshPro.fontSize = textSize;
            
            return damagePopup;
        }
        public static FloatingText Create(Vector3 position, string text, Color textColor, float textSize = 14)
        {
            var damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, position, Quaternion.identity);

            var damagePopup = damagePopupTransform.GetComponent<FloatingText>();
            damagePopup.m_originalPosition = position;
            damagePopup.m_randomDirection = Random.insideUnitSphere;
            damagePopup.m_randomDirection.Normalize();
            damagePopup.transform.position = position;
                
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