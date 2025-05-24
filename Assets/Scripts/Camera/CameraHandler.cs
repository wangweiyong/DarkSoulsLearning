using System.Collections;
using System.Collections.Generic;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.UIElements;

namespace wwy
{
    public class CameraHandler : MonoBehaviour
    {
        InputHandler inputHandler;
        public Transform targetTransform;// the transform the camera follows (the player)
        public Transform targetTransformWhileAiming;// the transform the camera follows while aiming
        public Transform cameraTransform;// camera当前位置
        public Transform cameraPivotTransform;// camera会围绕pivot旋转
        private Vector3 cameraTransformPosition;
        public LayerMask ignoreLayers;
        private Vector3 cameraFollowVelocity = Vector3.zero;
        private LayerMask environmentLayer;
        PlayerManager playerManager;
        public Camera cameraObject;

        public static CameraHandler singleton;

        public float lookSpeed = 0.1f;
        public float leftAndRightAimingLookSpeed = 5f;
        public float originalSpeed;
        public float speedUpMultiplier = 1.5f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;
        public float upAndDownAimingLookSpeed = 5f;

        private float defaultPosition;
        private float leftAndRightAngle;
        private float upAndDownAngle;
        public float minimumPivot = -35;
        public float maximumPivot = 35;

        public float cameraSphereRadius = 0.8f;
        public float cameraCollisionOffset = 0.8f;
        public float minimumCollisionOffset = 0.2f;
        public float offersetSpeed = 60f;
        public float lockedPivotPosition = 2.25f;
        public float unlockedPivtoPosition = 1.65f;

        public float maxmumLockOnDistance = 30;
        List<CharacterManager> availableTargets = new List<CharacterManager>();
        public CharacterManager  currentLockOnTarget;
        public CharacterManager nearestLockOnTarget;
        public CharacterManager leftLockOnTarget;
        public CharacterManager rightLockOnTarget;
        private void Awake()
        {
            singleton = this;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10 |1 << 12 | 1<<13);
            environmentLayer = LayerMask.NameToLayer("Environment");
            originalSpeed = lookSpeed;
            targetTransform = FindObjectOfType<PlayerManager>().transform;
            inputHandler = FindObjectOfType<InputHandler>();
            playerManager = FindObjectOfType<PlayerManager>();
            cameraObject = GetComponentInChildren<Camera>();
        }

