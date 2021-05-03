using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

using Sfs2X;
using Sfs2X.Logging;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;

public class Chat : MonoBehaviour
{

    public InputField msgField;
    public Text chatText;
    public static bool chatting;

    // Start is called before the first frame update
    void Start()
    {
        SFS.setChat();
        chatText.text = SFS.chatText;
        chatting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (SFS.IsConnected()) {
			SFS.ProcessEvents();
		}
    }

    public void OnSendMessageKeyPress() {
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
			OnSendMessageButtonClick();
	}

    public void OnSendMessageButtonClick() {

		if (msgField.text != "") {
			// Send public message to Room
			SFS.Send(new Sfs2X.Requests.PublicMessageRequest(msgField.text));

			// Reset message field
			msgField.text = "";
		}

		msgField.ActivateInputField();
		msgField.Select();
	}

    public void printUserMessage() {
        chatText.text = SFS.chatText;
	}

    public void GoToGame(){
        chatting = false;
        SceneManager.LoadScene("GameBoard");
    }

    void OnApplicationQuit() {
        ChooseCharacter.RemoveLaunchedSession();
		// Always disconnect before quitting
		SFS.Disconnect();
	}
}
