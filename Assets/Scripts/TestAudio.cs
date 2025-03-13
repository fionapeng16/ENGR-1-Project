using UnityEngine;

public class TestAudio : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            Debug.Log("Playing Saturn Music...");
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No AudioSource found!");
        }
    }
}