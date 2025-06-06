using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace wwy
{
    public class PlayerLocomotionManager : MonoBehaviour
    {
        CameraHandler cameraHandler;
        PlayerManager playerManager;
        PlayerStatsManager playerStatsManager;
        Transform cameraObject;
        InputHandler inputHandler;
        public Vector3 moveDirection;

        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlockerCollider;

        [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public PlayerAnimatorManager playerAnimationManager;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Ground & Air Detection Stats")]
        [SerializeField]
        float groundDetectionRayStartPoint = 0.5f;
        [SerializeField]
        float minimumDistanceNeededToBeginFall = 1f;
        [SerializeField]
        float groundDirectionRayDistance = 0.2f;
        LayerMask ignoreForGroundCheck;
        public float inAirTimer;

        [Header("Movement Stats")]
        [SerializeField]
        float movementSpeed = 5;
        [SerializeField]
        float walkSpeed = 1;
        [SerializeField]
        float sprintSpeed = 7;
        [SerializeField]
        float rotationSpeed = 10;
        [SerializeField]
        float fallingSpeed = 45;

        [Header("Roll Costs")]
        [SerializeField]
        int rollStaminaCost = 15;
        int backstepStaminaCost = 12;
        int sprintStaminaCost = 1;

        Vector3 dampMoveCurrentSpeed;

        // Start is called before the first frame update
        void Start()
        {
            playerManager = GetComponent<PlayerManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            playerAnimationManager = GetComponent<PlayerAnimatorManager>();  
            cameraObject = Camera.main.transform;
            myTransform = transform;
            cameraHandler = FindObjectOfType<CameraHandler>();

            playerManager.isGrounded = true;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
            Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider);
        }
        public void HandleJumping()
        {
            if (playerManager.isInteracting)
            {
                return;
            }
            if (playerStatsManager.currentStamina <= 0)
            {
                return;
            }
            if (inputHandler.jump_Input)
            {
                if(inputHandler.moveAmount > 0)
                {
                    moveDirection = cameraObject.forward * inputHandler.vertical;
                    moveDirection += cameraObject.right * inputHandler.horizontal;
                    playerAnimationManager.PlayTargetAnimation("Jump", true);
                    playerAnimationManager.animator.SetBool("isJumping", true);

                    //playerAnimationManager.EraseHandIKForWeapon();
                    moveDirection.y = 0;

                    Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = jumpRotation;
                }
            }
        }

        #region Movement

        Vector3 normalVector = new Vector3(0,1,0);
        Vector3 targetPosition;

        public void HandleRotation()
        {
            if (playerAnimationManager.canRotate)
            {
                if (playerManager.isAiming)
                {
                    Quaternion targetRotation = Quaternion.Euler(0, cameraHandler.cameraTransform.eulerAngles.y, 0);
                    Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, 1);
                    transform.rotation = playerRotation;
                }
                else
                {
                    if (inputHandler.lockOnFlag)
                    {
                        if (inputHandler.sprintFlag || inputHandler.rollFlag)
                        {
                            Vector3 targetDirection = Vector3.zero;
                            targetDirection = cameraHandler.cameraTransform.forward * inputHandler.vertical;
                            targetDirection += cameraHandler.cameraTransform.right * inputHandler.horizontal;
                            targetDirection.Normalize();
                            targetDirection.y = 0;

                            if (targetDirection == Vector3.zero)
                            {
                                targetDirection = transform.forward;
                            }

                            Quaternion tr = Quaternion.LookRotation(targetDirection);
                            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                            transform.rotation = targetRotation;
                        }
                        else
                        {
                            Vector3 rotationDirection = moveDirection;
                            rotationDirection = cameraHandler.currentLockOnTarget.transform.position - transform.position;
                            rotationDirection.y = 0;
                            rotationDirection.Normalize();
                            Quaternion tr = Quaternion.LookRotation(rotationDirection);
                            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                            transform.rotation = targetRotation;
                        }

                    }
                    else
                    {
                        Vector3 targetDir = Vector3.zero;
                        float moveOverride = inputHandler.moveAmount;

                        targetDir = cameraObject.forward * inputHandler.vertical;
                        targetDir += cameraObject.right * inputHandler.horizontal;

                        targetDir.Normalize();
                        targetDir.y = 0;

                        if (targetDir == Vector3.zero)
                        {
                            targetDir = myTransform.forward;
                        }

                        float rs = rotationSpeed;
                        Quaternion tr = Quaternion.LookRotation(targetDir);
                        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * rotationSpeed * Time.deltaTime);
                        myTransform.rotation = targetRotation;
                    }
                }
            }
            else
            {

            }
        }
        private Vector3 lastEulerRot;
        private float lastMoveAmount;
        public void HandleMovement()
        {
            if (inputHandler.rollFlag)
            {
                return;
            }

            if (playerManager.isInteracting)
            {
                return;
            }
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();

            //Quaternion currentRotation = myTransform.rotation;
            //Vector3 currentEuler = currentRotation.eulerAngles;
            //Vector3 eulerAngleDifference = currentEuler - lastEulerRot;
            //animatorHandler.anim.SetFloat("Horizontal", WrapAngle(eulerAngleDifference.y), 0.2f, Time.deltaTime);
            //lastEulerRot = currentEuler;
            float speed = movementSpeed;

            if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5f)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
                playerStatsManager.TakeStaminaDamage(sprintStaminaCost);
            }
            else
            {
                if(inputHandler.moveAmount < 0.5f)
                {
                    moveDirection *= walkSpeed;
                    playerManager.isSprinting = false;
                }
                else
                {
                    moveDirection *= speed;
                    playerManager.isSprinting = false;
                }
            }

            Vector3 projectVeclocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            //moveDiretion.y = 0;
            //Vector3 velocity = Vector3.SmoothDamp(rigidbody.velocity, projectVeclocity, ref dampMoveCurrentSpeed, 0.2f);
            rigidbody.velocity = projectVeclocity;

            if (inputHandler.lockOnFlag && inputHandler.sprintFlag == false)
            {
                playerAnimationManager.UpdateAnimatorValues(inputHandler.moveAmount, inputHandler.horizontal, playerManager.isSprinting);
            }
            else
            {
                playerAnimationManager.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);
            }

        }
        float WrapAngle(float angle)
        {
            angle = angle % 360;
            if (angle > 180)
            {
                angle -= 360;
            }
            return angle;
        }
        public void HandleRollingAndSprint()
        {
            if (playerAnimationManager.animator.GetBool("isInteracting"))
            {
                return;
            }

            //check if we have stamina, if we do not, return
            if(playerStatsManager.currentStamina <= 0)
            {
                return;
            }

            if (inputHandler.rollFlag)
            {
                inputHandler.rollFlag = false;
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;

                if(inputHandler.moveAmount > 0)
                {
                    playerAnimationManager.PlayTargetAnimation("Rolling", true);
                    //playerAnimationManager.EraseHandIKForWeapon();
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = rollRotation;
                    playerStatsManager.TakeStaminaDamage(rollStaminaCost);
                }
                else
                {
                    playerAnimationManager.PlayTargetAnimation("Backstep", true);
                    //playerAnimationManager.EraseHandIKForWeapon();
                    playerStatsManager.TakeStaminaDamage(backstepStaminaCost);
                }
            }
        }
        
        
        public void HandleFalling(Vector3 moveDirection)
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint;

            //this means forward has obstacles
            if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
            {
                moveDirection = Vector3.zero;
            }

            if (playerManager.isInAir)
            {
                //rigidbody.AddForce(-Vector3.up * fallingSpeed);
                // add some directionForce in order to not stuck in walls
                if(playerManager.isJumping==false)
                {
                    rigidbody.AddForce(moveDirection * fallingSpeed / 10f);
                    rigidbody.AddForce(-Vector3.up * fallingSpeed);
                }
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin = origin + dir * groundDirectionRayDistance;

            targetPosition = myTransform.position;
            // grounded
            Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, ignoreForGroundCheck);
            if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                if (!playerManager.isJumping)
                {
                    targetPosition.y = tp.y;
                    Vector3 velocity = rigidbody.velocity;
                    velocity.y = 0;
                    rigidbody.velocity = velocity;
                }

                if (playerManager.isInAir)
                {
                    Debug.Log("should land");
                    if (inAirTimer > 0.5f)
                    {
                        Debug.Log("You were in Air for " + inAirTimer);
                        playerAnimationManager.PlayTargetAnimation("Land", true);
                        playerAnimationManager.animator.SetBool("isJumping", false);
                        inAirTimer = 0;
                    }
                    else
                    {
                        playerAnimationManager.PlayTargetAnimation("Empty", false);
                        playerAnimationManager.animator.SetBool("isJumping", false);
                        inAirTimer = 0;
                    }
                    playerManager.isInAir = false;
                }
            }
            //not grounded
            else
            {
                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                if (playerManager.isInAir == false)
                {
                    if (playerManager.isInteracting == false)
                    {
                        playerAnimationManager.PlayTargetAnimation("Falling", true);
                    }
                    Debug.Log("falling");
                    Vector3 vel = rigidbody.velocity;
                    vel.Normalize();
                    rigidbody.velocity = vel * (movementSpeed / 2);
                    playerManager.isInAir = true;
                }
            }
            //这里只是控制y轴
            if (playerManager.isGrounded)
            {
                if (playerManager.isInteracting || inputHandler.moveAmount > 0)
                {
                    myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, 30 * Time.deltaTime);
                }
                else
                {
                    myTransform.position = targetPosition;
                }
            }
        }
        #endregion
    }
}
