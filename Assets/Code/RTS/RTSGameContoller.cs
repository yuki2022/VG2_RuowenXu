using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RTS
{
    public class RTSGameController : MonoBehaviour
    {
        public Canvas uniCanvas;
        public RectTransform selectionBox;

        float mouseDragThreshold = 3f;

        public List<GameObject> currentSelection;
        public Vector2 mouseClickStart;

        void Start()
        {
            currentSelection = new List<GameObject>();
            
        }

        // Update is called once per frame
        void Update()
        {
            // Mouse Interactions
            Mouse mouse = Mouse.current;
            if(mouse != null)
            {   // Start Drag
                if (mouse.leftButton.wasPressedThisFrame)
                {
                    mouseClickStart = mouse.position.ReadValue();

                }

                // Handle Drag
                if (mouse.leftButton.isPressed)
                {
                    Vector2 mousePosition = mouse.position.ReadValue();
                    if(Vector2.Distance(mouseClickStart, mousePosition) > mouseDragThreshold)
                    {
                        selectionBox.gameObject.SetActive(true);
                        Vector2 boxMidpoint = Vector2.Lerp(mouseClickStart, mousePosition, 0.5f);
                        selectionBox.anchoredPosition = boxMidpoint / uniCanvas.scaleFactor;

                        Vector2 box = new Vector2(
                            Mathf.Abs(mouseClickStart.x - mousePosition.x),
                            Mathf.Abs(mouseClickStart.y - mousePosition.y)
                        );
                        selectionBox.sizeDelta = box / uniCanvas.scaleFactor;
                    }
                }

                // Selection Command
                if (mouse.leftButton.wasReleasedThisFrame)
                {
                    if (selectionBox.gameObject.activeInHierarchy)
                    {
                        SelectWithinBox();
                    }
                    else
                    {
                        SelectUnderMouse();
                    }
                    selectionBox.gameObject.SetActive(false);
                }

                // Interaction Command
                if(mouse.rightButton.wasPressedThisFrame && currentSelection.Count > 0)
                {
                    foreach(GameObject selection in currentSelection)
                    {
                        RTSCharacterController character = selection.GetComponent<RTSCharacterController>();
                        if (character)
                        {
                            Ray selectionRaycast = Camera.main.ScreenPointToRay(mouse.position.ReadValue());
                            RaycastHit[] hits = Physics.RaycastAll(selectionRaycast);

                            foreach (RaycastHit hit in hits)
                            {   if(hit.collider.gameObject.layer != LayerMask.NameToLayer("Ground"))
                                {
                                    character.SetTarget(hit.collider.gameObject);
                                    break;
                                }
                                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                                {
                                    character.SetTarget(null);
                                    character.SetDestination(hit.point);
                                }
                            }
                        }

                    }
                }
                
            }
        }

        void SelectUnderMouse()
        {
            Ray selectionRaycast = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit[] hits = Physics.RaycastAll(selectionRaycast);

            DeselectAll();
            foreach(RaycastHit hit in hits)
            {
                if (hit.collider.GetComponent<RTSCharacterController>())
                {
                    currentSelection.Add(hit.collider.gameObject);
                    hit.collider.SendMessage("Select", SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        void SelectWithinBox()
        {
            RTSCharacterController[] characterControllers = FindObjectsOfType<RTSCharacterController>();

            DeselectAll();
            foreach(RTSCharacterController character in characterControllers)
            {
                Vector2 characterPosition = Camera.main.WorldToScreenPoint(character.transform.position);

                characterPosition = characterPosition / uniCanvas.scaleFactor;

                Rect anchoredRect = new Rect(
                    selectionBox.anchoredPosition.x - selectionBox.sizeDelta.x / 2f,
                    selectionBox.anchoredPosition.y - selectionBox.sizeDelta.y / 2f,
                    selectionBox.sizeDelta.x,
                    selectionBox.sizeDelta.y

                );

                if (anchoredRect.Contains(characterPosition))
                {
                    currentSelection.Add(character.gameObject);
                    character.SendMessage("Select", SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        void DeselectAll()
        {
            foreach(GameObject selection in currentSelection)
            {
                selection.SendMessage("Deselect", SendMessageOptions.DontRequireReceiver);

            }
            currentSelection.Clear();
        }
    }

}
