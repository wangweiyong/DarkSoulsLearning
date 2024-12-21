using UnityEngine;

namespace wwy
{
    public class PlayerManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerLocomotion playerLocomotion;
        Animator anim;

        CameraHandler cameraHandler;


        public bool isInteracting;

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;

        private void Start()
        {
            cameraHandler = CameraHandler.singleton;
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
        }

        private void Update()
        {
            float delta = Time.deltaTime;
            isInteracting = anim.GetBool("isInteracting");
            canDoCombo = anim.GetBool("CanDoCombo");
            inputHandler.TickInput(delta);
            playerLocomotion.HandleRollingAndSprint(delta);
        }

        private void FixedUpdate()
        {
            float delta = Time.deltaTime;
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);

            playerLocomotion.HandleMovement(delta);


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

            isSprinting = inputHandler.b_Input;

            if (isInAir)
            {
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }
        }
    }
}
