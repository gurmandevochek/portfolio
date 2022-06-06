using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class ContractHandler
{
    public static async UniTask<string> MotodDexCall(string method, string abi, string _chain, string _network, string _contract, string _tokenId, string _rpc="")
    {
        string[] obj = { _tokenId };
        string args = JsonConvert.SerializeObject(obj);
        string response = await EVM.Call(_chain, _network, _contract, abi, method, args, _rpc);
        try
        {
            return response;
        } 
        catch 
        {
            Debug.LogError(response);
            throw;
        }
    }

    public static async UniTask<string> MotodDexCallWithoutSerialize(string method, string abi, string _chain, string _network, string _contract, string _tokenId, string _rpc = "")
    {
        string args = _tokenId;
        string response = await EVM.Call(_chain, _network, _contract, abi, method, args, _rpc);
        try
        {
            return response;
        }
        catch
        {
            Debug.LogError(response);
            throw;
        }
    }
}
