using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    public Menu menuObj;
    public GameObject miku39;
    private string[] cheatCode;
    private int index;
    // Start is called before the first frame update
    void Start()
    {
        cheatCode = new string[] { "m", "i", "k", "u", "3", "9" };
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if any key is pressed
        if (Input.anyKeyDown)
        {
            // Check if the next key in the code is pressed
            if (Input.GetKeyDown(cheatCode[index]))
            {
                // Add 1 to index to check the next key in the code
                index++;
            }
            // Wrong key entered, we reset code typing
            else
            {
                index = 0;
            }
        }

        // If index reaches the length of the cheatCode string, 
        // the entire code was correctly entered
        if (index == cheatCode.Length)
        {
            // Cheat code successfully inputted!
            // Unlock crazy cheat code stuff
            Debug.Log("MIKU39 Activated");
            menuObj.GoAfterLogin();
            menuObj.ApplySkin(miku39);
            index = 0;
        }
    }
}
