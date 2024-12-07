using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace wwy
{
    public class PlayerLocomotion : MonoBehaviour
    {
        Transform cameraObject;
        InputHandler inputHandler;
        Vector3 moveDiretion;

        [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public AnimatorHandler animatorHandler;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Stats")]
        [SerializeField]
        float movementSpeed = 5;
        [SerializeField]
        float rotationSpeed = 10;

        // Start is called before the first frame update
        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();  
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animatorHandler.Initialze();
        }

        public void Update()
        {
            float delta = Time.deltaTime;
            
            inputHandler.TickInput(delta);

            moveDiretion = cameraObject.forward * inputHandler.vertical;
            moveDiretion += cameraObject.right * inputHandler.horizontal;
            moveDiretion.Normalize();

            float speed = movementSpeed;
            moveDiretion *= speed;

            Vector3 projectVeclocity = Vector3.ProjectOnPlane(moveDiretion, normalVector);
            //moveDiretion.y = 0;
            rigidbody.velocity = projectVeclocity;

            animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0);
            if (animatorHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }

        #region Movement

        Vector3 normalVector = new Vector3(0,1,0);
        Vector3 targetPosition;

        private void HandleRotation(float delta)
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = inputHandler.moveAmount;

            targetDir = cameraObject.forward * inputHandler.vertical;
            targetDir += cameraObject.right * inputHandler.horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if(targetDir == Vector3.zero)
            {
                targetDir = myTransform.forward;
            }

            float rs = rotationSpeed;
            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * rotationSpeed * delta);
            myTransform.rotation = targetRotation;
        }
        #endregion
    }
}
