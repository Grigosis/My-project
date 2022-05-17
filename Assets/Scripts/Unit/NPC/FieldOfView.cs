using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SecondCycleGame
{
    public class FieldOfView : MonoBehaviour
    {
        [Range(1, 100)] public int radius;
        [Range(0, 180)] public int angle;
        public LayerMask targetMask;
        public Transform indicator;

        public bool canSeePlayer;
        public bool canHearPlayer;
        public Transform Target { get; private set; }

        private void Start()
        {
            StartCoroutine(FOVRoutine());
            UpdateIndicator();
        }
        private void LateUpdate()
        {
            UpdateIndicator();
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
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

            if (rangeChecks.Length != 0)
            {
                foreach (var target in rangeChecks)
                {
                    Vector3 directionToTarget = target.transform.position - transform.position;

                    if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                    {
                        if (Physics.Raycast(transform.position + Vector3.up, directionToTarget, out RaycastHit hit, radius))
                        {
                            if (hit.transform.CompareTag("Player"))
                            {
                                Target = hit.transform;
                                canSeePlayer = true;
                                return;
                            }
                        }
                    }
                }
            }
            canSeePlayer = false;
        }
        private void UpdateIndicator()
        {
            indicator.position = transform.position + Vector3.up * 2.2f;
        }
    }
}
