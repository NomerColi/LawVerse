using System.Collections;
using System.Collections.Generic;
using agora_gaming_rtc;
using UnityEngine;
using UnityEngine.UI;
using uWindowCapture;
using Photon.Pun;

public class VideoScreen : ClickableObject
{
    public PhotonView photonView;
    public UwcWindowTexture windowTexture;
    public Texture2D texture2D;

    bool isMine;

    bool sharing;

    // Start is called before the first frame update
    void Start()
    {
        windowTexture.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SharingRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (windowTexture.window == null || windowTexture.window.texture == null)
                continue;

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
            
            SendImage(tex2d.EncodeToJPG(), tex2d.width, tex2d.height, windowTex.format);
        }
    }

    public void SendImage(byte[] bytes, int width, int height, TextureFormat textureFormat)
    {
        photonView.RPC("RPC_SendImage", RpcTarget.Others, bytes, width, height, textureFormat);

        Debug.Log("SendImage bytes length : " + bytes.Length + " width : " + width + " height : " + height + " format : " + textureFormat.ToString());
    }

    byte[] prevBytes = new byte[0];

    [PunRPC]
    public void RPC_SendImage(byte[] bytes, int width, int height, TextureFormat textureFormat)
    {
        if (isMine)
        {
            StopSharing();
        }

        if (prevBytes == bytes)
            return;

        prevBytes = bytes;

        var tex = new Texture2D(width, height, textureFormat, false);
        tex.LoadImage (bytes);
        //Rect rect = new Rect(0, 0, tex.width, tex.height);
        //windowTexture.material_.mainTexture = Sprite.Create (tex, rect, new Vector2() , 100f).texture;
        
        windowTexture.transform.localScale = new Vector3(tex.width / 1000f, tex.height / 1000f, 1);
        windowTexture.material_.mainTexture = tex;

        Debug.Log("RPC_SendImage bytes length : " + bytes.Length + " width : " + width + " height : " + height);
    }

    public void StartSharing()
    {
        this.StopAllCoroutines();
        
        isMine = true;
        sharing = true;

        StartCoroutine(SharingRoutine());
    }

    public void StopSharing()
    {
        windowTexture.enabled = false;

        isMine = false;
        sharing = false;

        this.StopAllCoroutines();
    }

    public override void OnClick()
    {
        Debug.Log("VideOScreen OnClick()");

        windowTexture.enabled = true;

        WorldManager.Instance.OpenVideoScreenPopUp(this);
    }

    public override void Clear()
    {
        this.StopAllCoroutines();
    }
}
