using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource audioSource;
    public AudioSource enemySource;
    [Header("Player")]
    public AudioClip runAudio, fiteAudio, normalAudio, dakandaoAudio, kexuezhanfuAudio, xiaolifeidaoAudio, lightAudio, levelupAudio, winAudio;

    [Header("Enemy")]
    public AudioClip attackAudio, lightningAudio;

    private void Awake()
    {
        instance = this;
    }

    public void RunAudio()
    {
        audioSource.clip = runAudio;
        audioSource.Play();
    }

    public void FiteAudio()
    {
        audioSource.clip = fiteAudio;
        audioSource.Play();
    }

    public void NoramlAudio()
    {
        audioSource.clip = normalAudio;
        audioSource.Play();
    }

    public void DakandaoAudio()
    {
        audioSource.clip = dakandaoAudio;
        audioSource.Play();
    }

    public void KexuezhanfuAudio()
    {
        audioSource.clip = kexuezhanfuAudio;
        audioSource.Play();
    }

    public void XiaolifeidaoAudio()
    {
        audioSource.clip = xiaolifeidaoAudio;
        audioSource.Play();
    }

    public void EnemyAttackAudio()
    {
        enemySource.clip = attackAudio;
        enemySource.Play();
    }

    public void LightAudio()
    {
        enemySource.clip = lightAudio;
        enemySource.Play();
    }

    public void LevelUpAudio()
    {
        audioSource.clip = levelupAudio;
        audioSource.Play();
    }

    public void WinAudio()
    {
        audioSource.clip = winAudio;
        audioSource.Play();
    }
}
