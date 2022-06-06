using System;
using System.Collections.Generic;
using System.Linq;
using GameSystems.Scripts;
using UnityEngine;

namespace UI
{
    public class ChooseNftComponent : MonoBehaviour
    {
        [SerializeField] private NftItemComponent[] _nftItemComponents;

        public void Setup(Contract[] items, IDictionary<string, string> _addedTokens = null, Action<int> onChoosed = null)
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                nftItemComponent._onChoosedAvailable = false;
                nftItemComponent.SetCount("0");
                nftItemComponent.SetAvailable(false);
                nftItemComponent.SetOnChoosed(onChoosed);
                
                if ( _addedTokens != null && _addedTokens.ContainsKey(nftItemComponent.Type.ToString()))
                {
                    nftItemComponent._onChoosedAvailable = true;
                    nftItemComponent.SetupForAdded(_addedTokens[nftItemComponent.Type.ToString()], onChoosed, _addedTokens);
                    nftItemComponent.SetAvailable(true);
                }
            }

            for (int i = 0; i < items?.Length; i++)
            {
                foreach (var nftItemComponent in _nftItemComponents)
                {
                    if ( nftItemComponent.Type == items[i].ItemData?.type)
                    {
                        if (_addedTokens != null && _addedTokens.ContainsKey(nftItemComponent.Type.ToString()))
                        {
                            nftItemComponent._onChoosedAvailable = true;
                            nftItemComponent.Setup(items[i].ItemData.name.ToUpper(), items[i].balance, items[i].tokenId, onChoosed);
                            nftItemComponent.SetupForAdded(_addedTokens[nftItemComponent.Type.ToString()], onChoosed, _addedTokens);
                        }
                        else
                        {
                            nftItemComponent._onChoosedAvailable = false;
                            nftItemComponent.Setup(items[i].ItemData.name.ToUpper(), items[i].balance, items[i].tokenId, onChoosed);
                        }
                        nftItemComponent.SetAvailable(true);
                    }           
                }
            }
        }

        public void Reselect(int type)
        {
            foreach (var nftItemComponent in _nftItemComponents)
            {
                if (nftItemComponent.Type == type)
                {
                    nftItemComponent.transform.Find("Checkmark").gameObject.SetActive(true);
                }
                else
                {
                    nftItemComponent.transform.Find("Checkmark").gameObject.SetActive(false);
                }
            }
        }
        
        

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}