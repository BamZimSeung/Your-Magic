using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAR_TouchTest : MonoBehaviour
{
    public static MAR_TouchTest instance;


    [SerializeField] private AudioClip useMagicAudio;
    [SerializeField] private AudioClip shootMagicAudio;


    const int channel = 1;

    OVRHapticsClip HapticClip;

    // Use this for initialization

    bool isOn = false;
    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    

    private void Update()
    {

        // Make Haptic Clip:


        HapticClip = new OVRHapticsClip(useMagicAudio);

        // Play Haptic Clip:
        switch (MAR_HandState.handState[(int)MAR_HandState.Hand.LEFT])
        {
            case MAR_HandState.State.MAGIC_USE:
                OVRHaptics.LeftChannel.Queue(HapticClip);
                break;
        }


        switch (MAR_HandState.handState[(int)MAR_HandState.Hand.RIGHT])
        {
            case MAR_HandState.State.MAGIC_USE:
                OVRHaptics.RightChannel.Queue(HapticClip);
                break;
        }


    }

    public void ClearVibration()
    {
        OVRHaptics.LeftChannel.Clear();
        OVRHaptics.RightChannel.Clear();
    }

    public void ShootVibration(int whatHand)
    {
        HapticClip = new OVRHapticsClip(shootMagicAudio);
        if (whatHand == (int)MAR_HandState.Hand.LEFT)
        {
            OVRHaptics.LeftChannel.Preempt(HapticClip);
        }
        else
        {
            OVRHaptics.RightChannel.Preempt(HapticClip);
        }
    }

}

 