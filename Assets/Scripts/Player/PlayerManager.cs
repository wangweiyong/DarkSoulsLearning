using UnityEngine;

namespace wwy
{
    public class PlayerManager : CharacterManager
    {
        InputHandler inputHandler;
        PlayerLocomotionManager playerLocomotion;
        PlayerStatsManager playerStatsManager;
        PlayerEffectsManager playerEffectsManager;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;
        InteractableUI interactableUI;
        Animator animator;
        PlayerAnimatorManager playerAnimatorManager;
        CameraHandler cameraHandler;

        protected override void Awake()
        {
            base.Awake();
            backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
            inputHandler = GetComponent<InputHandler>();
            animator = GetComponentInChildren<Animator>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerLocomotion = GetComponent<PlayerLocomotionManager>();
            interactableUI = FindObjectOfType<InteractableUI>();
            playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
        }
        private void Start()
        {
            cameraHandler = CameraHandler.singleton;

        }
        private void Update()
        {
            float delta = Time.deltaTime;
            isInteracting = animator.GetBool("isInteracting");
            canDoCombo = animator.GetBool("CanDoCombo");
            isJumping = animator.GetBool("isJumping");
            animator.SetBool("isInAir", isInAir);
            animator.SetBool("isBlocking", isBlocking);
            animator.SetBool("isTwoHandingWeaon", isTwoHandingWeapon);
            isUsingLeftHand = animator.GetBool("isUsingLeftHand");
            isUsingRightHand = animator.GetBool("isUsingRightHand");
            isInvulnerable = animator.GetBool("isInvulnerable");
            isFiringSpell = animator.GetBool("isFiringSpell");
            isHoldingArrow = animator.GetBool("isHoldingArrow");
            animator.SetBool("isDead", playerStatsManager.isDead);
            inputHandler.TickInput(delta);
            playerAnimatorManager.canRotate = animator.GetBool("canRotate");
            playerLocomotion.HandleRollingAndSprint(delta);
            playerLocomotion.HandleJumping();

            playerStatsManager.RegenerateStamina();

            CheckForInteractableObject();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            float delta = Time.deltaTime;
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleRotation(delta);
            playerEffectsManager.HandleAllBuildUpEffects();
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;
        }
        private void LateUpdate()
        {
            float delta = Time.deltaTime;
            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }

            //Update InputHandler
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.d_Pad_Up = false;
            inputHandler.a_Input = false;
            inputHandler.jump_Input = false;
            inputHandler.inventory_Input = false;
            inputHandler.lt_Input = false;  
            isSprinting = inputHandler.b_Input;

            if (isInAir)
            {
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }
        }

        #region Player Interactons
        public void CheckForInteractableObject()
        {
            RaycastHit hit;
            
            if(Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {
                if(hit.collider.tag == "Interactable")
                {
                    Interactabble interactableObject = hit.collider.GetComponent<Interactabble>();
                    if(interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        // set the ui text to the interactable
                        //set the text pop up to true
                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);

                        if (inputHandler.a_Input)
                        {
                            interactableObject.Interact(this);
                        }
                    }
                }
            }
            else
            {
                if(interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if(itemInteractableGameObject != null && inputHandler.a_Input)
                {
                    itemInteractableGameObject.SetActive(false);
                }
            }
        }
        
        public void OpenChestInteraction(Transform playerStandsHereWithOpeningChest)
        {
            playerLocomotion.rigidbody.velocity = Vector3.zero;
            transform.position = playerStandsHereWithOpeningChest.position;
            playerAnimatorManager.PlayTargetAnimation("Open Chest", true);
        }

        public void PassThroughForWallInteraction(Transform fogwallEntrance)
        {
            //wanna make sure we face the fog wall first
            playerLocomotion.rigidbody.velocity = Vector3.zero;

            Vector3 rotationDirection = fogwallEntrance.transform.forward;
            Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = turnRotation;
            //rotate over time so it does not llok as rigid

            playerAnimatorManager.PlayTargetAnimation("Pass Throught Fog", true);
            
        }
        #endregion

    }
}
