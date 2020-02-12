using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class IMG2Sprite : MonoBehaviour {


    // This script loads a PNG or JPEG image from disk and returns it as a Sprite
    // Drop it on any GameObject/Camera in your scene (singleton implementation)
    //
    // Usage from any other script:
    // MySprite = IMG2Sprite.instance.LoadNewSprite(FilePath, [PixelsPerUnit (optional)])

    private static IMG2Sprite _instance;

    public static IMG2Sprite instance
    {
        get
        {
            //If _instance hasn't been set yet, we grab it from the scene!
            //This will only happen the first time this reference is used.

            if (_instance == null)
                _instance = GameObject.FindObjectOfType<IMG2Sprite>();
            return _instance;
        }
    }

    public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 200.0f, SpriteMeshType spriteType = SpriteMeshType.FullRect)
    {
        // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference
        Texture2D SpriteTexture = LoadTexture(FilePath);
        Debug.Log("Width: " + SpriteTexture.width.ToString() );
        Debug.Log("Height: " + SpriteTexture.height.ToString() );
        
        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit, 0, spriteType);
        Debug.Log("Sprite Width: " + NewSprite.rect.width.ToString() );
        Debug.Log("Sprite Height: " + NewSprite.rect.height.ToString() );
        return NewSprite;
    }

    public Texture2D LoadTexture(string FilePath)
    {

        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails

        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        Debug.Log("Flie not found");
        return null;                     // Return null if load failed
    }


}