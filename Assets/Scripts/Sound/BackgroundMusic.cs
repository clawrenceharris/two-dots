
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip backgroundMusic;
    private AudioSource audioSource;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        
    }
    private void Start()
    {
        audioSource.loop = true;
        audioSource.Play();
    }
}