        //follow the player
        public void FollowTarget(float delta)
        {
            if (playerManager.isAiming)
            {
                Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransformWhileAiming.position, ref cameraFollowVelocity, delta * followSpeed);
                transform.position = targetPosition;
            }
            else
            {
                Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, delta * followSpeed);
                transform.position = targetPosition;
            }
            HandleCameraCollisions(Time.deltaTime);
        }

        Vector3 currentRotation;
        Vector3 rotationSmoothVelocity;
        //rotate the camera
        public void HandleCameraRotation()
        {
            if(inputHandler.lockOnFlag == true && currentLockOnTarget != null)
            {
                HandleLockedCameraRotation();
            }
            else if (playerManager.isAiming)
            {
                HandleAimedCameraRotation();
            }
            else
            {
                HandleStandardCameraRotation();
            }
        }
        public void HandleStandardCameraRotation()
        {
                leftAndRightAngle += (inputHandler.mouseX * lookSpeed);
                upAndDownAngle -= (inputHandler.mouseY * pivotSpeed);
                upAndDownAngle = Mathf.Clamp(upAndDownAngle, minimumPivot, maximumPivot);

                /* Vector3 rotation = Vector3.zero;
                 rotation.y = lookAngle;
                 Quaternion targetRotation = Quaternion.Euler(rotation);
                 myTransform.rotation = targetRotation;

                 rotation = Vector3.zero;
                 rotation.x = pivotAngle;
                 targetRotation = Quaternion.Euler(rotation);
                 cameraPivotTransform.localRotation = targetRotation;*/
                currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(upAndDownAngle, leftAndRightAngle), ref rotationSmoothVelocity, 0.2f);
                transform.eulerAngles = currentRotation;
        }
        private void HandleLockedCameraRotation()
        {
            if(currentLockOnTarget != null)
            {
                Vector3 dir = currentLockOnTarget.transform.position - transform.position;

                dir.Normalize();
                dir.y = 0;

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = targetRotation;

                dir = currentLockOnTarget.transform.position - cameraPivotTransform.position;
                dir.Normalize();
                targetRotation = Quaternion.LookRotation(dir);
                Vector3 eulerAngles = targetRotation.eulerAngles;
                eulerAngles.y = 0;
                cameraPivotTransform.localEulerAngles = eulerAngles;
            }
        }

        private void HandleAimedCameraRotation()
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            cameraPivotTransform.rotation = Quaternion.Euler(0, 0, 0);
            Quaternion targetRotationX;
            Quaternion targetRotationY;
            Vector3 cameraRotationX = Vector3.zero;
            Vector3 cameraRotationY = Vector3.zero;

            leftAndRightAngle += (inputHandler.mouseX * leftAndRightAimingLookSpeed) * Time.deltaTime;
            upAndDownAngle -= (inputHandler.mouseY * upAndDownAimingLookSpeed) * Time.deltaTime;

            cameraRotationY.y = leftAndRightAngle;
            targetRotationY = Quaternion.Euler(cameraRotationY);
            targetRotationY = Quaternion.Slerp(transform.rotation, targetRotationY, 1);
            transform.rotation = targetRotationY;

            cameraRotationX.x = upAndDownAngle;
            targetRotationX = Quaternion.Euler(cameraRotationX);
            targetRotationX = Quaternion.Slerp(cameraTransform.localRotation, targetRotationX, 1);
            cameraTransform.localRotation = targetRotationX;
        }
        private void HandleCameraCollisions(float delta)
        {
            float targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();
            if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition),ignoreLayers))
            {
                Debug.DrawLine(cameraPivotTransform.position,hit.point,Color.red, 0.1f);
                lookSpeed = originalSpeed * speedUpMultiplier;
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOffset);
            }
            else
            {
                lookSpeed = originalSpeed;
            }

            if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
            {
                targetPosition = -minimumCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, offersetSpeed * delta);
            cameraTransform.localPosition = cameraTransformPosition;
        }
    
        public void HandleLockOn()
        {
            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = -Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;
            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 26);

            for(int i = 0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].GetComponent<CharacterManager>(); 
            
                if(character != null)
                {
                    Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                    float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                    float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);

                    RaycastHit hit;
                    if(character.transform.root != targetTransform.transform.root && viewableAngle < 50 && viewableAngle > -50 && distanceFromTarget <= maxmumLockOnDistance)
                    {
                        if(Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                        {
                            Debug.DrawLine(playerManager.lockOnTransform.position, character.lockOnTransform.position);
                        
                            if(hit.transform.gameObject.layer == environmentLayer)
                            {
                                //cannot lock on
                            }
                            else
                            {
                                availableTargets.Add(character);

                            }
                        }
                    }
                        
                }
            }
            for(int k = 0; k < availableTargets.Count; ++k)
            {
                float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[k].transform.position);
                if(distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[k];
                }

                if (inputHandler.lockOnFlag)
                {
                    //Vector3 relativeEnemyPosition = currentLockOnTarget.transform.InverseTransformPoint(availableTargets[k].transform.position);
                    //var distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTargets[k].transform.position.x;
                    //var distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTargets[k].transform.position.x;
                    Vector3 relativeEnemyPosition = inputHandler.transform.InverseTransformPoint(availableTargets[k].transform.position);
                    var distanceFromLeftTarget = relativeEnemyPosition.x;
                    var distanceFromRightTarget = relativeEnemyPosition.x;
                    if (relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget &&
                        availableTargets[k] != currentLockOnTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                        leftLockOnTarget = availableTargets[k];
                    }
                    else if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget &&
                        availableTargets[k] != currentLockOnTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget;
                        rightLockOnTarget = availableTargets[k];
                    }
                }
            }
        }
    
        public void ClearLockOnTargets()
        {
            availableTargets.Clear();
            nearestLockOnTarget = null;
            currentLockOnTarget = null;
        }
    
        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
            Vector3 newunLockedPosition = new Vector3(0, unlockedPivtoPosition);

            if(currentLockOnTarget != null)
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
            }
            else
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newunLockedPosition, ref velocity, Time.deltaTime);

            }
        }
    }

}