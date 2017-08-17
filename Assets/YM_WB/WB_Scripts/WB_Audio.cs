﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WB_Audio : MonoBehaviour {
    // 오디오 용
    private AudioClip[] audioClips = new AudioClip[17];
    private AudioSource audioSources = new AudioSource();

    private void Awake()
    {
        audioSources = gameObject.AddComponent<AudioSource>();
        audioSources.Stop();
        
        audioClips[0] = Resources.Load("wand_001", typeof(AudioClip)) as AudioClip; // 패턴 발동시.
        audioClips[1] = Resources.Load("recovery_02_a", typeof(AudioClip)) as AudioClip; // 패턴 활성화 소리
        audioClips[2] = Resources.Load("recovery_02_b", typeof(AudioClip)) as AudioClip; // 패턴 활성화 소리
        audioClips[3] = Resources.Load("Menu1B", typeof(AudioClip)) as AudioClip; // 패턴 활성화 소리
        audioClips[4] = Resources.Load("chant_008", typeof(AudioClip)) as AudioClip; // 에너지 볼트 생성
        audioClips[5] = Resources.Load("ray_03", typeof(AudioClip)) as AudioClip; // 에너지 볼트 발사
        audioClips[6] = Resources.Load("flame_012", typeof(AudioClip)) as AudioClip; // 화염구 생성
        audioClips[7] = Resources.Load("explosion_002", typeof(AudioClip)) as AudioClip; // 화염구 폭팔.
        audioClips[8] = Resources.Load("thunder_006_a", typeof(AudioClip)) as AudioClip; // 번개마법.
        audioClips[9] = Resources.Load("ice_002_b", typeof(AudioClip)) as AudioClip; // 아이스 스피어 생성
        audioClips[10] = Resources.Load("ice_004", typeof(AudioClip)) as AudioClip; // 아이스 스피어 타격
        audioClips[11] = Resources.Load("gravity_01", typeof(AudioClip)) as AudioClip; // 중력자탄 생성.
        audioClips[12] = Resources.Load("barrier_001", typeof(AudioClip)) as AudioClip; // 쉴드 생성시.
        audioClips[13] = Resources.Load("barrier_001_1", typeof(AudioClip)) as AudioClip; // 막을 때
        audioClips[14] = Resources.Load("magicfail (1)", typeof(AudioClip)) as AudioClip; // 마법 실패 할 때
        audioClips[15] = Resources.Load("magicfail (2)", typeof(AudioClip)) as AudioClip; // 마법 실패 할 때
        audioClips[16] = Resources.Load("magicfail (3)", typeof(AudioClip)) as AudioClip; // 마법 실패 할 때
    }

    public void Play(int index)
    {
        audioSources.clip = audioClips[index];
        audioSources.loop = false;
        audioSources.playOnAwake = false;
        audioSources.Play();
    }
}