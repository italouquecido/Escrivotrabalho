using UnityEngine;
using UnityEngine.SceneManagement;

public class Vitoria : MonoBehaviour
{
    // Botão: Voltar para Temas
    public void IrParaTemas()
    {
        SceneManager.LoadScene("Temas");
    }

    // Botão: Jogar novamente
    public void JogarNovamente()
    {
        SceneManager.LoadScene("Jogo");
    }
}