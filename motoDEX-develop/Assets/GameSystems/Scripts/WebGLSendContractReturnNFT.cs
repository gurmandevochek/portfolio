using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameSystems.Scripts;
using Newtonsoft.Json;
using UnityEngine;
using Network = GameSystems.Scripts.Network;


#if UNITY_WEBGL
public class WebGLSendContractReturnNft : MonoBehaviour
{
    static async public UniTask<string> ReturnNft(string motoDEXAdress, string args, string method)
    {
        string ownerAccount = "";
        string chain = Chain.InUse;
        string network = Network.InUse;
        string _tokenId = args;
        string motoDEXnftContract = Contracts.MotoDEXnft;
        string motoDEXContract = Contracts.MotoDEX;

        string motoDEXnftAbi = Abis.MotoDEXnftABI;
        string motoDEXAbi = Abis.MotoDEXABI;

        if (PlayerPrefs.HasKey("Account"))
        {
            ownerAccount = PlayerPrefs.GetString("Account");
            Debug.Log(ownerAccount);
        }

        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";

        // connects to user's browser wallet (metamask) to update contract state
        try {
            string[] tokenIdsForSerialize = {_tokenId};
            string serializedTokenId = JsonConvert.SerializeObject(tokenIdsForSerialize);
            
            string response = await Web3GL.SendContract(method, motoDEXAbi, motoDEXContract, serializedTokenId, "", gasLimit, gasPrice);
            Debug.Log(response);
            return response;
        } catch (Exception e) {
            Debug.LogException(e);
            return "fail";
        }
    }
}
#endif