using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkingController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        Connect();
    }

    // Update is called once per frame
    void Connect()
    {
    	Debug.Log("Connecting to master server");
    	PhotonNetwork.GameVersion = "0.1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
    	Debug.Log("Connected to master server");
    	PhotonNetwork.AutomaticallySyncScene = true;
    	PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returncode, string message)
    {
    	Debug.Log("Could not join any room, reason: " + returncode.ToString());
    	CreatePhotonRoom();
    }

    void CreatePhotonRoom()
    {
    	RoomOptions options = new RoomOptions() {IsVisible = true, IsOpen = true, MaxPlayers = 10};
    	PhotonNetwork.CreateRoom("Room " + Random.Range(0,10000), options);
    	Debug.Log("Created a new Photon Room");
    }

    public override void OnJoinedRoom() 
    {
    	Debug.Log("Joined Room");
    	SpawnPerson();
    }

    void SpawnPerson()
    {
    	GameObject ClientPerson = PhotonNetwork.Instantiate("Person", Vector3.zero, Quaternion.identity, 0);
    	ClientPerson.GetComponent<Movement>().enabled = true;
    	ClientPerson.transform.Find("Main Camera").gameObject.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
    	Debug.Log("Disconnected from server, reason: " + cause.ToString());
    }
}