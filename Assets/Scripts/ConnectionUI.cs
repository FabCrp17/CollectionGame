using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ConnectionUI : MonoBehaviour
{
    public TMP_InputField ipInputField;
    public Button hostButton;
    public Button clientButton;

    public GameObject canvasHUD;

    private void Start()
    {
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);
    }

    private void StartHost()
    {
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = "127.0.0.1";

        NetworkManager.Singleton.StartHost();

        gameObject.SetActive(false);
        canvasHUD.SetActive(true);

    }

    private void StartClient()
    {
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        string ip = string.IsNullOrEmpty(ipInputField.text) ? "127.0.0.1" : ipInputField.text;
        transport.ConnectionData.Address = ip;

        NetworkManager.Singleton.StartClient();

        gameObject.SetActive(false);
        canvasHUD.SetActive(true);
    }
}