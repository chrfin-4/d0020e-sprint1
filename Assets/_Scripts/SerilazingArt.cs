using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;


public class SerilazingArt : MonoBehaviourPunCallbacks
{
    //Variables

    //Other Functions
    public void ExportAssets()
    {
        PhotonView pv = PhotonView.Get(this);
        string path = @"C:\Users\tompa\Pictures\Freddie01.jpg";
        byte[] byteArray = File.ReadAllBytes(path);
        pv.RPC("AssetRPC", RpcTarget.OthersBuffered, byteArray, "Tavla1", "Freddie01.jpg");
    }

    [PunRPC]
    void AssetRPC(byte[] file, string pos, string filename)
    {
        Debug.Log("Här kommer Data!!!");
        string pathRoot = "";
        File.WriteAllBytes(pathRoot + filename, file);
        Sprite MySprite = IMG2Sprite.instance.LoadNewSprite(pathRoot + filename);
        Debug.Log("Sprite Loaded!");
        GameObject sprite = GameObject.Find(pos);
        sprite.GetComponent<SpriteRenderer>().sprite = MySprite;
    }
}