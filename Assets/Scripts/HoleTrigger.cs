using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class HoleTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Ensure the player has the "Player" tag
        {
            RestartLevel();
        }
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reloads current scene
    }
}
