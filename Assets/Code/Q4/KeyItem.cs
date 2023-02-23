using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FPS
{
    public class Keyitem : MonoBehaviour
    {
        public int id;

        private void OnTriggerEnter(Collider other)
        {
           
                PlayerController targetPlayer = other.GetComponent<PlayerController>();
                if(targetPlayer != null)
                {
                    targetPlayer.keyIDsObtained.Add(id);
                    Destroy(gameObject);
                }
            }
        }
    }

