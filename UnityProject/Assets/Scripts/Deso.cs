using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Deso : MonoBehaviour
{
    public Menu menu;
    public List<string> nftHashList = new List<string>();
    public List<Button> btnList = new List<Button>();

    public void openNFTpage(GameObject self)
    {
        int pos = self.transform.GetSiblingIndex();
        Application.OpenURL("https://bitclout.com/nft/" + nftHashList[pos - 1]);
    }
    public void KeyOwnsNFTs()
    {
        IEnumerator getNFT(string publicKey, string NFThash)
        {
            string json = "{\"ReaderPublicKeyBase58Check\":\"" + publicKey + "\",\"PostHashHex\":\"" + NFThash + "\"}";
            string url = "https://diamondapp.com/api/v0/get-nft-entries-for-nft-post";
            var uwr = new UnityWebRequest(url, "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");

            //Send the request then wait here until it returns
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Error While Sending: " + uwr.error);
            }
            else
            {
                string JsonString = uwr.downloadHandler.text;
                // converting this huge json string into C# class seems complicated, so I'll just check if the string contains the logged in public key
                // which should return true if public key owns the nft
                if (JsonString.Contains(publicKey))
                {
                    Debug.Log(NFThash + " owned :O");
                    btnList[nftHashList.IndexOf(NFThash)].interactable = true;
                }
                else
                {
                    btnList[nftHashList.IndexOf(NFThash)].interactable = false;
                }
            }
            yield break;

        }
        string publickey = "";
        if (PlayerPrefs.HasKey("LastToken") && PlayerPrefs.HasKey("LastLoggedin"))
        {
            string tkn = PlayerPrefs.GetString("LastToken");
            string key = PlayerPrefs.GetString("LastLoggedin");
            string dec = menu.decrypt(tkn);
            if (dec == key) { publickey = key; }
            else { return; }
        }
        else { return; }
        for (int i = 0; i < nftHashList.Count; i++)
        {
            string NFThash = nftHashList[i];
            StartCoroutine(getNFT(publickey, NFThash));
        }
    }
}
