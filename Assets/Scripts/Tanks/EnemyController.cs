﻿using UnityEngine;
using Tank;

namespace Enemy
{
    public class EnemyController : TankController
    {
        private TankController target;
        [SerializeField]
        private float visionConeAngle = 45f;

        [SerializeField]
        private float shootRange = 10f;

        [SerializeField]
        private float movementDistance = 15f;
        private Vector3 moveDir;
        private Vector3 distanceCheckPos;

        private void Update()
        {
            if (CheckIfTargetIsVisible())
            {
                turret.transform.LookAt(target.transform.position);
                if (!bulletCooldownFlag)
                {
                    ShootBullet();
                }
            }
            else
            {
                KeepPatrolling();
            }
        }

        private bool CheckIfTargetIsVisible()
        {
            if (target != null)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < shootRange)
                {
                    float dotProduct = Vector3.Dot((target.transform.position - transform.position).normalized, turret.transform.forward);
                    float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
                    return angle < visionConeAngle;
                }
            }
            return false;
        }

        private void KeepPatrolling()
        {
            if (Vector3.Distance(transform.position, distanceCheckPos) < movementDistance)
            {
                transform.Translate(moveDir * 5f * Time.deltaTime);
            }
            else
            {
                distanceCheckPos = transform.position;
                moveDir = -moveDir;
            }
        }

        public void SetupEnemy(TankController enemyTarget)
        {
            target = enemyTarget;
            Debug.Log(transform.eulerAngles.y);
            switch (transform.eulerAngles.y)
            {
                case 270f:
                    moveDir = new Vector3(0, 0, -1);
                    break;
                case 180f:
                    moveDir = new Vector3(0, 0, 1);
                    break;
                case 0f:
                    moveDir = new Vector3(0, 0, -1);
                    break;
                case 90f:
                    moveDir = new Vector3(0, 0, 1);
                    break;
                default:
                    moveDir = Vector3.zero;
                    break;
            }
            distanceCheckPos = transform.position;
        }
    }
}