using UnityEngine;

namespace wwy
{
    public class PlayerManager : CharacterManager
    {
        public UIManager uIManager;
        public InputHandler inputHandler;
        PlayerLocomotionManager playerLocomotion;
        public PlayerStatsManager playerStatsManager;
        public PlayerInventoryManager playerInventoryManager;
        public PlayerEffectsManager playerEffectsManager;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;
        public PlayerWeaponSlotManager playerWeaponSlotManager;
        public PlayerCombatManager playerCombatManager;
        InteractableUI interactableUI;
        public PlayerEquipmentManager playerEquipmentManager;
        Animator animator;
        public PlayerAnimatorManager playerAnimatorManager;
        public CameraHandler cameraHandler;

        protected override void Awake()
        {
            base.Awake();
            backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
            inputHandler = GetComponent<InputHandler>();
            animator = GetComponentInChildren<Animator>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerLocomotion = GetComponent<PlayerLocomotionManager>();
            interactableUI = FindObjectOfType<InteractableUI>();
            playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            uIManager = FindObjectOfType<UIManager>();
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
            animator.SetBool("isTwoHandingLocomotion", isTwoHandingWeapon);
            isInvulnerable = animator.GetBool("isInvulnerable");
            isFiringSpell = animator.GetBool("isFiringSpell");
            isHoldingArrow = animator.GetBool("isHoldingArrow");
            animator.SetBool("isDead", playerStatsManager.isDead);
            inputHandler.TickInput(delta);
            playerAnimatorManager.canRotate = animator.GetBool("canRotate");
            playerLocomotion.HandleRollingAndSprint();
            playerLocomotion.HandleJumping();

            playerStatsManager.RegenerateStamina();

            CheckForInteractableObject();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            playerLocomotion.HandleFalling(playerLocomotion.moveDirection);
            playerLocomotion.HandleMovement();
            playerLocomotion.HandleRotation();
            playerEffectsManager.HandleAllBuildUpEffects();
            inputHandler.sprintFlag = false;
        }
        private void LateUpdate()
        {
            float delta = Time.deltaTime;
            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation();
            }
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.d_Pad_Up = false;
            inputHandler.a_Input = false;
            inputHandler.inventory_Input = false;
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
