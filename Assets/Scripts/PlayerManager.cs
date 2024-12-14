using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

            inputHandler.TickInput(delta);
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleRollingAndSprint(delta);
            
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
            inputHandler.rollFlag = false;
            inputHandler.sprintFlag = false;

            isSprinting = inputHandler.b_Input;
        }
    }
}
