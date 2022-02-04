using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uWindowCapture;

public class ImageTest : MonoBehaviour
{
    public RawImage image;
    public RawImage tempImage;


    public UwcWindowTexture windowTexture;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Routine());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log(BitConverter.ToString(tempBytes));
        }
    }

    byte[] tempBytes = new byte[0];

    IEnumerator Routine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (windowTexture.window.texture == null)
                continue;

            tempImage.texture = windowTexture.window.texture;
            
            var windowTex = windowTexture.window.texture;
            
            // Create a temporary RenderTexture of the same size as the texture
            RenderTexture tmp = RenderTexture.GetTemporary( 
                                windowTex.width,
                                windowTex.height,
                                0,
                                RenderTextureFormat.Default,
                                RenderTextureReadWrite.Linear);


            // Blit the pixels on texture to the RenderTexture
            Graphics.Blit(windowTex, tmp);


            // Backup the currently set RenderTexture
            RenderTexture previous = RenderTexture.active;


            // Set the current RenderTexture to the temporary one we created
            RenderTexture.active = tmp;


            // Create a new readable Texture2D to copy the pixels to it
            Texture2D tex2d = new Texture2D(windowTex.width, windowTex.height, windowTex.format, windowTex.mipmapCount > 1);


            // Copy the pixels from the RenderTexture to the new Texture
            tex2d.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
            tex2d.Apply();

            // Reset the active RenderTexture
            RenderTexture.active = previous;

            // Release the temporary RenderTexture
            RenderTexture.ReleaseTemporary(tmp);
            

            var tex = new Texture2D(tex2d.width, tex2d.height, tex2d.format, tex2d.mipmapCount > 1);
            var bytes = tex2d.EncodeToJPG();

            tempBytes = bytes;

            tex.LoadImage (bytes);
            //tex.LoadRawTextureData(Bytes);
            //tex.Apply();
            
            image.rectTransform.sizeDelta = new Vector2(tex2d.width, tex2d.height);
            image.texture = tex;

            Debug.Log("format : " + tex2d.format.ToString() + " mipmapCount : " + tex2d.mipmapCount + " bytes : " + bytes.Length);
        }
    }

    
}
