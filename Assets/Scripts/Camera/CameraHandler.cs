using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace wwy
{
    public class CameraHandler : MonoBehaviour
    {
        public Transform targetTransform;// camera最终会去到的位置
        public Transform cameraTransform;// camera当前位置
        public Transform cameraPivotTransform;// camera会围绕pivot旋转
        private Transform myTransform;// 当前对象的位置
        private Vector3 cameraTransformPosition;
        private LayerMask ignoreLayers;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        public static CameraHandler singleton;

        public float lookSpeed = 0.1f;
        public float originalSpeed;
        public float speedUpMultiplier = 1.5f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;

        private float defaultPosition;
        private float lookAngle;
        private float pivotAngle;
        public float minimumPivot = -35;
        public float maximumPivot = 35;

        public float cameraSphereRadius = 0.8f;
        public float cameraCollisionOffset = 0.8f;
        public float minimumCollisionOffset = 0.2f;
        public float offersetSpeed = 60f;

        private void Awake()
        {
            singleton = this;
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
            originalSpeed = lookSpeed;
            targetTransform = FindObjectOfType<PlayerManager>().transform;
        }

        public void FollowTarget(float delta)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta * followSpeed);
            myTransform.position = targetPosition;
            HandleCameraCollisions(Time.deltaTime);

        }

        Vector3 currentRotation;
        Vector3 rotationSmoothVelocity;
        public void HandleCameraRotation(float delta, float mouseXInut, float mouseYInput)
        {
            lookAngle += (mouseXInut * lookSpeed) ;
            pivotAngle -= (mouseYInput * pivotSpeed) ;
            pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

            /* Vector3 rotation = Vector3.zero;
             rotation.y = lookAngle;
             Quaternion targetRotation = Quaternion.Euler(rotation);
             myTransform.rotation = targetRotation;

             rotation = Vector3.zero;
             rotation.x = pivotAngle;
             targetRotation = Quaternion.Euler(rotation);
             cameraPivotTransform.localRotation = targetRotation;*/
            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pivotAngle, lookAngle), ref rotationSmoothVelocity, 0.2f);
            myTransform.eulerAngles = currentRotation;
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
    }

}