using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class taikhoan : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField password;
    public TextMeshProUGUI thongbao;

    public void DangnhapButton()
    {
        StartCoroutine(DangNhap());
    }

    IEnumerator DangNhap()
    {
        WWWForm form = new WWWForm();
        form.AddField("user", username.text);
        form.AddField("passwd", password.text);
        UnityWebRequest www = UnityWebRequest.Post("https://fpl.expvn.com/dangnhap.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            thongbao.text = "Ket noi khong thanh cong";
            Debug.Log(www.error);
        }
        else
        {
            string get = www.downloadHandler.text;
            Debug.Log("Server Response: " + get);

            if (get == "empty")
            {
                thongbao.text = "Khong duoc de trong";
            }
            else if (string.IsNullOrEmpty(get))
            {
                thongbao.text = "Tai khoan hoac mat khau khong chinh xac";
            }
            else if (get.Contains("Loi"))
            {
                thongbao.text = "Khong ket noi duoc toi server";
            }
            else
            {
                thongbao.text = "Dang nhap thanh cong";
                PlayerPrefs.SetString("token", get);
                Debug.Log(get);
                DangNhapThanhCong();
            }
        }
    }

    public void DangNhapThanhCong()
    {
        // Chuyển đổi sang scene tiếp theo (ví dụ: "Gamemenu")
        SceneManager.LoadScene("Gamemenu");
    }
}