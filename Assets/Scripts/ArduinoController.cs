using UnityEngine;
using TMPro;
using BlueUnity;

public class ArduinoController : MonoBehaviour
{
    public TMP_InputField inputFieldPalavra;
    public TextMeshProUGUI statusText;

    void Start()
    {
        if (statusText != null)
            statusText.text = "Aguardando Bluetooth...";
    }

    public void EnviarTextoAoDigitar()
    {
        if (inputFieldPalavra != null && BluetoothHandler.Instance != null)
        {
            string mensagem = inputFieldPalavra.text + "\n";
            byte[] dadosEmBytes = System.Text.Encoding.UTF8.GetBytes(mensagem);
            BluetoothHandler.Instance.Write(dadosEmBytes);
        }
    }

    public void FeedbackAcerto()
    {
        EnviarComandoEspecial("#CERTO\n");
        if (statusText != null) statusText.text = "Acertou! LED Verde";
    }

    public void FeedbackErro()
    {
        EnviarComandoEspecial("*ERRADO\n");
        if (statusText != null) statusText.text = "Errado! LED Vermelho";
    }

    private void EnviarComandoEspecial(string comando)
    {
        if (BluetoothHandler.Instance != null)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(comando);
            BluetoothHandler.Instance.Write(bytes);
        }
    }
}