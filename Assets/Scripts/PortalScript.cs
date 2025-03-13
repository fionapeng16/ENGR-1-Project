using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PortalScript : MonoBehaviour
{
    public AudioClip portalSound; // Assign this in the Inspector
    private AudioSource audioSource;
    private bool hasEntered = false; // Prevents multiple triggers
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
                StartCoroutine(PlayShortenedSound(audioSource, 1f)); // Plays only 2.5 seconds
            }
            else
            {
                SceneManager.LoadScene("Fiona2.0"); // Load scene immediately if no sound is assigned
            }
        }
    }

    private IEnumerator PlayShortenedSound(AudioSource audioSource, float cutOffTime)
    {
        audioSource.Play();
        yield return new WaitForSeconds(cutOffTime); // Stops the sound after X seconds
        SceneManager.LoadScene("Fiona2.0");
    }
}