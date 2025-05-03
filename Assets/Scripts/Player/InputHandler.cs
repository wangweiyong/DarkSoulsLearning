using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting;
using UnityEngine;

namespace wwy
{

    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_Input;
        public bool a_Input;
        public bool y_Input;
        public bool x_Input;
        public bool lockOnInut;
        public bool sprintFlag;
        public bool comboFlag;
        public bool inventoryFlag;
        public bool rollFlag;
        public bool lockOnFlag;
        public bool twoHandFlag;
        public float rollInputTimer;
        public bool rb_Input;
        public bool rt_Input;
        public bool lt_Input;
        public bool lb_Input;
        public bool critical_Attack_Input;
        public bool jump_Input;
        public bool inventory_Input;

        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;
        public bool right_Stick_Right_Input;
        public bool left_Stick_Left_Input;


        public float durationForRoll2Sprint = 0.3f;

        public Transform criticalAttackRayCastStartPoint;
        PlayerControls inputActions;
        PlayerCombatManager playerCombatManager;
        PlayerInventoryManager playerInventoryManager;
        PlayerManager playerManager;
        UIManager uiManager;
        CameraHandler cameraHandler;
        BlockingCollider blockingCollider;
        PlayerEffectsManager playerEffectsManager;
        PlayerAnimatorManager playerAnimatorManager;
        PlayerStatsManager playerStatsManager;
        PlayerWeaponSlotManager weaponSlotManager;

        Vector2 movementInput;
        Vector2 cameraInput;


        private void Awake()
        {
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerInventoryManager= GetComponent<PlayerInventoryManager>();
            playerManager = GetComponent<PlayerManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            weaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            blockingCollider = GetComponentInChildren<BlockingCollider>();
            uiManager = FindObjectOfType<UIManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
        }

        private void OnEnable()
        {
            if(inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += (inputActions) =>
                { movementInput = inputActions.ReadValue<Vector2>(); };
                inputActions.PlayerMovement.Camera.performed += (inputActions) =>
                { cameraInput = inputActions.ReadValue<Vector2>(); };

                inputActions.PlayerActions.RB.performed += i => rb_Input = true;
                inputActions.PlayerActions.RT.performed += i => rt_Input = true;
                inputActions.PlayerActions.LT.performed += i => lt_Input = true;
                inputActions.PlayerActions.LB.performed += i => lb_Input = true;
                inputActions.PlayerActions.LB.canceled += i => lb_Input = false;
                inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
                inputActions.PlayerActions.Roll.performed += i => b_Input = true;

                inputActions.PlayerQuickSlots.D_Pad_Right.performed += i => { d_Pad_Right = true; };
                inputActions.PlayerQuickSlots.D_Pad_Up.performed += i => { d_Pad_Up = true; };
                inputActions.PlayerQuickSlots.D_Pad_Down.performed += i => { d_Pad_Down = true; };
                inputActions.PlayerQuickSlots.D_Pad_Left.performed += i => { d_Pad_Left = true; };
            
                inputActions.PlayerActions.A.performed += i => { a_Input = true; };
                inputActions.PlayerActions.X.performed += i => { x_Input = true; };
                inputActions.PlayerActions.Jump.performed += i => { jump_Input = true; };
                inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOnInut = true;

                inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => left_Stick_Left_Input = true;

                inputActions.PlayerActions.Y.performed += i => y_Input = true;

                inputActions.PlayerActions.CriticalAttack.performed += i => critical_Attack_Input = true;
            }
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            HandleMoveInput(delta);
            HandleRollInput(delta);
            HandleCombatInput(delta);
            HandleQuickSlotsInput(delta);
            HandleInteractingButtonInput();
            HandleJumpInput();
            HandleInventoryInput();
            HandleLockOnInput();
            HandleTwoHandFlagInput();
            HandleCriticalAttackInput();
            HandleUseConsumableInput();
        }

