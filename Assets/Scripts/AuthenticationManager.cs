using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class AuthenticationManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public Button registerButton;
    public GameObject loadingIndicator;
    public TextMeshProUGUI statusText; // optional

    private void Start()
    {
        if (loadingIndicator != null) loadingIndicator.SetActive(false);
        if (loginButton != null) loginButton.onClick.AddListener(OnLoginClicked);
        if (registerButton != null) registerButton.onClick.AddListener(OnRegisterClicked);
    }

    public void OnLoginClicked()
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowStatus("Please enter both username and password.");
            return;
        }

        string storedPassword = PlayerPrefs.GetString($"user_{username}", "");
        if (storedPassword == password)
        {
            ShowStatus("Login successful!");
            PlayerPrefs.SetString("currentUser", username);
            PlayerPrefs.Save();
            StartCoroutine(GoToLobby());
        }
        else
        {
            ShowStatus("Invalid credentials. Please try again.");
        }
    }

    public void OnRegisterClicked()
    {
        string username = usernameInput.text.Trim();
        string password = passwordInput.text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowStatus("Please enter both username and password.");
            return;
        }

        if (PlayerPrefs.HasKey($"user_{username}"))
        {
            ShowStatus("Username already exists.");
            return;
        }

        PlayerPrefs.SetString($"user_{username}", password);
        PlayerPrefs.Save();

        ShowStatus("Registration successful! You can now log in.");
    }

    private IEnumerator GoToLobby()
    {
        if (loadingIndicator != null) loadingIndicator.SetActive(true);
        yield return new WaitForSeconds(1f);
        if (loadingIndicator != null) loadingIndicator.SetActive(false);

        SceneManager.LoadScene("LobbyScene");
    }

    private void ShowStatus(string message)
    {
        Debug.Log(message);
        if (statusText != null) statusText.text = message;
    }
}
