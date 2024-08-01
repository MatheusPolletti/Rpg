using System.Globalization;
using System.Text;
Console.OutputEncoding = Encoding.UTF8;

Console.Title = "Jogo de RPG";

string nomeMago = "";

List<string> feiticosTotais = new List<string>();
string nomeFeiticoTemporario = "";

TextInfo transformarTexto = new CultureInfo("pt-br", false).TextInfo;

while (nomeMago.Length < 7)
{
    Console.WriteLine("Por favor, grande mago, nos diga seu nome: ");
    nomeMago = transformarTexto.ToTitleCase(Convert.ToString(Console.ReadLine()));
    if (nomeMago.Length < 7)
    {
        Console.WriteLine("O nome do mago deve ter pelo menos 7 caracteres.");
    }
}

for (int c = 0; c < 3; c++)
{
    Console.WriteLine("Digite o nome do feitiço, ele precisa ter entre 2 e 14 caracteres e não pode ser repetido.");

    while (nomeFeiticoTemporario.Length < 2 || nomeFeiticoTemporario.Length > 14 || feiticosTotais.Contains(nomeFeiticoTemporario))
    {
        nomeFeiticoTemporario = transformarTexto.ToTitleCase(Convert.ToString(Console.ReadLine()));
        if (nomeFeiticoTemporario.Length < 2 || nomeFeiticoTemporario.Length > 14)
        {
            Console.WriteLine("O nome do feitiço precisa ter entre 2 e 14 caracteres.");
        }
        else if (feiticosTotais.Contains(nomeFeiticoTemporario))
        {
            Console.WriteLine("Você já possui como habilidade esse feitiço, escolha outro, por favor.");
        }
    }

    while (nomeMago == nomeFeiticoTemporario)
    {
        Console.WriteLine("O nome do feitiço não pode ser o mesmo que o nome do mago.");
        nomeFeiticoTemporario = transformarTexto.ToTitleCase(Convert.ToString(Console.ReadLine()));
    }

    feiticosTotais.Add(nomeFeiticoTemporario);
    nomeFeiticoTemporario = "";
}

modeloMago magoHistoria = new modeloMago(nomeMago, feiticosTotais);

Console.ReadKey();

class modeloMago
{
    public string? nome { get; set; }
    public List<string> feitico;
    private int espacosFeiticos;
    private float experiencia;
    private int pocao;
    private int vida;

    public static int Contador;

    public modeloMago(string _nome, List<string> _feitico)
    {
        nome = _nome;
        feitico = _feitico;
        espacosFeiticos = 3;
        experiencia = 0f;
        pocao = 3;
        vida = 100;

        Contador++;
    }

    public int ChamarFeitico()
    {
        if (espacosFeiticos > 0)
        {
            Console.WriteLine("Escolha qual feitiço você quer usar: \n1 - {0}\n2 - {1}\n3 - {2}", feitico[0], feitico[1], feitico[2]);
            int numeroFeitico = Convert.ToInt32(Console.ReadLine());
            
            if (numeroFeitico > 3 || numeroFeitico < 1)
            {
                Console.WriteLine("Esse feitiço não existe");
                return 0;
            }
            else
            {
                Console.WriteLine("O mago {0} usou o feitiço de {1}.", nome, feitico[numeroFeitico - 1]);
                espacosFeiticos--;
                Random aumento = new Random();
                float aumentoExperiencia = aumento.Next(1, 5) / 10f;
                experiencia += aumentoExperiencia;
                switch (numeroFeitico)
                {
                    case 1:
                        return 40;
                    case 2:
                        return 20;
                    case 3:
                        return 10;
                    default:
                        break;
                }
            }
        }
        else
        {
            Console.WriteLine("O mago {0} não tem espaços suficientes para usar um feitiço.", nome);
            return 0;
        }
        return 0;
    }

    public void Meditar()
    {
        Random numero = new Random();
        Console.WriteLine("O mago {0} meditou por {1} horas.", nome, numero.Next(3, 6));
        espacosFeiticos = espacosFeiticos + numero.Next(2, 6);
    }

    public void Dados()
    {
        Console.WriteLine("O mago {0} tem {1} pontos de experiência, ele possui {2} espaços de feitiços, {3} poções e {4} pontos de vida.", nome, experiencia, espacosFeiticos, pocao, vida);
    }

    public void BeberPocao()
    {
        if (pocao > 0 && vida > 0)
        {
            Random vidaAumento = new Random();
            int vidaMago = vidaAumento.Next(20, 51);
            pocao--;
            vida += vidaMago;
            if (vida > 100)
            {
                Console.WriteLine("Não é possível que o mago tenha mais que 100 pontos de vida");
                vida = 100;
            }
            Console.WriteLine("O mago {0} bebeu uma poção e recuperou {1} pontos de vida, estejando agora com {2} pontos de vida.", nome, vidaMago, vida);
        }
        else if (pocao <= 0)
        {
            Console.WriteLine("O mago {0} não tem poções para usar.", nome);
        }
        else if (vida <= 0)
        {
            Console.WriteLine("O mago {0} está morto.", nome);
        }
        else
        {
            Console.WriteLine("Algo não funcionou no uso da poção");
        }
    }

    public void LutarInimigo()
    {
        ModeloInimigo inimigo = new ModeloInimigo();
        int inimigoAtual = inimigo.InimigoVaiLutar();

        string nomeInimigo = inimigo.nomesInimigosNivel1[inimigoAtual];
        int vidaInimigo = inimigo.vidaInimigoNivel1[inimigoAtual];
        int danoInimigo = inimigo.danoInimigoNivel1[inimigoAtual];

        while (vidaInimigo > 0)
        {
            int dano = ChamarFeitico();
            vidaInimigo -= dano;
            Console.WriteLine("Vida do {0}: {1}.", nomeInimigo, vidaInimigo);
            if (vidaInimigo <= 0)
            {
                Random aumento = new Random();
                int quantiaEspacos = aumento.Next(0, 2);
                int quantiaPocao = aumento.Next(0, 3);
                float quantiaExperiencia = aumento.Next(6, 9) / 10f;

                espacosFeiticos += quantiaEspacos;
                pocao += quantiaPocao;
                experiencia += quantiaExperiencia;

                Console.WriteLine("O mago {0} derrotou um {1} e ganhou {2} espaços de feitiço, {3} poções e {4} de experiência", nome, nomeInimigo, quantiaEspacos, quantiaPocao, quantiaExperiencia);
                break;
            }
            vida -= danoInimigo;
            Console.WriteLine("Vida do mago {0}: {1}.", nome, vida);
            if (vida <= 0)
            {
                Console.WriteLine("O mago {0} morreu na batalha contra um {1}", nome, nomeInimigo);
                break;
            }
        }
    }
}


class ModeloInimigo
{
    public List<string> nomesInimigosNivel1;
    public List<int> vidaInimigoNivel1;
    public List<int> danoInimigoNivel1;

    public ModeloInimigo()
    {
        nomesInimigosNivel1 = ["Porco Espinho", "Rato Atroz", "Escorpião", "Goblin"];
        vidaInimigoNivel1 = [20, 30, 40, 60];
        danoInimigoNivel1 = [12, 16, 20, 24];
    }

    public int InimigoVaiLutar()
    {
        Random gerarNumero = new Random();
        int numeroInimigo = gerarNumero.Next(0, 4);

        Console.WriteLine("Um {0} apareceu.\nVida: {1}\nDano: {2}", nomesInimigosNivel1[numeroInimigo], vidaInimigoNivel1[numeroInimigo], danoInimigoNivel1[numeroInimigo]);
        return numeroInimigo;
    }
}
