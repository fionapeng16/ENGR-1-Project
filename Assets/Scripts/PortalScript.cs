using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Ensure the player has the tag "Player"
        {
            SceneManager.LoadScene("Fiona2.0"); // Load the next scene
        }
    }
}