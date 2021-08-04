using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadLine : MonoBehaviour
{
    private GameController gameController;
    private AudioSource audioSource;
    [SerializeField] AudioClip[] audioClipHit;
    [SerializeField] AudioClip audioClipBonus;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Cube"))
        {
            gameController.GameOver();
        }
    }

    public void SoundForCube()
    {
        int rand = Random.Range(0, 4);
        audioSource.clip = audioClipHit[rand];
        audioSource.Play();
    }

    public void SoundForBonusCube()
    {
        audioSource.clip = audioClipBonus;
        audioSource.Play();
    }
}
