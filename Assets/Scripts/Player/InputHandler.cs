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
        public bool tap_rb_Input;
        public bool tap_rt_Input;
        public bool tap_lt_Input;
        public bool lb_Input;
        public bool tap_lb_Input;
        public bool critical_Attack_Input;
        public bool hold_rb_Input;
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
        public UIManager uiManager;
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

                inputActions.PlayerActions.RB.performed += i => tap_rb_Input = true;
                inputActions.PlayerActions.HoldRB.performed += i => hold_rb_Input = true;
                inputActions.PlayerActions.HoldRB.canceled += i => hold_rb_Input = false;

                inputActions.PlayerActions.RT.performed += i => tap_rt_Input = true;
                inputActions.PlayerActions.LT.performed += i => tap_lt_Input = true;
                inputActions.PlayerActions.LB.performed += i => lb_Input = true;
                inputActions.PlayerActions.LB.canceled += i => lb_Input = false;
                inputActions.PlayerActions.TapLB.performed += i => tap_lb_Input = true;
                
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
            if (playerStatsManager.isDead) return;
            HandleMoveInput();
            HandleRollInput();
            HandleHoldLBInput();
            HandleTapRBInput();
            HandleTapRTInput();
            HandleTapLTInput();
            HandleTapLBInput();
            HandleQuickSlotsInput();
            HandleInteractingButtonInput();
            HandleJumpInput();
            HandleInventoryInput();
            HandleLockOnInput();
            HandleTwoHandFlagInput();
            //HandleCriticalAttackInput();
            HandleUseConsumableInput();
            HandleHoldRBInput();
        }

        private void HandleMoveInput()
        {
            if (playerManager.isHoldingArrow)
            {
                horizontal = movementInput.x;
                vertical = movementInput.y;
                moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
                //float inputMagnitude = Mathf.Clamp01(Mathf.Sqrt(horizontal * horizontal + vertical * vertical));
                //Debug.Log($"{moveAmount}, {inputMagnitude}");
                if(moveAmount > 0.5f)
                {
                    moveAmount = 0.5f;
                }
                
                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
            else
            {
                horizontal = movementInput.x;
                vertical = movementInput.y;
                moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
                //float inputMagnitude = Mathf.Clamp01(Mathf.Sqrt(horizontal * horizontal + vertical * vertical));
                //Debug.Log($"{moveAmount}, {inputMagnitude}");
                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }

        }
        private void HandleRollInput()
        {

            if (b_Input)
            {
                rollInputTimer += Time.deltaTime;
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
        
        private void HandleTapRBInput()
        {
            if (tap_rb_Input)
            {
                tap_rb_Input = false;
                if (playerInventoryManager.rightWeapon.tap_RB_Action != null)
                {
                    playerManager.UpdateWhichHandCharacterIsUsing(true);
                    playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                    playerInventoryManager.rightWeapon.tap_RB_Action.PerformAction(playerManager);
                }
                
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
        }
        private void HandleHoldRBInput()
        {
            if (hold_rb_Input)
            {
                if (playerInventoryManager.rightWeapon.hold_RB_Action != null)
                {
                    playerManager.UpdateWhichHandCharacterIsUsing(true);
                    playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                    playerInventoryManager.rightWeapon.hold_RB_Action.PerformAction(playerManager);
                }
            }
        }
        private void HandleTapRTInput()
        {
            if (tap_rt_Input)
            {
                tap_rt_Input = false;
                if (playerInventoryManager.rightWeapon.tap_RT_Action != null)
                {
                    playerManager.UpdateWhichHandCharacterIsUsing(true);
                    playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                    playerInventoryManager.rightWeapon.tap_RT_Action.PerformAction(playerManager);
                }
               

            }
        }
        private void HandleTapLTInput()
        {
            if (tap_lt_Input)
            {
                tap_lt_Input = false;
                if (playerManager.isTwoHandingWeapon)
                {
                    if (playerInventoryManager.rightWeapon.tap_LT_Action != null)
                    {
                        // it will be the right handed weapon
                        playerManager.UpdateWhichHandCharacterIsUsing(true);
                        playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                        playerInventoryManager.rightWeapon.tap_LT_Action.PerformAction(playerManager);
                    }
                   
                }
                else
                {
                    if (playerInventoryManager.leftWeapon.tap_LT_Action != null)
                    {
                        playerManager.UpdateWhichHandCharacterIsUsing(false);
                        playerInventoryManager.currentItemBeingUsed = playerInventoryManager.leftWeapon;
                        playerInventoryManager.leftWeapon.tap_LT_Action.PerformAction(playerManager);
                    }
                }
            }
        }
        private void HandleHoldLBInput()
        {
            if(playerManager.isInAir || playerManager.isSprinting || playerManager.isFiringSpell)
            {
                lb_Input = false;
                return;
            }
            if (lb_Input)
            {
                //do a block
                if (playerManager.isTwoHandingWeapon)
                {
                    if (playerInventoryManager.rightWeapon.hold_LB_Action != null)
                    {
                        playerManager.UpdateWhichHandCharacterIsUsing(true);
                        playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                        playerInventoryManager.rightWeapon.hold_LB_Action.PerformAction(playerManager);
                    }
                }
                else
                {
                    if (playerInventoryManager.leftWeapon.hold_LB_Action != null)
                    {
                        playerManager.UpdateWhichHandCharacterIsUsing(false);
                        playerInventoryManager.currentItemBeingUsed = playerInventoryManager.leftWeapon;
                        playerInventoryManager.leftWeapon.hold_LB_Action.PerformAction(playerManager);
                    }
                }
            }
            else if(lb_Input == false)
            {
                if (playerManager.isAiming)
                {
                    uiManager.crossHair.SetActive(false);
                    playerManager.isAiming = false;
                    cameraHandler.ResetAimCameraRotation();
                }
                if (blockingCollider.blockingCollider.enabled)
                {
                    playerManager.isBlocking = false;
                    blockingCollider.DisableBlockingCollider();
                }
            }
        }
        private void HandleTapLBInput()
        {
            if (tap_lb_Input)
            {
                tap_lb_Input = false;
                if (playerManager.isTwoHandingWeapon)
                {
                    if (playerInventoryManager.rightWeapon.tap_RB_Action != null)
                    {
                        playerManager.UpdateWhichHandCharacterIsUsing(true);
                        playerInventoryManager.currentItemBeingUsed = playerInventoryManager.rightWeapon;
                        playerInventoryManager.rightWeapon.tap_RB_Action.PerformAction(playerManager);
                    }
                }
                else
                {
                    if (playerInventoryManager.leftWeapon.tap_RB_Action != null)
                    {
                        playerManager.UpdateWhichHandCharacterIsUsing(false);
                        playerInventoryManager.currentItemBeingUsed = playerInventoryManager.leftWeapon;
                        playerInventoryManager.leftWeapon.tap_RB_Action.PerformAction(playerManager);
                    }
                }
            }
        }
        private void HandleQuickSlotsInput()
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
                    playerManager.isTwoHandingWeapon = true;
                }
                else
                {
                    //disable two handing
                    weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
                    weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
                    playerManager.isTwoHandingWeapon = false;
                }
            }
        }
    
/*        private void HandleCriticalAttackInput()
        {
            if (critical_Attack_Input)
            {
                critical_Attack_Input = false;
                playerCombatManager.AttemptBackStabOrRiposte();
            }
        }*/
    
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
