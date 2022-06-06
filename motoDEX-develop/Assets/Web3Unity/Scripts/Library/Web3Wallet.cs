using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Web3Wallet
{
    #if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        private static string url = "https://metamask.app.link/dapp/chainsafe.github.io/game-web3wallet/";
    #else
        private static string url = "https://chainsafe.github.io/game-web3wallet/";
    #endif

    public static async UniTask<string> SendTransaction(string _chainId, string _to, string _value, string _data = "", string _gasLimit = "", string _gasPrice = "")
    {
        // open application
        Application.OpenURL(url + "?action=send" + "&chainId=" + _chainId + "&to=" + _to + "&value=" + _value + "&data=" + _data + "&gasLimit=" + _gasLimit + "&gasPrice=" + _gasPrice);
        // set clipboard to empty
        GUIUtility.systemCopyBuffer = "";
        // wait for clipboard response
        string clipBoard = "";
        while (clipBoard == "")
        {
            #if UNITY_WEBGL && !UNITY_EDITOR
                clipBoard = InputFieldDataCollection.inputFieldData();
            #else
                clipBoard = GUIUtility.systemCopyBuffer;
            #endif
            
            await UniTask.Delay(100);
        }
        // check if clipboard response is valid
        if (clipBoard.StartsWith("0x") && clipBoard.Length == 66)
        {
            return clipBoard;
        }
        else
        {
            throw new Exception("transaction error");
        }
    }

    public static async UniTask<string> Sign(string _message)
    {
        // open application
        string message = Uri.EscapeDataString(_message);
        Application.OpenURL(url + "?action=sign" + "&message=" + message);
        // set clipboard to empty
        GUIUtility.systemCopyBuffer = "";
        // wait for clipboard response
        string clipBoard = "";
        while (clipBoard == "")
        {
            #if UNITY_WEBGL && !UNITY_EDITOR
                clipBoard = InputFieldDataCollection.inputFieldData();
            #else
                clipBoard = GUIUtility.systemCopyBuffer;
            #endif
            
            await UniTask.Delay(100);
        }
        // check if clipboard response is valid
        if (clipBoard.StartsWith("0x") && clipBoard.Length == 132)
        {
            return clipBoard;
        }
        else
        {
            throw new Exception("sign error");
        }   
    }
}
