using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using Photon.Realtime;

public class VoiceChat : MonoBehaviourPunCallbacks
{

    Recorder rec;

    private void Start()
    {
        rec = GetComponent("Recorder") as Recorder;
    }

    private void Update() {
        if(rec)
        {
            rec.TransmitEnabled = true;
            rec.DebugEchoMode = true;
        }
    }
    
}
