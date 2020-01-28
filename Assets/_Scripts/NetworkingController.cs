using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkingController : MonoBehaviourPunCallbacks
{
    //Variables
    private List <RoomInfo> rooms;
    private string roomName;
    public Camera standby;
    private int inRoom = 0;



    //Photon and unity Functions
    void Start()
    {
        Connect();
    }

    void Connect() //Connect To Photon server via AppID ('UsingSettings')
    {
    	Debug.Log("Connecting to master server");
    	PhotonNetwork.GameVersion = "0.1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
    	Debug.Log("Connected to master server");
    	PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        Debug.Log("InLobby: "+ PhotonNetwork.InLobby.ToString() );
    }

    public override void OnJoinRandomFailed(short returncode, string message) //No rooms are visible or available
    {
    	Debug.Log("Could not join any room, reason: " + returncode.ToString());
    	CreatePhotonRoom();
    }

    void OnCreatePhotonRoomFailed()
    {
        Debug.Log("Failed to create room, trying again");
        CreatePhotonRoom();
    }

    public override void OnRoomListUpdate(List <RoomInfo> roomList)
    {
        rooms = roomList;
    }

    public override void OnJoinedRoom() 
    {
    	Debug.Log("Joined Room");
        inRoom = 1;
        //Debug.Log("InLobby: " + PhotonNetwork.NetworkingClient.InLobby.ToString() );
    	SpawnPerson();

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
    	Debug.Log("Disconnected from server, reason: " + cause.ToString());
    }




    //Other Functions
    void OnGUI()
    {
     

       
        if(inRoom == 0)
        {
            //Button to create room in current lobby
            if(GUI.Button(new Rect (100, 100, 200, 50), "Create Room"))
            {
                CreatePhotonRoom();
            }

             //List all rooms in current lobby with button interraction to join
            int numberOfRooms = 0;
            if(rooms != null)
            {
                foreach(var room in rooms)
                {
                    numberOfRooms += 1;
                }
                if(numberOfRooms != 0)
                {
                    for(int i = 0; i < numberOfRooms; i++)
                    {  
                        if (GUI.Button(new Rect (100, 50 + (50 * (i + 2)), 200, 50), "Join Game" + rooms[i].Name.ToString() ))
                        {
                            Debug.Log("Room: " + rooms[i].ToString() );
                            Debug.Log("Joining: " + rooms[i].Name.ToString() );
                            PhotonNetwork.JoinRoom(rooms[i].Name);
                        }
                    }
                }
            }
        }
    }

    void CreatePhotonRoom()
    {
    	RoomOptions options = new RoomOptions() {IsVisible = true, IsOpen = true, MaxPlayers = 10};
        roomName = "Room " + Random.Range(0,10000);
        PhotonNetwork.CreateRoom(roomName, options);
    	Debug.Log("Created a new Photon Room in Lobby: ");// + PhotonNetwork.lobby.ToString());
    }

    void SpawnPerson() //Spawn person and activating movement script and main camera locally
    {
    	GameObject ClientPerson = PhotonNetwork.Instantiate("Person", Vector3.zero, Quaternion.identity, 0);
    	ClientPerson.GetComponent<Movement>().enabled = true;
    	ClientPerson.transform.Find("Main Camera").gameObject.SetActive(true);
        standby.gameObject.SetActive(false);

    }
}