using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; 

public class SoundManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created



    [SerializeField] Slider volumeSlider;
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load ();
        }
        else
        {
            Load();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }

    private void Load()
    {
        
        {
            volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }
    }

    private void Save ()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
