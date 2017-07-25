using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandState {
    

    public enum State
    {
        IDLE,
        MAGIC_CONTROLL_1,
        MAGIC_CONTROLL_2,
        MAGIC_CONTROLL_3,
        PAD_CONTROLL,
        MAGIC_USE,
        SHIELD
    }

    public enum Hand
    {
        LEFT=0,
        RIGHT=1
    }

    public static State[] handState = new State[2] { State.IDLE, State.IDLE };

}
