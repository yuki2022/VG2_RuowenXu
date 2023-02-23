using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FPS
{
    public class PlayerController : MonoBehaviour
    {

        public static PlayerController instance;

        public List<int> keyIDsObtained;


        void Awake()
        {
            instance = this;
            keyIDsObtained = new List<int>();
        }

        // Update is called once per frame
        void Update()
        {
            Keyboard keyboardInput = Keyboard.current;
            Mouse mouseInput = Mouse.current;
            if (keyboardInput != null && mouseInput != null)
            {
                // E KEY Interactions
                if (keyboardInput.eKey.wasPressedThisFrame)
                {
                    Vector2 mousePosition = mouseInput.position.ReadValue();

                    Ray ray = Camera.main.ScreenPointToRay(mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 2f))
                    {
                        print("Interacted with" + hit.transform.name + "from" + hit.distance + "m.");

                        Door targetDoor = hit.transform.GetComponent<Door>();
                        if (targetDoor)
                        {
                            targetDoor.Interact();
                        }

                        InteractButton targetButton = hit.transform.GetComponent<InteractButton>();
                        if(targetButton != null)
                        {
                            targetButton.Interact();
                        }
                    }
                }

            


            }
        }
    }
}