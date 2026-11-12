using System.Net;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionUI : MonoBehaviour
{
    public TMP_InputField ipInputField;
    public Button hostButton;
    public Button clientButton;
    public Button quitButton;

    public GameObject canvasHUD;

    private void Start()
    {
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);
        quitButton.onClick.AddListener(QuitGame);

        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
    }

    private void StartHost()
    {
        string ipLimpia = ipInputField.text.Trim();

        if (string.IsNullOrEmpty(ipLimpia) || !IPAddress.TryParse(ipLimpia, out _))
        {
            ipLimpia = "0.0.0.0";
            Debug.Log("HOST: Cuadro vacío o texto inválido. Escuchando en todas las redes (0.0.0.0).");
        }

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetConnectionData(ipLimpia, 7777, ipLimpia);

        NetworkManager.Singleton.StartHost();

        gameObject.SetActive(false);
        canvasHUD.SetActive(true);
    }

    private void StartClient()
    {
        string ipLimpia = ipInputField.text.Trim();

        if (string.IsNullOrEmpty(ipLimpia) || !IPAddress.TryParse(ipLimpia, out _))
        {
            ipLimpia = "127.0.0.1";
            Debug.Log("CLIENTE: Cuadro vacío o texto inválido. Conectando a Localhost (127.0.0.1).");
        }

        clientButton.interactable = false;

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetConnectionData(ipLimpia, 7777);

        NetworkManager.Singleton.StartClient();
    }

    private void HandleClientConnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            gameObject.SetActive(false);
            canvasHUD.SetActive(true);
            clientButton.interactable = true; 
        }
    }


    private void HandleClientDisconnect(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            Debug.LogWarning("Fallo al conectar. Revisa la IP, Hamachi o el Firewall del Host.");

            gameObject.SetActive(true);
            canvasHUD.SetActive(false);
            clientButton.interactable = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
        }
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}