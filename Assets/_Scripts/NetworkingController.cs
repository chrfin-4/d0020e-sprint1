using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkingController : MonoBehaviourPunCallbacks
{
    //Variables
    private List <RoomInfo> rooms;
    private string roomName;
    private int inRoom = 0;
    private List <GameObject> buttons = new List<GameObject>();
    public Camera standby;
    public Canvas canvas;
    public GameObject UIButton;
    
    //Photon and unity Functions
    void Start()
    {
        CreateRoomButton("Create Button");
        Connect();
    }

    void Update()
    {
        //DisplayRooms();
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
        DisplayRooms();
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
    
    void CreateRoomButton(string buttonText)
    {
        GameObject buttonObject = Instantiate(UIButton);
        Button button = (Button)buttonObject.GetComponent("Button");
        var buttonTextChild = buttonObject.transform.GetChild(0);
        Text buttonTextChildComponent = (Text)buttonTextChild.GetComponent("Text");
        buttonTextChildComponent.text = "Create Room";
        buttonObject.transform.SetParent(canvas.transform, false);
        button.onClick.AddListener(() => CreatePhotonRoom());


        buttons.Add(buttonObject);
        Debug.Log("Buttons: " + buttons.ToString() );
    }

    void JoinRoomButton(string roomID)
    {
        var buttonObject = Instantiate(UIButton);
        Button button = (Button)buttonObject.GetComponent("Button");
        var buttonTextChild = buttonObject.transform.GetChild(0);
        Text buttonTextChildComponent = (Text)buttonTextChild.GetComponent("Text");
        buttonTextChildComponent.text = "Join Room " + roomID;
        button.transform.SetParent(canvas.transform, false);
        button.onClick.AddListener(() => PhotonNetwork.JoinRoom(roomID));

        buttons.Add(buttonObject);
        Debug.Log("Buttons: " + buttons.ToString() );
    }

    void DisplayRooms()
    {
        if(inRoom == 0)
        {
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
                        JoinRoomButton(rooms[i].Name);
                    }
                }
            }
        }
    }
}