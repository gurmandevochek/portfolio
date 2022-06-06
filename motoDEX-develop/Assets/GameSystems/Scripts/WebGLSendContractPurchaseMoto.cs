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
public class WebGLSendContractPurchaseMoto : MonoBehaviour
{
    static async public UniTask<string> PurchaseMoto(string contract, string args)
    {
        string chain = Chain.InUse;
        string network = Network.InUse;
        string _tokenId = args;
        
        string[] obj = {args, "0x0000000000000000000000000000000000000000"};
        args = JsonConvert.SerializeObject(obj);
        
        // abi in json format
        string abi = Abis.MotoDEXnftABI;
        // gas limit OPTIONAL
        string gasLimit = "";
        // gas price OPTIONAL
        string gasPrice = "";
        // connects to user's browser wallet (metamask) to update contract state
        string method;
        try {
            string valueInMainCoin = await ContractHandler.MotodDexCall(method="valueInMainCoin", abi, chain, network, contract, _tokenId);
            Debug.Log(valueInMainCoin);
            string response = await Web3GL.SendContract(method="purchase", abi, contract, args, valueInMainCoin, gasLimit, gasPrice);
            Debug.Log(response);
            return response;
        } catch (Exception e) {
            Debug.LogException(e);
            return "fail";
        }
    }
}
#endif