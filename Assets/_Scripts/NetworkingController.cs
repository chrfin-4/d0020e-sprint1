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
    private GameObject personCam;
    public Camera standby;
    public Canvas canvas;
    public GameObject UIButton;
    private GameObject ClientPerson;
    
    //Photon and unity Functions
    void Start()
    {
        setupCanvas();
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
        DisplayRooms();
    }


    //On joined room destroy buttons and instantiates player prefab
    public override void OnJoinedRoom() 
    {
    	Debug.Log("Joined Room");
        inRoom = 1;
    	SpawnPerson();
        if(PhotonNetwork.IsMasterClient)
        {
            RoomSettings room = RoomSettings.GetTestRoomSettings();
            //ClientPerson.GetComponent<SerilazingArt>().ExportAssets();
            ClientPerson.GetComponent<SerilazingArt>().ExportArt(room);
        }
    }
    public override void OnPlayerEnteredRoom(Player newplayer)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            Debug.Log("New Player: " + newplayer.ToString() );
        }
        
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
    	Debug.Log("Disconnected from server, reason: " + cause.ToString());
    }




    //Other Functions

    //Setting up canvas which is necessary for world gui to work, essential for VR
    void setupCanvas()
    {
        canvas.worldCamera = standby;
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
    	ClientPerson = PhotonNetwork.Instantiate("Person", Vector3.zero, Quaternion.identity, 0);
    	ClientPerson.GetComponent<Movement>().enabled = true;
    	personCam = ClientPerson.transform.Find("Main Camera").gameObject;
        personCam.gameObject.SetActive(true);
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
    }

    void JoinRoomButton(string roomID, int posFactor)
    {
        var buttonObject = Instantiate(UIButton);
        Button button = (Button)buttonObject.GetComponent("Button");
        var buttonTextChild = buttonObject.transform.GetChild(0);
        Text buttonTextChildComponent = (Text)buttonTextChild.GetComponent("Text");
        buttonTextChildComponent.text = "Join Room " + roomID;
        button.transform.SetParent(canvas.transform, false);
        button.onClick.AddListener(() => PhotonNetwork.JoinRoom(roomID));
        
        Vector3 pos = new Vector3(80.0f , 30.0f * (posFactor + 2.0f), 0.0f);
        buttonObject.transform.position = pos;
        
        buttons.Add(buttonObject);
    }

    void DisplayRooms()
    {
        foreach(var button in buttons)
        {
            Destroy(button);
        }
        if(inRoom == 0)
        {
            CreateRoomButton("Create Button");
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
                        JoinRoomButton(rooms[i].Name, i);
                    }
                }
            }
        }
    }

    void LeaveRoom()
    {

    }
}
