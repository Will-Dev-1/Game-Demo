using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundMusic;

    void Start()
    {
        // Ensure music plays when game starts//
        if (backgroundMusic != null)
        {
            backgroundMusic.Play();
        }
    }
}
