using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace MyDoom.ShootingSystem
{
    public class AutoAimSystem : MonoBehaviour, IAutoAimSystem
    {
        private static AutoAimSystem _instance;
        public static AutoAimSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.Log("No AutoAimSystem");
                }

                return _instance;
            }
        }

        private void OnEnable()
        {
            _instance = this;
        }
//(Transform origin, GunData gunData)
        [CanBeNull]
        public GameObject ChooseEnemyWithAutoAim(AutoAimContext context)
        {
            List<KeyValuePair<float, GameObject>> enemiesInDirection = new();
            
            Transform origin = context.Origin;
            GunData gunData = context.GunData;
            int rayCount = context.RayCount;
            float fieldOfView = context.FieldOfView;
            float angleStep = fieldOfView / (rayCount - 1);

            for (int i = 0; i <= rayCount; i++)
            {
                float angle = -fieldOfView / 2 + i * angleStep;
                Vector3 direction = origin.rotation * Quaternion.Euler(angle, 0, 0) * Vector3.forward;
                direction.Normalize();

                Debug.DrawRay(origin.position, direction * gunData.lengthRange, Color.yellow, 0.1f);

                if (Physics.Raycast(origin.position, direction, out RaycastHit hitInfo,
                        gunData.lengthRange))
                {
                    if (hitInfo.collider.CompareTag("Enemy"))
                    {
                        enemiesInDirection.Add(new KeyValuePair<float,
                            GameObject>(hitInfo.distance,
                            hitInfo.transform.gameObject));
                    }
                }
            }

            GameObject closest = null;
            float smallestDistance = float.MaxValue;

            foreach (var obj in enemiesInDirection)
            {
                if (obj.Key < smallestDistance)
                {
                    smallestDistance = obj.Key;
                    closest = obj.Value;
                }
            }

            return closest;
        }
    }
}