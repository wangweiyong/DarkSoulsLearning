using UnityEngine;

namespace wwy
{
    public class PlayerManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerLocomotion playerLocomotion;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;
        InteractableUI interactableUI;
        Animator anim;

        CameraHandler cameraHandler;


        public bool isInteracting;

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;
        public bool isJumping;

        private void Start()
        {
            cameraHandler = CameraHandler.singleton;
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            interactableUI = FindObjectOfType<InteractableUI>();    
        }

        private void Update()
        {
            float delta = Time.deltaTime;
            isInteracting = anim.GetBool("isInteracting");
            canDoCombo = anim.GetBool("CanDoCombo");
            isJumping = anim.GetBool("isJumping");
            anim.SetBool("isInAir", isInAir);
            inputHandler.TickInput(delta);
            playerLocomotion.HandleRollingAndSprint(delta);
            playerLocomotion.HandleJumping();
            CheckForInteractableObject();
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
            inputHandler.a_Input = false;
            inputHandler.jump_Input = false;
            inputHandler.inventory_Input = false;

            isSprinting = inputHandler.b_Input;

            if (isInAir)
            {
                playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
            }
        }
    
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
    }
}
