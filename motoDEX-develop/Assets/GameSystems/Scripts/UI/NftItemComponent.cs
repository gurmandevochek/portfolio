using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using GameSystems.Scripts;
using UI;
using WalletConnectSharp.Core;
using Network = GameSystems.Scripts.Network;
using Types = GameSystems.Scripts.Types;

public class NftItemComponent : MonoBehaviour
{
    [SerializeField] private Types _type;
    [SerializeField] private Image image;
    [SerializeField] private Sprite _availableSprite;
    [SerializeField] private Sprite _defaultSprite;
    [Tooltip("In %")] [SerializeField] private float _opacity = 0.8f;
    [SerializeField] private TMPro.TMP_Text _nameText;
    [SerializeField] public TMPro.TMP_Text _countText;
    [SerializeField] public Button _buyButton;
    [SerializeField] public Button _addButton;
    [SerializeField] public Button _returnButton;
    [SerializeField] private string _openUrl = "https://app.openbisea.com/metaverse";
    [SerializeField] private Button _chooseButton;
    [SerializeField] private GameObject _txStatus;

    public Button _txStatusButton;
    private const int TotalOpacity = 1;
    public int Type => (int) _type;
    private Action<int> _onChoosed;
    public bool _onChoosedAvailable = false;
    private string _tokenId;
    private IDictionary<string, string> _addedTokens;
    public void SetAvailable(bool isAvailable)
    {
        this.
        image.sprite = isAvailable ? _availableSprite : _defaultSprite;
        var color = image.color;
        color.a = isAvailable ? TotalOpacity : _opacity;
        _addButton.gameObject.SetActive(isAvailable);
        _returnButton.gameObject.SetActive(false);
        if (isAvailable && !_onChoosedAvailable)
        {
            image.sprite = !isAvailable ? _availableSprite : _defaultSprite;
            color.a = !isAvailable ? TotalOpacity : _opacity;
        }
        else if (isAvailable && _onChoosedAvailable)
        {
            _addButton.gameObject.SetActive(!isAvailable);
            _returnButton.gameObject.SetActive(isAvailable);
        }
        image.color = color;
        SetName(_type.ToString().Replace("_", " "));
        _countText.gameObject.SetActive(isAvailable);
        //_buyButton.gameObject.SetActive(!isAvailable);
    }


    public void Setup(string name, string count, string tokenId, Action<int> onChoosed)
    {
        AddCount(count);
        SetName(name);
        _onChoosed = onChoosed;
        _tokenId = tokenId;
    }
    
    public void SetupForAdded(string tokenId, Action<int> onChoosed, IDictionary<string, string> addedTokens)
    {
        _onChoosed = onChoosed;
        _tokenId = tokenId;
        _addedTokens = addedTokens;
    }
    public void SetCount(string count)
    {
        _countText.gameObject.SetActive(true);
        _countText.text = count;
    }

    public void SetOnChoosed(Action<int> onChoosed)
    {
        _onChoosed = onChoosed;
    }

    public void AddCount(string count)
    {
        _countText.gameObject.SetActive(true);
        Int32.TryParse(_countText.text, out var currCountIntValue);
        count = (currCountIntValue + int.Parse(count)).ToString();
        _countText.text = count;
    }
    
    private void SetName(string name)
    {
        _nameText.text = name;
    }

    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    private async void OnBuyButtonClick()
    {
        RemoveListeners();
        Debug.Log(Type);
        _txStatusButton = _buyButton;
        _buyButton.gameObject.SetActive(false);
        _txStatus.SetActive(true);
        string response = await WebGLSendContractPurchaseMoto.PurchaseMoto(Contracts.MotoDEXnft, Type.ToString());
        _txStatus.GetComponent<TxStatus>().SetStatus(Chain.InUse, Network.InUse, response);
        Debug.Log(response);
        Debug.Log(response);
    }

    private async void OnNFTAddClick()
    {
        RemoveListeners();
        Debug.Log(Type);
        _txStatusButton = _addButton;
        _addButton.gameObject.SetActive(false);
        _txStatus.SetActive(true);
        string method = "addMoto";
        if (Type > 99)
        {
            method = "addTrack";
        }
        string response = await WebGLSendContractAddNft.AddNFT(Contracts.MotoDEX, _tokenId, method);
        _txStatus.GetComponent<TxStatus>().SetStatus(Chain.InUse, Network.InUse, response);
        Debug.Log("OnNFTAddClick: " + response);
    }
    
    private async void OnReturnButtonClick()
    {
        RemoveListeners();
        Debug.Log(Type);
        _txStatusButton = _returnButton;
        _returnButton.gameObject.SetActive(false);
        _txStatus.SetActive(true);
        string method = "returnMoto";
        if (Type > 99)
        {
            method = "returnTrack";
        }
        string response = await WebGLSendContractReturnNft.ReturnNft(Contracts.MotoDEX, _tokenId, method);
        _txStatus.GetComponent<TxStatus>().SetStatus(Chain.InUse, Network.InUse, response); 
        Debug.Log("OnReturnButtonClick: " + response);
    }
    
    private void OnChooseButtonClick()
    {
        Debug.Log("choosed");
        if (_onChoosedAvailable) _onChoosed?.Invoke(Type);
    }

    public void AddListeners()
    {
        _buyButton.onClick.AddListener(OnBuyButtonClick);
        _chooseButton.onClick.AddListener(OnChooseButtonClick);
        _addButton.onClick.AddListener(OnNFTAddClick);
        _returnButton.onClick.AddListener(OnReturnButtonClick);
    }
    
    public void RemoveListeners()
    {
        _buyButton.onClick.RemoveListener(OnBuyButtonClick);
        _chooseButton.onClick.RemoveListener(OnChooseButtonClick);
        _addButton.onClick.RemoveListener(OnNFTAddClick);
        _returnButton.onClick.RemoveListener(OnReturnButtonClick);
    }
}