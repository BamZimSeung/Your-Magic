using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAR_TouchTest : MonoBehaviour
{
    public static MAR_TouchTest instance;


    [SerializeField] private AudioClip useMagicAudio;
    [SerializeField] private AudioClip shootMagicAudio;


    const int channel = 1;
    
    OVRHapticsClip shootClip;
    OVRHapticsClip magicClip;

    // Use this for initialization

    bool isOn = false;
    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
            shootClip = new OVRHapticsClip(shootMagicAudio);
            magicClip = new OVRHapticsClip(useMagicAudio);
        }
    }
    

    private void Update()
    {
        // Make Haptic Clip:

        

        // Play Haptic Clip:
        switch (MAR_HandState.handState[(int)MAR_HandState.Hand.LEFT])
        {
            case MAR_HandState.State.MAGIC_USE:
                OVRHaptics.LeftChannel.Queue(magicClip);
                break;
        }


        switch (MAR_HandState.handState[(int)MAR_HandState.Hand.RIGHT])
        {
            case MAR_HandState.State.MAGIC_USE:
                OVRHaptics.RightChannel.Queue(magicClip);
                break;
        }


    }

    public void ClearVibration(int whatHand)
    {
        if (whatHand == (int)MAR_HandState.Hand.LEFT)
        {
            OVRHaptics.LeftChannel.Clear();
        }
        else
        {
            OVRHaptics.RightChannel.Clear();
        }
    }

    public void ShootVibration(int whatHand)
    {
        if (whatHand == (int)MAR_HandState.Hand.LEFT)
        {
            OVRHaptics.LeftChannel.Preempt(shootClip);
        }
        else
        {
            OVRHaptics.RightChannel.Preempt(shootClip);
        }
    }
}

 