        private void HandleMoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            //float inputMagnitude = Mathf.Clamp01(Mathf.Sqrt(horizontal * horizontal + vertical * vertical));
            //Debug.Log($"{moveAmount}, {inputMagnitude}");
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }
        private void HandleRollInput(float delta)
        {

            if (b_Input)
            {
                rollInputTimer += delta;
                if(playerStatsManager.currentStamina <= 0)
                {
                    sprintFlag = false;
                    b_Input = false;
                }
                if (moveAmount > 0.5f && playerStatsManager.currentStamina > 0)
                {
                    sprintFlag = true;
                }
            }
            else
            {
                sprintFlag = false;
                if(rollInputTimer > 0 && rollInputTimer < durationForRoll2Sprint)
                {
                    rollFlag = true;
                }
                rollInputTimer = 0;
            }
        }
        
        private void HandleCombatInput(float delta)
        {
            if (rb_Input)
            {
                playerCombatManager.HandleRBAction();
              /*  if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                }
                else
                {
                    if (playerManager.isInteracting) return;
                    if (playerManager.canDoCombo) return;
                    animatorHandler.anim.SetBool("isUsingRightHand", true);
                    playerAttacker.HandleLigthAttack(playerInventory.rightWeapon);
                }*/
            }
            if (rt_Input)
            {
                playerCombatManager.HandleHeavyAttack(playerInventoryManager.rightWeapon);
            }
            if (lb_Input)
            {
                //do a block
                playerCombatManager.HandleLBAction();
            }
            else
            {
                playerManager.isBlocking = false;
                if (blockingCollider.blockingCollider.enabled)
                {
                    blockingCollider.DisableBlockingCollider();
                }
            }
            if (lt_Input)
            {
                //if two handing handle weapon art
                //else handle light attack if melee weapon
                //handle weapon art if shield
                if (twoHandFlag)
                {

                }
                else
                {
                    playerCombatManager.HandleLTAction();
                }
            }
        }
        private void HandleQuickSlotsInput(float delta)
        {
          
            if (d_Pad_Right)
            {
                playerInventoryManager.ChangeRightWeapon();
            }

            else if(d_Pad_Left)
            {
                playerInventoryManager.ChangeLeftWeapon();
            }
        }
    
        private void HandleInteractingButtonInput()
        {
        }
    
        private void HandleJumpInput()
        {
        }

        private void HandleInventoryInput()
        {

            if (inventory_Input)
            {
                inventoryFlag = !inventoryFlag;

                if(inventoryFlag)
                {
                    uiManager.OpenSelectWindow();
                    uiManager.UpdateUI();
                    uiManager.hudWindow.SetActive(false);
                }
                else
                {
                    uiManager.CloseSelectWindow();
                    uiManager.CloseAllInventoryWindows();
                    uiManager.hudWindow.SetActive(true);
                }
            }
        }
    
        private void HandleLockOnInput()
        {
            if (lockOnInut &&lockOnFlag == false)
            {
                lockOnInut = false;
                cameraHandler.HandleLockOn();
                if(cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if(lockOnInut && lockOnFlag)
            {
                lockOnInut = false;
                lockOnFlag = false;
                //Clear Lock on targets
                cameraHandler.ClearLockOnTargets();

            }

            if(lockOnFlag && right_Stick_Right_Input)
            {
                right_Stick_Right_Input = false;
                cameraHandler.HandleLockOn();
                if(cameraHandler.rightLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockOnTarget;
                }
            }
            if(lockOnFlag && left_Stick_Left_Input)
            {
                left_Stick_Left_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.leftLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockOnTarget;
                }
            }
            cameraHandler.SetCameraHeight();
        }
       
        private void HandleTwoHandFlagInput()
        {
            if (y_Input)
            {
                y_Input = false;
                twoHandFlag = !twoHandFlag;
                if (twoHandFlag)
                {
                    //Enable two handing
                    weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
                }
                else
                {
                    //disable two handing
                    weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
                    weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);

                }
            }
        }
    
        private void HandleCriticalAttackInput()
        {
            if (critical_Attack_Input)
            {
                critical_Attack_Input = false;
                playerCombatManager.AttemptBackStabOrRiposte();
            }
        }
    
        private void HandleUseConsumableInput()
        {
            if (x_Input)
            {
                x_Input = false;
                playerInventoryManager.currentConsumableItem.AttempToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
            }
        }
    }
}
