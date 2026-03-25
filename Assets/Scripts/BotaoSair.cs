using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class BotaoSair : MonoBehaviour
{
    public Animator animador1;
    public Animator animador2;
    public Button botao;
    public GameObject objetoAnimado1;
    public GameObject objetoAnimado2;
    public GameObject logoFalsa;
    public CanvasGroup logoCanvasGroup;

    public float tempoAnimacao = 1.0f;

    void Start()
    {
        if (botao != null)
        {
            botao.onClick.AddListener(AoClicar);
        }

        if (logoCanvasGroup != null)
        {
            logoCanvasGroup.alpha = 0;
        }
    }

    void AoClicar()
    {
        Debug.Log("Botão clicado");

        if (botao != null)
        {
            botao.interactable = false;
        }

        if (animador1 != null)
        {
            animador1.SetTrigger("TriggarSair");
        }

        if (animador2 != null)
        {
            animador2.SetTrigger("TriggarSairCima");
        }

        StartCoroutine(DesativarDepois());
    }

    IEnumerator DesativarDepois()
    {
        yield return new WaitForSeconds(tempoAnimacao);

        if (objetoAnimado1 != null)
        {
            objetoAnimado1.SetActive(false);
        }

        if (objetoAnimado2 != null)
        {
            objetoAnimado2.SetActive(false);
        }

        if (botao != null)
        {
            botao.gameObject.SetActive(false);
        }

        if (logoFalsa != null)
        {
            logoFalsa.SetActive(false);
        }

        if (logoCanvasGroup != null)
        {
            logoCanvasGroup.alpha = 1;
        }

        Debug.Log("Carregando cena Temas...");

        SceneManager.LoadScene("Temas"); // ✅ corrigido aqui
    }
}