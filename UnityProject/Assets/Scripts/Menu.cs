using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Cinemachine;
public class Menu : MonoBehaviour
{
    public string tokenGenerationUrl = "https://katanagame.ankoooosh.repl.co/";

    public GameObject MenuUI;
    public GameObject World;

    public GameObject Home;
    public GameObject Login;
    public TMP_InputField TokenInput;
    public GameObject AfterLoggedin;
    public GameObject Shop;

    private SingleProfile LoggedinProfile;
    public TMP_Text UsernameText;

    public Deso deso;

    private string LoggedInKey = "";
    [SerializeField] private TextAsset envFile;

    public CinemachineFreeLook freeLook;

    public GameObject Skin;
    bool MenuVisible = false;
    public void ToggleUI()
    {
        MenuVisible = !MenuVisible;
        MenuUI.SetActive(MenuVisible);
        World.SetActive(!MenuVisible);
    }

    public void GoHome()
    {
        Home.SetActive(true);
        Login.SetActive(false);
        AfterLoggedin.SetActive(false);
        Shop.SetActive(false);
    }

    public void GoLoginScreen()
    {
        Home.SetActive(false);
        Login.SetActive(true);
        AfterLoggedin.SetActive(false);
        Shop.SetActive(false);
    }

    public void GoAfterLogin()
    {
        Home.SetActive(false);
        Login.SetActive(false);
        AfterLoggedin.SetActive(true);
        Shop.SetActive(false);
    }

    public void GoShop()
    {
        Home.SetActive(false);
        Login.SetActive(false);
        AfterLoggedin.SetActive(false);
        Shop.SetActive(true);
    }

    void Start()
    {
        ToggleUI();
        if (PlayerPrefs.HasKey("LastToken") && PlayerPrefs.HasKey("LastLoggedin"))
        {
            string tkn = PlayerPrefs.GetString("LastToken");
            string key = PlayerPrefs.GetString("LastLoggedin");
            string dec = decrypt(tkn);
            if (dec == key)
            {
                TokenInput.text = tkn;
                UserLogin();
            }
            else
            {
                GoHome();
                TokenInput.text = "";
            }
        }
        else
        {
            GoHome();
        }
    }

    bool isFocused = false;
    public void Update()
    {
        // Look
        // if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetMouseButtonDown(0) && !isFocused))
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // if (LoggedInKey.Length == 0)
            // {
            //     return;
            // }
            ToggleUI();
            isFocused = !isFocused;
            Cursor.visible = !isFocused;
            if (isFocused)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
            if (isFocused)
            {
                freeLook.m_XAxis.m_InputAxisName = "Mouse X";
                freeLook.m_YAxis.m_InputAxisName = "Mouse Y";
            }
            else
            {
                freeLook.m_XAxis.m_InputAxisName = "";
                freeLook.m_YAxis.m_InputAxisName = "";


            }
        }
    }

    public void OpenTokenGeneratingWebsite()
    {
        Application.OpenURL(tokenGenerationUrl);
    }

    public string decrypt(string token)
    {
        string key, iv;

        string env = envFile.text;
        key = env.Split('\n')[0];
        iv = env.Split('\n')[1];

        var textEncoder = new UTF8Encoding();
        var aes = Aes.Create("AesManaged");

        aes.Key = textEncoder.GetBytes(key);
        aes.IV = textEncoder.GetBytes(iv);

        var decryptor = aes.CreateDecryptor();
        var cipher = System.Convert.FromBase64String(token);
        var text_bytes = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);

        var text = textEncoder.GetString(text_bytes);

        return text;
    }

    IEnumerator getProfile(string json)
    {
        string url = "https://bitclout.com/api/v0/get-single-profile";
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
            LoggedinProfile = JsonUtility.FromJson<SingleProfile>(JsonString);
            UsernameText.text = LoggedinProfile.Profile.Username;
        }
    }

    public void UserLogin()
    {
        string token = TokenInput.text;
        string dec = decrypt(token);

        // Debug.Log(dec);
        LoggedInKey = dec;
        PlayerPrefs.SetString("LastToken", token);
        PlayerPrefs.SetString("LastLoggedin", LoggedInKey);

        deso.KeyOwnsNFTs();

        // call deso apis to get profile info
        StartCoroutine(getProfile("{\"NoErrorOnMissing\":false,\"PublicKeyBase58Check\":\"" + LoggedInKey + "\"}"));
        GoAfterLogin();
    }

    public void UserLogout()
    {
        PlayerPrefs.DeleteKey("LastToken");
        PlayerPrefs.DeleteKey("LastLoggedin");
        TokenInput.text = "";
        LoggedInKey = "";
        GoHome();

    }

    public void ApplySkin(GameObject prf)
    {
        // delete existing childs
        while (Skin.transform.childCount > 0)
        {
            DestroyImmediate(Skin.transform.GetChild(0).gameObject);
        }
        // spawn a new child from prefab
        var instantiatedSkin = Instantiate(prf, Skin.transform);

    }

    public void ExitApp()
    {
        Application.Quit();
    }
}