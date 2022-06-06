using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

public class TxStatus: MonoBehaviour
{
    [SerializeField] private GameObject _addCountAnimation;
    [SerializeField] private GameObject _text;
    [SerializeField] private GameObject _loadingSpinner;
    [SerializeField] private GameObject _success;
    [SerializeField] private GameObject _fail;
    
    
    private string _chain;
    private string _network;
    private string _transaction;
    private string addCount;
    private string txStatus;

    private void OnEnable()
    {
        _transaction = "";
        InvokeRepeating("CheckStatus", 10.0f, 10.0f);
    }

    private void OnDisable()
    {
        _text.gameObject.SetActive(true);
        _loadingSpinner.gameObject.SetActive(true);
        
        if (!(txStatus == "success" && gameObject.GetComponentInParent<NftItemComponent>()._txStatusButton ==
            gameObject.GetComponentInParent<NftItemComponent>()._addButton))
        {
            gameObject.GetComponentInParent<NftItemComponent>()._txStatusButton.gameObject.SetActive(true);
        }
        if (txStatus == "success")
        {
            gameObject.GetComponentInParent<NftItemComponent>().SetAvailable(true);
        }
        if (txStatus == "success" && gameObject.GetComponentInParent<NftItemComponent>()._txStatusButton ==
            gameObject.GetComponentInParent<NftItemComponent>()._buyButton)
        {
            var selectLevelWindow = GameObject.Find("SelectWindow");
            selectLevelWindow.GetComponent<SelectLevelWindow>().SetupAvailabeContracts(gameObject.transform.parent.gameObject);
        }
        else
        {
            gameObject.GetComponentInParent<NftItemComponent>().AddListeners();  
        }
    }

    private async void CheckStatus()
    {
        Debug.Log(_transaction);
        if (_transaction == "fail")
        {
            txStatus = "fail";
        }
        else
        {
            txStatus = await EVM.TxStatus(_chain, _network, _transaction);
        }
        Debug.Log(txStatus + "TxStatus"); // success, fail, pending
        if (txStatus == "success" || txStatus == "fail")
        {
            _text.gameObject.SetActive(false);
            _loadingSpinner.gameObject.SetActive(false);
            
            if (txStatus == "success")
            {
                _success.gameObject.SetActive(true);
                if (gameObject.GetComponentInParent<NftItemComponent>()._txStatusButton ==
                    gameObject.GetComponentInParent<NftItemComponent>()._addButton)
                {
                    addCount = "-1";
                }
                else
                {
                    addCount = "+1";
                }
                gameObject.GetComponentInParent<NftItemComponent>()._countText.gameObject.SetActive(true);
                _addCountAnimation.gameObject.GetComponent<TMPro.TMP_Text>().text = addCount;
                _addCountAnimation.gameObject.SetActive(true);
                CancelInvoke();

            }
            else
            {
                _fail.gameObject.SetActive(true);
                CancelInvoke();
            }
        }
    }
    
    public void ToDoAfterAddCountAnimation()
    {
        gameObject.GetComponentInParent<NftItemComponent>().AddCount(addCount);
        if (gameObject.GetComponentInParent<NftItemComponent>()._txStatusButton ==
            gameObject.GetComponentInParent<NftItemComponent>()._addButton)
        {
            gameObject.GetComponentInParent<NftItemComponent>()._onChoosedAvailable = true;  
        }
        else if (gameObject.GetComponentInParent<NftItemComponent>()._txStatusButton ==
                 gameObject.GetComponentInParent<NftItemComponent>()._returnButton)
        {
            gameObject.GetComponentInParent<NftItemComponent>()._onChoosedAvailable = false;
        }
        _success.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    
    public void ToDoAfterFailAnimation()
    {
        _fail.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void SetStatus(string chain, string network, string transaction)
    {
        _chain = chain;
        _network = network;
        _transaction = transaction;
    }
}
