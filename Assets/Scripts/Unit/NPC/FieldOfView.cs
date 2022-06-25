using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SecondCycleGame
{
    public class FieldOfView : MonoBehaviour
    {
        [Range(1, 100)] public int viewRadius = 20;
        [Range(0, 180)] public int viewAngle = 90;
        [Range(1, 50)] public int hearingRadius = 12;
        [Range(0, 5)] public float suspectDuration = 2;
        private float _suspectingTime;
        public LayerMask targetMask;
        public SpriteRenderer indicator;

        public float dotProd;

        public bool canSeePlayer;
        public bool canHearPlayer;
        public Transform Target { get; private set; }

        private void Start()
        {
            StartCoroutine(FOVRoutine());
            UpdateIndicator();
            indicator.enabled = false;
        }
        private void LateUpdate()
        {
            if(canSeePlayer) UpdateIndicator();
        }

        private IEnumerator FOVRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(0.5f);

            while (true)
            {
                yield return wait;
                FieldOfViewCheck();
            }
        }

        private void FieldOfViewCheck()
        {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

            if (rangeChecks.Length != 0)
            {
                foreach (var target in rangeChecks)
                {
                    Vector3 directionToTarget = target.transform.position - transform.position;
                    dotProd = Vector3.Dot(transform.forward, directionToTarget.normalized);

                    if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
                    {
                        if (Physics.Raycast(transform.position + Vector3.up, directionToTarget, out RaycastHit hit, viewRadius))
                        {
                            if (hit.transform.CompareTag("Player"))
                            {
                                Target = hit.transform;
                                //canSeePlayer = true;
                                SetSuspection(true);
                                return;
                            }
                        }
                    }
                }
            }
            //canSeePlayer = false;
            SetSuspection(false);
        }
        private void UpdateIndicator()
        {
            indicator.transform.position = transform.position + Vector3.up * 2.2f;
            if(_suspectingTime > suspectDuration)
            {
                indicator.color = Color.red;
                return;
            }
            _suspectingTime += Time.deltaTime;
        }
        
        private void SetSuspection(bool value)
        {
            if (canSeePlayer == !value)
            {
                canSeePlayer = value;
                indicator.enabled = value;
                if (value)
                {
                    _suspectingTime = 0;
                    indicator.color = Color.yellow;
                }
            }
        }
    }
}
