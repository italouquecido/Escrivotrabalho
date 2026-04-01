using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class JogoController : MonoBehaviour
{
    public TextMeshProUGUI textLetra;
    public TextMeshProUGUI textTema;
    public TextMeshProUGUI textDica;
    public TextMeshProUGUI textTentativas; // agora mostra "Vidas"
    public TextMeshProUGUI textRodada;
    public TMP_InputField inputField;

    public AudioSource audioSource;
    public AudioClip somAcerto;
    public AudioClip somErro;

    int vidas; // antes era tentativas
    int indiceDica;
    int rodada = 1;
    int maxRodadas = 3;

    string palavraCorreta;
    string[,] palavrasAtuais;
    string[] dicasAtuais;

    bool jogoAtivo = false;

    void Start()
    {
        string dificuldade = PlayerPrefs.GetString("dificuldade", "facil");

        if (dificuldade == "medio")
            palavrasAtuais = palavrasMedio;
        else if (dificuldade == "dificil")
            palavrasAtuais = palavrasDificil;
        else
            palavrasAtuais = palavrasFacil;

        inputField.lineType = TMP_InputField.LineType.SingleLine;
        inputField.onSubmit.AddListener(OnSubmitMobile);

        inputField.gameObject.SetActive(false);

        AtualizarRodada();
        StartCoroutine(RodarRoleta());
    }

    void AtualizarRodada()
    {
        textRodada.text = "Rodada: " + rodada + "/" + maxRodadas;
    }

    IEnumerator RodarRoleta()
    {
        jogoAtivo = false;

        vidas = 3; // inicializa as vidas
        indiceDica = 0;

        textTentativas.text = "Vidas: " + vidas;
        textDica.text = "";

        string[] letras = {
            "A","B","C","D","E","F","G","H","I","J","K","L","M",
            "N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
        };

        float tempo = 0.05f;

        for (int i = 0; i < 40; i++)
        {
            textLetra.text = letras[Random.Range(0, 26)];

            if (i > 25) tempo += 0.02f;

            yield return new WaitForSeconds(tempo);
        }

        CarregarPalavra();

        jogoAtivo = true;

        inputField.gameObject.SetActive(true);
        inputField.text = "";
        inputField.ActivateInputField();
    }

    void CarregarPalavra()
    {
        int i = Random.Range(0, palavrasAtuais.GetLength(0));

        palavraCorreta = palavrasAtuais[i, 2];

        textLetra.text = palavrasAtuais[i, 0];
        textTema.text = "Tema: " + palavrasAtuais[i, 1];

        dicasAtuais = new string[3];
        dicasAtuais[0] = palavrasAtuais[i, 3];
        dicasAtuais[1] = palavrasAtuais[i, 4];
        dicasAtuais[2] = palavrasAtuais[i, 5];

        AtualizarDicas();
    }

    void AtualizarDicas()
    {
        string texto = "Dicas:\n";

        for (int i = 0; i <= indiceDica; i++)
        {
            texto += (i + 1) + " - " + dicasAtuais[i] + "\n";
        }

        textDica.text = texto;
    }

    void OnSubmitMobile(string texto)
    {
        VerificarResposta();
    }

    public void BotaoEnviar()
    {
        VerificarResposta();
    }

    public void VerificarResposta()
    {
        if (!jogoAtivo) return;

        string resposta = inputField.text.ToUpper().Trim();
        if (resposta == "") return;

        if (resposta == palavraCorreta)
        {
            if (audioSource != null && somAcerto != null)
                audioSource.PlayOneShot(somAcerto);

            jogoAtivo = false;
            inputField.gameObject.SetActive(false);

            textDica.text = "ACERTOU!";

            rodada++;

            if (rodada > maxRodadas)
                StartCoroutine(IrParaVitoria());
            else
            {
                AtualizarRodada();
                StartCoroutine(ProximaRodada());
            }
        }
        else
        {
            if (audioSource != null && somErro != null)
                audioSource.PlayOneShot(somErro);

            vidas--; // decrementa vida
            textTentativas.text = "Vidas: " + vidas;

            if (vidas <= 0)
            {
                jogoAtivo = false;
                inputField.gameObject.SetActive(false);

                textDica.text = "ERROU! Era: " + palavraCorreta;

                StartCoroutine(IrParaDerrota());
                return;
            }

            indiceDica++;
            if (indiceDica > 2) indiceDica = 2;

            AtualizarDicas();
        }

        inputField.text = "";
        inputField.ActivateInputField();
    }

    IEnumerator ProximaRodada()
    {
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(RodarRoleta());
    }

    IEnumerator IrParaVitoria()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Vitoria");
    }

    IEnumerator IrParaDerrota()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Derrota");
    }

    void Update()
    {
        if (!jogoAtivo) return;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame ||
                Keyboard.current.numpadEnterKey.wasPressedThisFrame)
            {
                VerificarResposta();
            }
        }
    }

    // ===== BANCO DE PALAVRAS SUPER FÁCIL =====

    string[,] palavrasFacil = new string[,]
    {
        {"A","Animal","ABELHA","faz mel","tem listras","voa"},
        {"B","Objeto","BOLA","redonda","usada no futebol","pula"},
        {"C","Animal","CAVALO","tem quatro patas","faz galope","vive na fazenda"},
        {"D","Parte do Corpo","DENTE","fica na boca","usado para comer","branco"},
        {"E","Objeto","ESCOVA","limpa os dentes","penteia o cabelo","limpeza"},
        {"F","Comida","FEIJAO","pretinho ou carioquinha","come com arroz","tem caldo"},
        {"G","Animal","GATO","faz miau","tem bigodes","gosta de leite"},
        {"H","Meio de Transporte","HELICOPTERO","voa no ar","tem helice","nao e aviao"},
        {"I","Lugar","IGREJA","tem sino","lugar de orar","tem cruz"},
        {"J","Animal","JACARE","vive na agua","tem bocao","tem escamas"},
        {"L","Fruta","LARANJA","cor de fruta","faz suco","tem vitamina c"},
        {"M","Animal","MACACO","gosta de banana","pula em arvore","faz careta"},
        {"N","Natureza","NUVEM","fica no ceu","parece algodao","solta chuva"},
        {"O","Comida","OVO","vem da galinha","tem gema","pode ser frito"},
        {"P","Comida","PIPOCA","estoura na panela","come no cinema","e de milho"},
        {"Q","Comida","QUEIJO","amarelo ou branco","rato gosta","vai no pao"},
        {"R","Animal","RATO","tem medo de gato","gosta de queijo","tem rabo comprido"},
        {"S","Animal","SAPO","verde e pula","vive na lagoa","come mosca"},
        {"T","Objeto","TELEFONE","usado para ligar","tem numero","faz trim trim"},
        {"U","Fruta","UVA","pequena e redonda","pode ser verde ou roxa","cresce em cachos"},
        {"V","Animal","VACA","da leite","faz muu","tem chifres"},
        {"Z","Animal","ZEBRA","tem listras","parece cavalo","preto e branco"}
    };

    string[,] palavrasMedio = new string[,]
    {
        {"A","Objeto","ANEL","usa no dedo","pode ser de ouro","acessorio"},
        {"B","Fruta","BANANA","amarela","fruta do macaco","comprida"},
        {"C","Objeto","CADEIRA","usada para sentar","tem quatro pernas","tem na sala"},
        {"D","Natureza","DIA","tem sol","claro","contrario de noite"},
        {"E","Objeto","ESPELHO","reflete a imagem","feito de vidro","usado para se ver"},
        {"F","Objeto","FACA","usada para cortar","fica na cozinha","e afiada"},
        {"G","Animal","GALINHA","bota ovo","tem penas","faz cocoricó"},
        {"I","Lugar","ILHA","terra cercada de agua","tem praia","fica no mar"},
        {"J","Objeto","JANELA","fica na parede","pode abrir e fechar","da para ver a rua"},
        {"L","Objeto","LAPIZ","usado para escrever","tem grafite","usa na escola"},
        {"M","Objeto","MARTELO","bate prego","ferramenta","pesado"},
        {"N","Natureza","NEVE","gelo que cai do ceu","muito gelada","branca"},
        {"O","Parte do Corpo","OUVIDO","usado para escutar","fica na cabeca","orelha"},
        {"P","Objeto","PORTA","entrada da casa","tem macaneta","abre e fecha"},
        {"R","Objeto","RELOGIO","mostra as horas","fica no pulso","faz tic tac"},
        {"S","Clima","SOL","bola de fogo","aquece a terra","brilha de dia"},
        {"T","Objeto","TESOURA","usada para cortar papel","tem duas laminas","objeto escolar"},
        {"U","Objeto","URSO","animal de pelucia","tem muito pelo","vive na floresta"},
        {"V","Objeto","VASSOURA","usada para varrer","limpa o chao","tem cabo"},
        {"X","Bebida","XICARA","usada para tomar cafe","tem asa","pequena"}
    };

    string[,] palavrasDificil = new string[,]
    {
        {"A","Objeto","AVIAO","tem asas","voa com passageiros","tem piloto"},
        {"B","Objeto","BICICLETA","tem duas rodas","tem pedal","anda na rua"},
        {"C","Objeto","CAMPO","tem muita grama","onde joga bola","lugar aberto"},
        {"D","Comida","DOCE","tem muito acucar","crianca adora","balas e pirulitos"},
        {"E","Lugar","ESCOLA","onde o aluno estuda","tem professor","tem lousa"},
        {"F","Natureza","FOGO","e muito quente","queima","faz fumaca"},
        {"G","Objeto","GARRAFA","guarda agua","pode ser de plastico","tem tampa"},
        {"J","Lugar","JARDIM","tem muitas flores","tem grama","lugar bonito"},
        {"L","Bebida","LEITE","vem da vaca","e branco","bebe no cafe"},
        {"M","Objeto","MOCHILA","guarda livros","carrega nas costas","usa na escola"},
        {"N","Natureza","NOITE","tem lua","escuro","hora de dormir"},
        {"O","Parte do Corpo","OLHO","usado para ver","fica no rosto","tem iris"},
        {"P","Comida","PAO","compra na padaria","come no cafe","tem farinha"},
        {"S","Objeto","SAPATO","usa no pe","tem cadarco","anda no chao"},
        {"T","Objeto","TELEVISAO","passa desenho","tem tela","tem controle remoto"},
        {"V","Objeto","VELA","clarea no escuro","derrete","tem pavio"}
    };
}