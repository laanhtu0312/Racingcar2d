using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class quanlydangky : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField password;
    public TextMeshProUGUI thongbao;
    public void Dangkybutton()
    {

        StartCoroutine(Dangky());
    }

    private IEnumerator Dangky()
    {
        WWWForm form = new WWWForm();
        form.AddField("user", username.text);
        form.AddField("passwd", password.text);

        UnityWebRequest www = UnityWebRequest.Post("https://fpl.expvn.com/dangky.php", form);
        yield return www.SendWebRequest();

        if (!www.isDone)
        {
            thongbao.text = "Ket noi khong thanh cong";
        }
        else
        {
            string get = www.downloadHandler.text;
            switch (get)
            {
                case "exist":
                    thongbao.text = "tai khoan da ton tai";
                    break;
                case "OK":
                    thongbao.text = "dang ky thanh cong";
                    break;
                case "ERROR":
                    thongbao.text = "Dang ky khong thanh cong";
                    break;
                default:
                    thongbao.text = "Khong ket noi duoc sever";
                    break;
            }
        }
    }
}
