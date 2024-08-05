using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class RaceResultManager : MonoBehaviour
{
    public static RaceResultManager instance;

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

    public void SaveRaceResult(int position, float raceTime, int moneyEarned)
    {
        // Lưu vị trí về đích
        PlayerPrefs.SetInt("PlayerPosition", position);

        // Lưu thời gian đua
        PlayerPrefs.SetFloat("PlayerRaceTime", raceTime);

        // Lưu và cộng dồn số tiền kiếm được
        int currentMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
        currentMoney += moneyEarned;
        PlayerPrefs.SetInt("PlayerMoney", currentMoney);

        PlayerPrefs.Save();
    }

    public int GetPlayerPosition()
    {
        return PlayerPrefs.GetInt("PlayerPosition", 0);
    }

    public float GetPlayerRaceTime()
    {
        return PlayerPrefs.GetFloat("PlayerRaceTime", 0f);
    }

    public int GetPlayerMoney()
    {
        return PlayerPrefs.GetInt("PlayerMoney", 0);
    }

    public void SaveRaceResultToFile()
    {
        string DulieuGame = Application.persistentDataPath + "/RaceResult.txt";

        int position = GetPlayerPosition();
        float raceTime = GetPlayerRaceTime();
        int moneyEarned = GetPlayerMoney();

        string resultText = $"Position: {position}\nRace Time: {raceTime}\nMoney Earned: {moneyEarned}";

        File.WriteAllText(DulieuGame, resultText);
        Debug.Log($"Dữ liệu đã được lưu vào file: {DulieuGame}");
        string filePath = Application.persistentDataPath + "/RaceResult.txt";
        Debug.Log("Đường dẫn file: " + filePath);
    }

    public void SendRaceResultToDatabase()
    {
        string playerName = "Hieu333"; 
        int position = GetPlayerPosition();
        float raceTime = GetPlayerRaceTime();
        int moneyEarned = GetPlayerMoney();

        // Lấy token từ PlayerPrefs
        string token = PlayerPrefs.GetString("token", "");

        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("Token is missing. Please log in first.");
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("score", moneyEarned);
        form.AddField("token", token);
        form.AddField("playerName", playerName);


        StartCoroutine(PostRequest(form));
    }

    private IEnumerator PostRequest(WWWForm form)
    {
        UnityWebRequest www = UnityWebRequest.Post("https://fpl.expvn.com/InsertHighscore.php", form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Dữ liệu đã được lưu lên cơ sở dữ liệu!");
        }
        else
        {
            Debug.LogError("Lỗi khi gửi dữ liệu: " + www.error);
        }
    }
}