using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManeger : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip start;
    [SerializeField] AudioClip spawn;
    [SerializeField] AudioClip mark;
    [SerializeField] AudioClip shot;
    [SerializeField] AudioClip damage;
    [SerializeField] AudioClip delete;
    [SerializeField] AudioClip reroll;
    [SerializeField] AudioClip conbo;
    [SerializeField] AudioClip winner;
    [SerializeField] AudioClip lose;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(start);
    }

    void Update()
    {
        
    }

    public void SpawnSound()
    {
        audioSource.PlayOneShot(spawn);
    }

    public void MarkSound()
    {
        audioSource.PlayOneShot(mark);
    }

    public void ShotSound()
    {
        audioSource.PlayOneShot(shot);
    }

    public void DamageSound()
    {
        audioSource.PlayOneShot(damage);
    }

    public void DeleteSound()
    {
        audioSource.PlayOneShot(delete);
    }

    public void RerollSound()
    {
        audioSource.PlayOneShot(reroll);
    }

    public void ConboSound()
    {
        audioSource.PlayOneShot(conbo);
    }

    public void WineerSound()
    {
        audioSource.PlayOneShot(winner);
    }

    public void LoseSound()
    {
        audioSource.PlayOneShot(lose);
    }
}
