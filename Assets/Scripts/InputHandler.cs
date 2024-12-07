using System.Collections;
using System.Collections.Generic;
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

        PlayerControls inputActions;
        CameraHandler cameraHandler;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            cameraHandler = CameraHandler.singleton;
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            if(cameraHandler != null )
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
            }
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
            }
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
        }

        private void MoveInput(float delta)
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
}
