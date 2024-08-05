using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenManager : MonoBehaviour
{
    public static TokenManager instance;

    private string token;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveToken(string userToken)
    {
        token = userToken;
        PlayerPrefs.SetString("token", token);
        PlayerPrefs.Save();
    }

    public string GetToken()
    {
        if (string.IsNullOrEmpty(token))
        {
            token = PlayerPrefs.GetString("token", "");
        }
        return token;
    }

    public void ClearToken()
    {
        token = null;
        PlayerPrefs.DeleteKey("token");
    }
}