using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameSystems.Scripts;
using Newtonsoft.Json;
using QRCoder;
using QRCoder.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WalletConnectSharp.Unity;

namespace UI
{
    public class SelectLevelWindow : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private Button playButton;
        [SerializeField] private List<GameObject> buildings;
        [SerializeField] private int[] scenesIndexes;
        [SerializeField] private int[] playerIndexes;
        [SerializeField] private ChooseNftComponent _chooseNft;
        [SerializeField] private ChooseNftComponent _chooseLevelWallet;
        [SerializeField] private ChooseNftComponent _chooseLevelInvested;
        [SerializeField] private TabsUIHorizontal _nftComponentsTab;
        [SerializeField] private ConnectChain connectChain;

        [SerializeField] private Button retryLoginButton;

        // [SerializeField] private TMPro.TMP_Text _balanceText;
        [SerializeField] private Image _image;
        [SerializeField] private GameObject _tabMenu;
        [SerializeField] private GameObject _loadingSpinner;
        
        private Contract[] _contracts;
        private List<ItemData> _items = new List<ItemData>();
        private int? selectedLevel;
        private int levelStratingId = 100;
        private int? selectedMoto;
        private void Awake()
        {
            InitComponents();
            _tabMenu.SetActive(true);
        }

        private void Start()
        {
            OnRetryLoginClick();
        }

        private void OnEnable()
        {
            Select(0);
            _nftComponentsTab.gameObject.SetActive(false);
            _image.gameObject.SetActive(false);
            WalletConnect.Instance.ConnectionStarted += WalletConnectOnConnectionStarted;
        }

        private void OnDisable()
        {
            WalletConnect.Instance.ConnectionStarted -= WalletConnectOnConnectionStarted;
        }

        private void WalletConnectOnConnectionStarted(object sender, EventArgs e)
        {
            Debug.Log($"WalletConnectOnConnectionStarted sender {sender}  \t {e}");
            var url = WalletConnect.Instance.Session.URI;
            Debug.Log("Connecting to: " + url);
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            UnityQRCode qrCode = new UnityQRCode(qrCodeData);

            // Copy the URL to the clipboard to allow for manual connection in wallet apps that support it
            GUIUtility.systemCopyBuffer = url;

            // Create the QR code as a Texture2D. Note: "pixelsPerModule" means the size of each black-or-white block in the
            // QR code image. For example, a size of 2 will give us a 138x138 image (too small!), while 20 will give us a
            // 1380x1380 image (too big!). Here we'll use a value of 10 which gives us a 690x690 pixel image.
            Texture2D qrCodeAsTexture2D = qrCode.GetGraphic(pixelsPerModule: 10);

            // Change the filtering mode to point (i.e. nearest) rather than the default of linear - we want sharp edges on
            // the blocks, not blurry interpolated edges!
            qrCodeAsTexture2D.filterMode = FilterMode.Point;

            // Convert the texture into a sprite and assign it to our QR code image
            var qrCodeSprite = Sprite.Create(qrCodeAsTexture2D,
                new Rect(0, 0, qrCodeAsTexture2D.width, qrCodeAsTexture2D.height),
                new Vector2(0.5f, 0.5f), 100f);
            _image.sprite = qrCodeSprite;
            _image.gameObject.SetActive(true);
        }


        private void InitComponents()
        {
            dropdown.onValueChanged.AddListener(Select);
            playButton.onClick.AddListener(OnPlay);
            retryLoginButton.onClick.AddListener(OnRetryLoginClick);
        }

        private void OnPlay()
        {
            SceneManager.LoadScene(scenesIndexes[(int) selectedLevel]);
        }

        private void Select(int index)
        {
            DeselectAll();
            buildings[index].SetActive(true);
        }

        private void DeselectAll()
        {
            foreach (var building in buildings)
            {
                building.SetActive(false);
            }
        }

        private void SelectedMoto(int type)
        {
            if (type>5) return;
            _chooseNft.Reselect(type);
            selectedMoto = type;
            SavedPlayer.savedPlayer = playerIndexes[type];
            if (selectedLevel == null) return;
            playButton.transform.gameObject.SetActive(true);
        }

        private void SelectedLevel(int type)
        {
            _chooseLevelWallet.Reselect(type);
            selectedLevel = type - levelStratingId;
            Select((int) selectedLevel);
            if (selectedMoto == null) return;
            playButton.transform.gameObject.SetActive(true);
            
        }

        private async void OnRetryLoginClick()
        {
            connectChain.SetConnectCallback(OnConnected);
            connectChain.Login();

            await UniTask.Delay(4000); // wait 4 sec for connect
            WalletConnect.Instance.OpenDeepLink();
        }
        
        public async void OnRefreshButtonClick()
        {
            await SetupAvailabeContracts();
        }

        private async void OnConnected()
        {
            _tabMenu.SetActive(false);
            _loadingSpinner.SetActive(true);
            
            var balance = await connectChain.ShowBalance();
            await SetupAvailabeContracts();
            
            _loadingSpinner.SetActive(false);
            _nftComponentsTab.gameObject.SetActive(true);
        }

        public async UniTask<string> SetupAvailabeContracts(GameObject nftItemComponent = null)
        {
            int bal = await connectChain.GetBalanceByContract();
            if (bal > 0)
            {
                var responce = await connectChain.GetAllErc721();
                await SetContarts(responce);
            }   
            
            var addedTokens = await connectChain.GetAllTokenIdsAndOwners();
            Debug.Log(addedTokens.Count);
            //TODO add here request all contracts for tracks
            
            _chooseNft.Setup(_contracts, addedTokens, SelectedMoto);
            _chooseLevelInvested.Setup(_contracts);
            _chooseLevelWallet.Setup(_contracts, addedTokens, SelectedLevel);

            nftItemComponent?.GetComponent<NftItemComponent>().AddListeners();
            return null;
        }
        
        private async UniTask<string> SetContarts(string json)
        {
            Debug.Log(json);
            _contracts = JsonConvert.DeserializeObject<Contract[]>(json);
            for (int i = 0; i < _contracts.Length; i++)
            {
                var itemData = await GetRequest(_contracts[i].uri);
                var item = JsonConvert.DeserializeObject<ItemData>(itemData);
                _contracts[i].ItemData = item;
            }
            return null;
        }

        async UniTask<string> GetRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                await webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                if (webRequest.isNetworkError)
                {
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    return string.Empty;
                }
                else
                {
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    return webRequest.downloadHandler.text;
                }
            }
        }
    }
}