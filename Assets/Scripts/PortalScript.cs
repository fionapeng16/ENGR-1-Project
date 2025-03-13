// using UnityEngine;
// using UnityEngine.SceneManagement;
// using System.Collections;

// public class PortalScript : MonoBehaviour
// {
//     public AudioClip portalSound; // Assign this in the Inspector
//     private AudioSource audioSource;
//     private bool hasEntered = false; // Prevents multiple triggers
//     private void Start()
//     {
//         // Add an AudioSource if not already attached
//         audioSource = gameObject.AddComponent<AudioSource>();
//     }

//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player") && !hasEntered)  
//         {
//             hasEntered = true; // Ensure the sound plays only once

//             if (portalSound != null)
//             {
//                 audioSource.PlayOneShot(portalSound); // Play portal sound
//                 StartCoroutine(PlayShortenedSound(audioSource, 1f)); // Plays only 2.5 seconds
//             }
//             else
//             {
//                 SceneManager.LoadScene("Fiona2.0"); // Load scene immediately if no sound is assigned
//             }
//         }
//     }

//     private IEnumerator PlayShortenedSound(AudioSource audioSource, float cutOffTime)
//     {
//         audioSource.Play();
//         yield return new WaitForSeconds(cutOffTime); // Stops the sound after X seconds
//         SceneManager.LoadScene("Neptune Level");
//     }
// }

//new
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PortalScript : MonoBehaviour
{
    public AudioClip portalSound; // Assign this in the Inspector
    private AudioSource audioSource;
    private bool hasEntered = false; // Prevents multiple triggers

    private string[] levelOrder = { "Neptune Level", "Earth Level", "Game Over" }; // Updated level sequence
    private static int currentLevelIndex = -1; // Starts before Neptune Level

    private void Start()
    {
        // Add an AudioSource if not already attached
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasEntered)
        {
            hasEntered = true; // Ensure the sound plays only once

            if (portalSound != null)
            {
                audioSource.PlayOneShot(portalSound); // Play portal sound
                StartCoroutine(LoadNextLevelAfterSound(1f)); // Play sound before loading next level
            }
            else
            {
                LoadNextLevel(); // Skip sound if none assigned
            }
        }
    }

    private IEnumerator LoadNextLevelAfterSound(float cutOffTime)
    {
        yield return new WaitForSeconds(cutOffTime); // Wait for sound duration
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        currentLevelIndex++;

        if (currentLevelIndex < levelOrder.Length)
        {
            SceneManager.LoadScene(levelOrder[currentLevelIndex]); // Load the next level in sequence
        }
        else
        {
            Debug.Log("Game Over – No more levels to load.");
        }
    }
}