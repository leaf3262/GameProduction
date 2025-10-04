using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class PurchaseManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI balanceText;
    public TMP_InputField amountInput;
    public Button topUpButton;

    [Header("Backend Settings")]
    public string backendBaseUrl = "http://localhost:5000";

    private string jwtToken = "";

    private void Start()
    {
        if (topUpButton != null) topUpButton.onClick.AddListener(OnTopUpClicked);
        jwtToken = PlayerPrefs.GetString("jwt", "");
        StartCoroutine(FetchBalance());
    }

    public IEnumerator FetchBalance()
    {
        var url = $"{backendBaseUrl}/wallet/balance";
        using var req = UnityWebRequest.Get(url);

        if (!string.IsNullOrEmpty(jwtToken))
            req.SetRequestHeader("Authorization", $"Bearer {jwtToken}");

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            if (balanceText != null)
                balanceText.text = "Credits: " + req.downloadHandler.text;
        }
        else
        {
            Debug.LogError("FetchBalance error: " + req.error);
        }
    }

    private void OnTopUpClicked()
    {
        int amount = 100;
        if (amountInput != null && int.TryParse(amountInput.text, out int parsed))
        {
            amount = parsed;
        }
        StartCoroutine(StartTopUp(amount));
    }

    private IEnumerator StartTopUp(int amount)
    {
        var url = $"{backendBaseUrl}/wallet/topup";
        WWWForm form = new WWWForm();
        form.AddField("amount", amount);

        using var req = UnityWebRequest.Post(url, form);

        if (!string.IsNullOrEmpty(jwtToken))
            req.SetRequestHeader("Authorization", $"Bearer {jwtToken}");

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            var checkoutUrl = req.downloadHandler.text.Trim('"');
            Application.OpenURL(checkoutUrl);
        }
        else
        {
            Debug.LogError("TopUp error: " + req.error);
        }
    }
}
