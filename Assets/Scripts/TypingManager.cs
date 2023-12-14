using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypingManager : MonoBehaviour
{
    [SerializeField]
    EmailMainPage documentHandler;

    int currentCharacter;

    string textToType;

    string typedSoFar;



    void Start()
    {
        GameEventSystem.instance.onReplyButtonTrigger += UpdateCurrentString;
    }

    void Update()
    {
        if (documentHandler == null)
        {
            Debug.Log("No Document handler");
            return;
        }
        if (documentHandler.currentEmail == null) 
        {
            return;
        }
        if (documentHandler.OnReplyScreen == false)
        {
            return;
        }
        if (currentCharacter >= textToType.Length)
        {
            return;
        }
        string input = Input.inputString;

        if (input == "")
        {
            return;
        }

        if (input == "\r" || input == "\n")
        {
            input = "\n";
        }

        char typedChar = input[0];

        if (typedChar == textToType[currentCharacter])
        {
            currentCharacter++;

            documentHandler.AddLetterToReply(typedChar);

            if (currentCharacter >= textToType.Length)
            {
                SignalReadyToSend();
            }
           
        }

    }

    //string InputLetter()
    //{
    //    string input = Input.inputString;
    //    if (input.Equals(""))
    //    {
    //        return "";
    //    }

    //    return input.Substring(0, 1);
    //}

    void UpdateCurrentString()
    {
        if (documentHandler == null)
        {
            Debug.Log("No Document handler when trying to update current string");
            return;
        }
        if (documentHandler.currentEmail == null)
        {
            return;
        }
        textToType = documentHandler.currentEmail.reply.EmailBody;
        currentCharacter = 0;
    }

    //bool letterMatch(char inputChar)
    //{
    //    if (inputChar == textToType[currentCharacter])
    //    {
    //        currentCharacter++;

    //        documentHandler.AddLetterToReply(inputChar);

    //        if (currentCharacter >= textToType.Length)
    //        {
    //            SignalReadyToSend();
    //        }
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    void SignalReadyToSend()
    {
        GameEventSystem.instance.ReadyToSendTrigger();
    }

}
