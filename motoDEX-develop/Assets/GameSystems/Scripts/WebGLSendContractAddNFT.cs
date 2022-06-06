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
public class WebGLSendContractAddNft : MonoBehaviour
{
    static async public UniTask<string> AddNFT(string motoDEXAdress, string args, string method)
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

        string[] listForSerialize = {ownerAccount, _tokenId};
        string tokenIdArgs = JsonConvert.SerializeObject(listForSerialize);
        
        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";

        // connects to user's browser wallet (metamask) to update contract state
        try {
            string[] obj = {motoDEXAdress, _tokenId};
            args = JsonConvert.SerializeObject(obj);
            
            string[] tokenIdsForSerialize = {_tokenId};
            string serializedTokenId = JsonConvert.SerializeObject(tokenIdsForSerialize);
            
            string approve = await Web3GL.SendContract("approve", motoDEXnftAbi, motoDEXnftContract, args, "", gasLimit, gasPrice);
            Debug.Log(approve);
            string minimalFeeInUSD = await ContractHandler.MotodDexCallWithoutSerialize("minimalFeeInUSD", motoDEXAbi, chain, network, motoDEXContract, "[]");
            Debug.Log(minimalFeeInUSD);
            string valueInMainCoin = await ContractHandler.MotodDexCall("valueInMainCoin", motoDEXAbi, chain, network, motoDEXContract, minimalFeeInUSD);
            Debug.Log(valueInMainCoin);
            string response = await Web3GL.SendContract(method, motoDEXAbi, motoDEXContract, serializedTokenId, valueInMainCoin, gasLimit, gasPrice);
            Debug.Log(response);
            return response;
        } catch (Exception e) {
            Debug.LogException(e);
            return "fail";
        }
    }
}
#endif