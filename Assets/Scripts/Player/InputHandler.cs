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
        public bool jump_Input;
        public bool inventory_Input;

        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;
        public bool right_Stick_Right_Input;
        public bool left_Stick_Left_Input;


        public float durationForRoll2Sprint = 0.3f;

        PlayerControls inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        PlayerManager playerManager;
        UIManager uiManager;
        CameraHandler cameraHandler;


        WeaponSlotManager weaponSlotManager;

        Vector2 movementInput;
        Vector2 cameraInput;


        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory= GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            uiManager = FindObjectOfType<UIManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
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

                inputActions.PlayerQuickSlots.D_Pad_Right.performed += i => { d_Pad_Right = true; };
                inputActions.PlayerQuickSlots.D_Pad_Up.performed += i => { d_Pad_Up = true; };
                inputActions.PlayerQuickSlots.D_Pad_Down.performed += i => { d_Pad_Down = true; };
                inputActions.PlayerQuickSlots.D_Pad_Left.performed += i => { d_Pad_Left = true; };
            
                inputActions.PlayerActions.A.performed += i => { a_Input = true; };
                inputActions.PlayerActions.Jump.performed += i => { jump_Input = true; };
                inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOnInut = true;

                inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => left_Stick_Left_Input = true;

                inputActions.PlayerActions.Y.performed += i => y_Input = true;
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
            HandleAttackInput(delta);
            HandleQuickSlotsInput(delta);
            HandleInteractingButtonInput();
            HandleJumpInput();
            HandleInventoryInput();
            HandleLockOnInput();
            HandleTwoHandFlagInput();
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
            b_Input = inputActions.PlayerActions.Roll.IsPressed();

            if (b_Input)
            {
                rollInputTimer += delta;
                if (rollInputTimer >= durationForRoll2Sprint)
                {
                    sprintFlag = true;
                }
            }
            else
            {
                if(rollInputTimer > 0 && rollInputTimer < durationForRoll2Sprint)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }
                rollInputTimer = 0;
            }
        }
        
        private void HandleAttackInput(float delta)
        {
            if (rb_Input)
            {
                if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                }
                else
                {
                    if (playerManager.isInteracting) return;
                    if (playerManager.canDoCombo) return;
                    playerAttacker.HandleLigthAttack(playerInventory.rightWeapon);
                }
            }
            if (rt_Input)
            {
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }
        private void HandleQuickSlotsInput(float delta)
        {
          
            if (d_Pad_Right)
            {
                playerInventory.ChangeRightWeapon();
            }

            else if(d_Pad_Left)
            {
                playerInventory.ChangeLeftWeapon();
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
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                }
                else
                {
                    //disable two handing
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);

                }
            }
        }
    }
}
