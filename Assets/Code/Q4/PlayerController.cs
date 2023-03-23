using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FPS
{
    public class PlayerController : MonoBehaviour
    {

        public static PlayerController instance;

        //Outlets
        public Transform povOrigin;
        public Transform projectileOrigin;
        public GameObject projectilePrefab;

        //Configuration
        public float attackRange;

        public List<int> keyIDsObtained;


        void Awake()
        {
            instance = this;
            keyIDsObtained = new List<int>();
        }

        void OnPrimaryAttack() {
            RaycastHit hit;
            bool hitSomething = Physics.Raycast(povOrigin.position, povOrigin.forward, out hit, attackRange);
            if (hitSomething) {
                Rigidbody targetRigidbody = hit.transform.GetComponent<Rigidbody>();
                if (targetRigidbody) {
                    targetRigidbody.AddForce(povOrigin.forward * 100f, ForceMode.Impulse);
                }
            }
        }

        // Update is called once per frame
        //void Update()
        //{
        //    Keyboard keyboardInput = Keyboard.current;
        //    Mouse mouseInput = Mouse.current;
        //    if (keyboardInput != null && mouseInput != null)
        //    {
        //        // E KEY Interactions
        //        if (keyboardInput.eKey.wasPressedThisFrame)
        //        {
        //            Vector2 mousePosition = mouseInput.position.ReadValue();

        //            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        //            RaycastHit hit;
        //            if (Physics.Raycast(ray, out hit, 2f))
        //            {
        //                print("Interacted with" + hit.transform.name + "from" + hit.distance + "m.");

        //                Door targetDoor = hit.transform.GetComponent<Door>();
        //                if (targetDoor)
        //                {
        //                    targetDoor.Interact();
        //                }

        //                InteractButton targetButton = hit.transform.GetComponent<InteractButton>();
        //                if(targetButton != null)
        //                {
        //                    targetButton.Interact();
        //                }
        //            }
        //        }
        //        //Left Mouse Interaction
        //        if (mouseInput.leftButton.wasPressedThisFrame) {
        //            OnPrimaryAttack();
        //        }

        //        //Right Mouse Interactions
        //        if (mouseInput.rightButton.wasPressedThisFrame) {
        //            OnSecondaryAttack();
        //        }
        //    }
        //}

        void OnInteract()
        {
            RaycastHit hit;
            if (Physics.Raycast(povOrigin.position, povOrigin.forward, out hit, 2f))
            {

                Door targetDoor = hit.transform.GetComponent<Door>();
                if (targetDoor)
                {
                    targetDoor.Interact();
                }

                InteractButton targetButton = hit.transform.GetComponent<InteractButton>();
                if (targetButton != null)
                {
                    targetButton.Interact();
                }
            }
        }

        void OnSecondaryAttack()
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileOrigin.position, Quaternion.LookRotation(povOrigin.forward));
            projectile.transform.localScale = Vector3.one * 5f;
            projectile.GetComponent<Rigidbody>().AddForce(povOrigin.forward * 25f, ForceMode.Impulse);
        }

    }
}