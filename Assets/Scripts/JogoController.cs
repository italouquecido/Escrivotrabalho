using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class JogoController : MonoBehaviour
{
    public TextMeshProUGUI textLetra;
    public TextMeshProUGUI textTema;
    public TextMeshProUGUI textDica;
    public TextMeshProUGUI textTentativas;
    public TMP_InputField inputField;

    int tentativas = 3;
    int indiceDica = 0;
    string palavraCorreta = "";
    string dificuldade = "";

    bool roletaRodando = false;
    bool jogoAtivo = false;

    // banco de palavras
    // estrutura: letra, tema, palavra, dica1, dica2, dica3

    string[,] palavrasFacil = new string[,]
    {
        {"A", "Animal", "ARARA", "ave colorida", "fala como humano", "vive na floresta"},
        {"B", "Animal", "BALEIA", "maior animal do mar", "mamifero aquatico", "sopra agua pelo alto"},
        {"C", "Animal", "COBRA", "reptil sem pernas", "pode ser venenosa", "rasteja no chao"},
        {"D", "Animal", "DINOSSAURO", "animal extinto", "existiu antes dos humanos", "alguns eram carnivoros"},
        {"E", "Animal", "ELEFANTE", "maior animal terrestre", "tem tromba", "tem grandes orelhas"},
        {"F", "Animal", "FRANGO", "ave domestica", "virou alimento", "nao voa muito"},
        {"G", "Animal", "GATO", "animal domestico", "mia", "tem bigodes"},
        {"H", "Animal", "HAMSTER", "roedor pequeno", "guarda comida nas bochechas", "roda em roda"},
        {"I", "Animal", "IGUANA", "reptil verde", "parece um dragao pequeno", "vive em areas quentes"},
        {"J", "Animal", "JAGUAR", "felino manchado", "e o maior felino das americas", "caça na agua"},
        {"K", "Animal", "KOALA", "animal australiano", "dorme muito", "come folhas de eucalipto"},
        {"L", "Animal", "LEAO", "rei da selva", "tem juba", "vive em alcateias"},
        {"M", "Animal", "MACACO", "primata", "vive em arvores", "come banana"},
        {"N", "Animal", "NUTRIA", "roedor aquatico", "parece um castor", "vive perto de rios"},
        {"O", "Animal", "ONCA", "felino brasileiro", "e ameacada de extincao", "vive na amazonia"},
        {"P", "Animal", "PATO", "ave aquatica", "faz quac quac", "tem bico largo"},
        {"Q", "Animal", "QUATI", "mamifero brasileiro", "tem focinho longo", "vive em grupos"},
        {"R", "Animal", "RATO", "roedor pequeno", "vive em esgotos", "tem dentes afiados"},
        {"S", "Animal", "SAPO", "anfibio", "vive perto da agua", "pula muito"},
        {"T", "Animal", "TIGRE", "felino listrado", "e o maior felino do mundo", "nada bem"},
        {"U", "Animal", "URUBU", "ave carniceira", "come animais mortos", "tem cabeca vermelha"},
        {"V", "Animal", "VACA", "animal da fazenda", "da leite", "muge"},
        {"W", "Animal", "WOMBAT", "marsupial australiano", "escava tocas", "fezes em formato de cubo"},
        {"X", "Animal", "XENOPS", "passaro pequeno", "vive na floresta tropical", "sobe em troncos"},
        {"Y", "Animal", "YAK", "boi peludo", "vive no Tibete", "suporta muito frio"},
        {"Z", "Animal", "ZEBRA", "animal listrado", "parece um cavalo", "vive na africa"}
    };

    string[,] palavrasMedio = new string[,]
    {
        {"A", "Profissao", "ATOR", "trabalha em filmes", "interpreta personagens", "pode ganhar oscar"},
        {"B", "Profissao", "BIOLOGO", "estuda seres vivos", "pode trabalhar na natureza", "analisa celulas"},
        {"C", "Profissao", "CHEF", "cozinha profissionalmente", "cria receitas", "trabalha em restaurantes"},
        {"D", "Profissao", "DENTISTA", "cuida dos dentes", "usa broca", "trabalha com sorriso"},
        {"E", "Profissao", "ENFERMEIRO", "cuida de pacientes", "trabalha em hospitais", "aplica injecoes"},
        {"F", "Profissao", "FOTOGRAFO", "tira fotos", "usa camera", "captura momentos"},
        {"G", "Profissao", "GEOLOGO", "estuda rochas", "analisa terrenos", "trabalha em minas"},
        {"H", "Profissao", "HACKER", "especialista em seguranca", "invade sistemas", "protege redes"},
        {"I", "Profissao", "INTERPRETE", "traduz idiomas", "trabalha em tempo real", "conhece varios idiomas"},
        {"J", "Profissao", "JUIZ", "aplica a lei", "usa toga", "julga processos"},
        {"K", "Esporte", "KARATE", "arte marcial", "usa kimono", "golpes com maos e pes"},
        {"L", "Profissao", "LOCUTOR", "fala no radio", "tem voz marcante", "narra eventos"},
        {"M", "Profissao", "MEDICO", "cuida da saude", "usa estetoscopio", "trabalha em hospitais"},
        {"N", "Profissao", "NUTRICIONISTA", "cuida da alimentacao", "monta dietas", "estuda alimentos"},
        {"O", "Profissao", "ODONTOLOGO", "cuida da boca", "outro nome do dentista", "trata os dentes"},
        {"P", "Profissao", "PILOTO", "dirige avioes", "trabalha nas alturas", "usa uniforme especial"},
        {"Q", "Quimica", "QUIMICA", "ciencia da materia", "estuda reacoes", "tem tabela periodica"},
        {"R", "Profissao", "ROTEIRISTA", "escreve historias", "cria dialogos", "trabalha em filmes"},
        {"S", "Profissao", "SOLDADO", "defende o pais", "usa farda", "faz treinamento fisico"},
        {"T", "Profissao", "TECNICO", "conserta equipamentos", "tem conhecimento especializado", "resolve problemas"},
        {"U", "Profissao", "URBANISTA", "planeja cidades", "projeta espacos urbanos", "trabalha com mapas"},
        {"V", "Profissao", "VETERINARIO", "medico de animais", "cuida de pets", "trabalha em clinicas"},
        {"W", "Tecnologia", "WIFI", "sinal sem fio", "conecta a internet", "tem senha"},
        {"X", "Instrumento", "XILOFONE", "instrumento de percussao", "tem teclas de madeira", "bate com baqueta"},
        {"Y", "Tecnologia", "YOUTUBE", "plataforma de videos", "tem criadores de conteudo", "e do google"},
        {"Z", "Profissao", "ZOOLOGO", "estuda animais", "trabalha em zoologicos", "faz pesquisas"}
    };

    string[,] palavrasDificil = new string[,]
    {
        {"A", "Ciencia", "ATOMO", "menor parte da materia", "tem protons e eletrons", "forma tudo no universo"},
        {"B", "Geografia", "BALTICO", "mar europeu", "e pouco salgado", "banha paises nordicos"},
        {"C", "Historia", "CZAR", "titulo de rei russo", "governou a russia", "foi deposto na revolucao"},
        {"D", "Ciencia", "DNA", "molecula genetica", "esta em todas as celulas", "define caracteristicas"},
        {"E", "Filosofia", "ETICA", "estudo da moral", "define certo e errado", "ramo da filosofia"},
        {"F", "Ciencia", "FOTOSSINTESE", "processo das plantas", "usa luz solar", "produz oxigenio"},
        {"G", "Historia", "GLADIADOR", "lutador romano", "lutava na arena", "enfrentava animais e humanos"},
        {"H", "Ciencia", "HIPOTESE", "suposicao cientifica", "precisa ser testada", "base do metodo cientifico"},
        {"I", "Filosofia", "IMANENCIA", "conceito filosofico", "oposto de transcendencia", "existe no proprio ser"},
        {"J", "Historia", "JACOBINO", "grupo da revolucao francesa", "era radical", "defendia o terror"},
        {"K", "Historia", "KAMIKAZE", "piloto suicida japones", "ocorreu na segunda guerra", "atacava navios"},
        {"L", "Ciencia", "LACUNA", "espaco vazio", "falha em argumento", "ponto fraco"},
        {"M", "Ciencia", "MITOSE", "divisao celular", "gera celulas iguais", "ocorre no crescimento"},
        {"N", "Filosofia", "NIILISMO", "negacao de valores", "nada tem sentido", "filosofia do vazio"},
        {"O", "Ciencia", "OSMOSE", "passagem de agua", "atravessa membrana", "equilibra concentracao"},
        {"P", "Ciencia", "PLASMA", "estado da materia", "e o quarto estado", "encontrado no sol"},
        {"Q", "Ciencia", "QUARKS", "particulas subatomicas", "formam protons", "descobertos na fisica"},
        {"R", "Historia", "RENASCIMENTO", "movimento cultural", "ocorreu na europa", "valorizava o homem"},
        {"S", "Ciencia", "SINERGIA", "cooperacao de elementos", "o todo e maior que as partes", "aumenta resultados"},
        {"T", "Ciencia", "TEORIA", "explicacao cientifica", "baseada em evidencias", "pode ser revisada"},
        {"U", "Ciencia", "UNIVERSO", "tudo que existe", "em constante expansao", "tem bilhoes de galaxias"},
        {"V", "Filosofia", "VIRTUDE", "qualidade moral", "busca pelo bem", "conceito de aristoteles"},
        {"W", "Historia", "WATERLOO", "batalha famosa", "napoleao foi derrotado", "ocorreu na belgica"},
        {"X", "Ciencia", "XENOFOBIA", "medo do estranho", "discriminacao", "rejeicao ao diferente"},
        {"Y", "Historia", "YALTA", "conferencia historica", "pos segunda guerra", "dividiu o mundo"},
        {"Z", "Ciencia", "ZIGOTO", "celula fertilizada", "inicio da vida", "une ovulo e espermatozoide"}
    };

    string letraSorteada = "";
    string[,] palavrasAtuais;

    void Start()
    {
        dificuldade = PlayerPrefs.GetString("dificuldade", "facil");

        if (dificuldade == "facil")
        {
            palavrasAtuais = palavrasFacil;
        }
        else if (dificuldade == "medio")
        {
            palavrasAtuais = palavrasMedio;
        }
        else
        {
            palavrasAtuais = palavrasDificil;
        }

        inputField.gameObject.SetActive(false);
        StartCoroutine(RodarRoleta());
    }

    IEnumerator RodarRoleta()
    {
        roletaRodando = true;
        jogoAtivo = false;

        string[] alfabeto = new string[]
        {
            "A","B","C","D","E","F","G","H","I","J","K","L","M",
            "N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
        };

        float tempo = 0.05f;
        int ciclos = 40;

        int i = 0;
        while (i < ciclos)
        {
            int indiceAleatorio = Random.Range(0, 26);
            textLetra.text = alfabeto[indiceAleatorio];

            if (i > 25)
            {
                tempo = tempo + 0.02f;
            }

            yield return new WaitForSeconds(tempo);
            i = i + 1;
        }

        int indiceFinal = Random.Range(0, 26);
        letraSorteada = alfabeto[indiceFinal];
        textLetra.text = letraSorteada;

        CarregarPalavra();

        roletaRodando = false;
        jogoAtivo = true;
        tentativas = 3;
        indiceDica = 0;
        inputField.gameObject.SetActive(true);
        inputField.Select();
        inputField.ActivateInputField();
    }

    void CarregarPalavra()
    {
        int indice = 0;
        int i = 0;
        while (i < palavrasAtuais.GetLength(0))
        {
            if (palavrasAtuais[i, 0] == letraSorteada)
            {
                indice = i;
            }
            i = i + 1;
        }

        palavraCorreta = palavrasAtuais[indice, 2];
        textTema.text = "Tema: " + palavrasAtuais[indice, 1];
        textDica.text = "Dica: " + palavrasAtuais[indice, 3];
        textTentativas.text = "Tentativas: " + tentativas;
    }

    void MostrarProximaDica(int indice)
    {
        int indicePalavra = 0;
        int i = 0;
        while (i < palavrasAtuais.GetLength(0))
        {
            if (palavrasAtuais[i, 0] == letraSorteada)
            {
                indicePalavra = i;
            }
            i = i + 1;
        }

        if (indice == 1)
        {
            textDica.text = "Dica: " + palavrasAtuais[indicePalavra, 4];
        }
        if (indice == 2)
        {
            textDica.text = "Dica: " + palavrasAtuais[indicePalavra, 5];
        }
    }

    public void VerificarResposta()
    {
        if (jogoAtivo == false)
        {
            return;
        }

        string resposta = inputField.text.ToUpper().Trim();
        inputField.text = "";
        inputField.Select();
        inputField.ActivateInputField();

        if (resposta == palavraCorreta)
        {
            jogoAtivo = false;
            inputField.gameObject.SetActive(false);
            textDica.text = "ACERTOU!";
            PlayerPrefs.SetString("resultado", "vitoria");
            PlayerPrefs.SetString("palavraCorreta", palavraCorreta);
            PlayerPrefs.Save();
            StartCoroutine(IrParaVitoria());
        }
        else
        {
            tentativas = tentativas - 1;
            textTentativas.text = "Tentativas: " + tentativas;

            if (tentativas <= 0)
            {
                jogoAtivo = false;
                inputField.gameObject.SetActive(false);
                textDica.text = "Era: " + palavraCorreta;
                PlayerPrefs.SetString("resultado", "derrota");
                PlayerPrefs.SetString("palavraCorreta", palavraCorreta);
                PlayerPrefs.Save();
                StartCoroutine(IrParaDerrota());
            }
            else
            {
                indiceDica = indiceDica + 1;
                MostrarProximaDica(indiceDica);
            }
        }
    }

    IEnumerator IrParaVitoria()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Vitoria");
    }

    IEnumerator IrParaDerrota()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Derrota");
    }

    void Update()
    {
        if (jogoAtivo == true)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                VerificarResposta();
            }
        }
    }
}