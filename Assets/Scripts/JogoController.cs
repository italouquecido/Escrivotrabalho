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
    public TextMeshProUGUI textTentativas;
    public TextMeshProUGUI textRodada;
    public TMP_InputField inputField;

    public AudioSource audioSource;
    public AudioClip somAcerto;
    public AudioClip somErro;

    int tentativas;
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

        tentativas = 3;
        indiceDica = 0;

        textTentativas.text = "Tentativas: " + tentativas;
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

            tentativas--;
            textTentativas.text = "Tentativas: " + tentativas;

            if (tentativas <= 0)
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

    // ===== BANCO GIGANTE =====

    string[,] palavrasFacil = new string[,]
    {
        {"A","Animal","ARARA","ave colorida","fala","floresta"},
        {"B","Animal","BURRO","carga","cavalo","teimoso"},
        {"C","Animal","COBRA","rasteja","veneno","sem pernas"},
        {"D","Animal","DOGO","cachorro grande","forte","guarda"},
        {"E","Animal","EGUA","cavalo femea","fazenda","montaria"},
        {"F","Animal","FOCA","animal marinho","nada","frio"},
        {"G","Animal","GATO","mia","pet","dorme"},
        {"H","Animal","HIPPO","grande","agua","africa"},
        {"I","Animal","IGUANA","reptil","verde","sol"},
        {"J","Animal","JAGUAR","felino","rapido","selva"},
        {"K","Animal","KOALA","australia","dorme","folha"},
        {"L","Animal","LEAO","rei","juba","forte"},
        {"M","Animal","MACACO","arvore","banana","salta"},
        {"N","Animal","NUTRO","roedor","agua","rio"},
        {"O","Animal","ONCA","felino","brasil","rapido"},
        {"P","Animal","PANDA","bambu","preto branco","china"},
        {"Q","Animal","QUATI","grupo","focinho","mata"},
        {"R","Animal","RAPOSA","esperta","laranja","rapida"},
        {"S","Animal","SAPO","pula","verde","agua"},
        {"T","Animal","TIGRE","listras","forte","selva"},
        {"U","Animal","URUBU","morto","voa","preto"},
        {"V","Animal","VACA","leite","fazenda","capim"},
        {"W","Animal","WOMBAT","toca","noturno","australia"},
        {"X","Animal","XEXEU","ninho","canta","passaro"},
        {"Y","Animal","YAGUE","felino","manchado","selva"},
        {"Z","Animal","ZEBRA","listras","africa","cavalo"}
    };

    string[,] palavrasMedio = new string[,]
    {
        {"A","Profissao","ATOR","filmes","personagem","fama"},
        {"B","Profissao","BOMBEIRO","fogo","salva","agua"},
        {"C","Profissao","COZINHEIRO","comida","receita","cozinha"},
        {"D","Profissao","DENTISTA","dente","tratamento","consulta"},
        {"E","Profissao","ENGENHEIRO","projeto","construcao","calculo"},
        {"F","Profissao","FOTOGRAFO","foto","camera","imagem"},
        {"G","Profissao","GARCOM","restaurante","serve","cliente"},
        {"H","Profissao","HISTORIADOR","passado","estudo","historia"},
        {"I","Profissao","INSTRUTOR","ensina","treino","aula"},
        {"J","Profissao","JORNALISTA","noticia","reportagem","midia"},
        {"K","Esporte","KARATE","luta","golpe","kimono"},
        {"L","Profissao","LOCUTOR","radio","voz","fala"},
        {"M","Profissao","MEDICO","hospital","cura","paciente"},
        {"N","Profissao","NUTRICIONISTA","dieta","comida","saude"},
        {"O","Profissao","ODONTOLOGO","boca","dente","tratamento"},
        {"P","Profissao","PILOTO","aviao","voar","altura"},
        {"Q","Ciencia","QUIMICA","reacao","laboratorio","materia"},
        {"R","Profissao","ROTEIRISTA","historia","dialogo","filme"},
        {"S","Profissao","SOLDADO","defesa","guerra","treino"},
        {"T","Profissao","TECNICO","conserta","equipamento","problema"},
        {"U","Profissao","URBANISTA","cidade","planeja","mapa"},
        {"V","Profissao","VETERINARIO","animal","pet","clinica"},
        {"W","Tecnologia","WIFI","internet","rede","senha"},
        {"X","Instrumento","XILOFONE","musica","teclas","som"},
        {"Y","Tecnologia","YOUTUBE","video","canal","internet"},
        {"Z","Profissao","ZOOLOGO","animal","estudo","zoologico"}
    };

    string[,] palavrasDificil = new string[,]
    {
        {"A","Ciencia","ATOMO","materia","particula","estrutura"},
        {"B","Geografia","BALTICO","mar","europa","frio"},
        {"C","Historia","CZAR","rei","russia","imperio"},
        {"D","Ciencia","DNA","genetica","codigo","celula"},
        {"E","Filosofia","ETICA","moral","certo","errado"},
        {"F","Ciencia","FOTON","luz","energia","rapido"},
        {"G","Historia","GLADIADOR","roma","arena","luta"},
        {"H","Ciencia","HIPOTESE","teste","ideia","metodo"},
        {"I","Filosofia","IMANENCIA","conceito","ser","existencia"},
        {"J","Historia","JACOBINO","franca","revolucao","grupo"},
        {"K","Historia","KAMIKAZE","guerra","aviao","ataque"},
        {"L","Ciencia","LACUNA","falha","vazio","erro"},
        {"M","Ciencia","MITOSE","celula","divisao","crescimento"},
        {"N","Filosofia","NIILISMO","nada","sentido","filosofia"},
        {"O","Ciencia","OSMOSE","agua","membrana","equilibrio"},
        {"P","Ciencia","PLASMA","estado","energia","sol"},
        {"Q","Ciencia","QUARK","particula","fisica","proton"},
        {"R","Historia","RENASCIMENTO","arte","europa","cultura"},
        {"S","Ciencia","SINERGIA","grupo","forca","resultado"},
        {"T","Ciencia","TEORIA","explica","ciencia","evidencia"},
        {"U","Ciencia","UNIVERSO","espaco","galaxia","tudo"},
        {"V","Filosofia","VIRTUDE","moral","bem","etica"},
        {"W","Historia","WATERLOO","batalha","napoleao","fim"},
        {"X","Ciencia","XENOFOBIA","medo","estranho","rejeicao"},
        {"Y","Historia","YALTA","guerra","acordo","reuniao"},
        {"Z","Ciencia","ZIGOTO","vida","inicio","celula"}
    };
}