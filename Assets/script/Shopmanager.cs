using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;

public class CarShop : MonoBehaviour
{
    public List<Cars> carsForSale; // Danh sách các xe có sẵn để mua
    public Button[] buyButtons;   // Các nút mua xe
    public Text playerMoneyText;  // Văn bản UI để hiển thị tiền của người chơi
    public GameObject notEnoughMoneyPopup; // Popup khi không đủ tiền
    public Text moneyText;
    private int playerMoney;

    void Start()
    {
        LoadPlayerData();
        UpdateUI();
        UpdateMoneyDisplay();
        StartCoroutine(SendMoneyToServer());
    }

    void LoadPlayerData()
    {
        playerMoney = PlayerPrefs.GetInt("PlayerMoney", 0);

        foreach (Cars car in carsForSale)
        {
            car.isPurchased = PlayerPrefs.GetInt(car.carName, 0) == 1;
        }
    }

    void SavePlayerData()
    {
        PlayerPrefs.SetInt("PlayerMoney", playerMoney);

        foreach (Cars car in carsForSale)
        {
            PlayerPrefs.SetInt(car.carName, car.isPurchased ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    private IEnumerator ShowNotEnoughMoneyPopup()
    {
        notEnoughMoneyPopup.SetActive(true);
        yield return new WaitForSeconds(3);
        notEnoughMoneyPopup.SetActive(false);
    }

    void UpdateMoneyDisplay()
    {
        moneyText.text = "Money: $" + playerMoney.ToString();
    }

    public void BuyCar(int index)
    {
        Cars car = carsForSale[index];

        if (playerMoney >= car.price)
        {
            playerMoney -= car.price;
            car.isPurchased = true;

            SavePlayerData(); // Lưu lại dữ liệu sau khi mua xe
            UpdateUI(); // Cập nhật giao diện
            UpdateMoneyDisplay(); // Cập nhật hiển thị tiền
            StartCoroutine(SendMoneyToServer()); // Gửi tiền mới lên server sau khi mua xe
        }
        else
        {
            StartCoroutine(ShowNotEnoughMoneyPopup());
        }
    }

    void UpdateUI()
    {
        playerMoneyText.text = "$" + playerMoney.ToString();

        for (int i = 0; i < carsForSale.Count; i++)
        {
            if (carsForSale[i].isPurchased)
            {
                buyButtons[i].interactable = false;
                buyButtons[i].GetComponentInChildren<Text>().text = "Purchased";
            }
            else
            {
                buyButtons[i].interactable = true;
                buyButtons[i].GetComponentInChildren<Text>().text = "Buy ($" + carsForSale[i].price + ")";
            }
        }

        UpdateMoneyDisplay(); // Cập nhật hiển thị tiền sau khi cập nhật UI
    }

    private IEnumerator SendMoneyToServer()
    {
        string playerName = "Hieu333";
        string token = TokenManager.instance.GetToken();
        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("Token is missing. Please log in first.");
            yield break;
        }


        WWWForm form = new WWWForm();
        form.AddField("token", token);
        form.AddField("score", playerMoney);
        form.AddField("playerName", playerName);

        UnityWebRequest www = UnityWebRequest.Post("https://fpl.expvn.com/GetHighscore.php", form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Player money has been successfully updated on the server!");
        }
        else
        {
            Debug.LogError("Error sending player money to server: " + www.error);
        }
    }
}

[System.Serializable]
public class Cars
{
    public string carName;
    public int price;
    public bool isPurchased = false;
}