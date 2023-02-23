using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Q2
{
    public class KillZone : MonoBehaviour
    {
        // Start is called before the first frame update
        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<CharacterController>())
            {
                string currentScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentScene);
            }
        }


    }
}