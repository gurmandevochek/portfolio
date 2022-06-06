using System;
using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameSystems.Scripts;
using Newtonsoft.Json;
using UnityEngine;
using Network = GameSystems.Scripts.Network;

public class AddedCheck : MonoBehaviour
{
    public static async UniTask<IDictionary<string, string>> NFTAddedCheck(string account)
    {
        List<string> addedTokensTokenId = new List<string> {};
        List<string> addedTokensType = new List<string> {};
        IDictionary<string, string> addedTokens = new Dictionary<string, string>();
        string ownerAccount = account;
        string motoDEXnftAbi = Abis.MotoDEXnftABI;
        string motoDEXAbi = Abis.MotoDEXABI;
        string chain = Chain.InUse;
        string network = Network.InUse;
        string motoDEXnftContract = Contracts.MotoDEXnft;
        string motoDEXContract = Contracts.MotoDEX;
        try
        {
            string tokenIdsAndOwners = await ContractHandler.MotodDexCallWithoutSerialize("tokenIdsAndOwners", motoDEXAbi, chain, network, motoDEXContract, "[]");
            Debug.Log(tokenIdsAndOwners);
            var _tokenIdsAndOwners = JsonConvert.DeserializeObject<TokenIdsAndOwners>(tokenIdsAndOwners);
            for (int i = 0; i < _tokenIdsAndOwners.motoIds.Length; i++)
            {
                if (_tokenIdsAndOwners.motoIdsOwners[i].ToUpper() == ownerAccount.ToUpper())
                {
                    addedTokensTokenId.Add(_tokenIdsAndOwners.motoIds[i]);
                }
            }
            for (int i = 0; i < _tokenIdsAndOwners.trackIds.Length; i++)
            {
                if (_tokenIdsAndOwners.trackIdsOwners[i].ToUpper() == ownerAccount.ToUpper())
                {
                    addedTokensTokenId.Add(_tokenIdsAndOwners.trackIds[i]);
                }
            }

            for (int i = 0; i < addedTokensTokenId.Count; i++)
            {
                addedTokensType.Add(await ContractHandler.MotodDexCall("getTypeForId", motoDEXnftAbi, chain, network, motoDEXnftContract, addedTokensTokenId[i]));
            }

            for (int i = 0; i < addedTokensType.Count; i++)
            {
                addedTokens.Add(new KeyValuePair<string, string>(addedTokensType[i], addedTokensTokenId[i]));
            }

            return addedTokens;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return addedTokens;
        }
    }

}
