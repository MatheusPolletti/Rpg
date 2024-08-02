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

int vidaMago = magoHistoria.vida;
while (vidaMago > 0)
{
    Console.Clear();
    Console.WriteLine("Olá, mago {0}. O que você gostaria de fazer: ", nomeMago);
    Console.WriteLine("1 - Treinar Feitiço\n2 - Meditar\n3 - Beber poção de vida\n4 - Lutar Contra um Inimigo\n5 - Fazer compra\n6 - Ver dados do mago");
    int opcaoEscolhida = Convert.ToInt32(Console.ReadLine());

    switch (opcaoEscolhida)
    {
        case 1:
            magoHistoria.ChamarFeitico();
            break;
        case 2:
            magoHistoria.Meditar();
            break;
        case 3:
            magoHistoria.BeberPocao();
            break;
        case 4:
            magoHistoria.LutarInimigo();
            break;
        case 5:
            magoHistoria.FazerCompras();
            break;
        case 6:
            magoHistoria.Dados();
            break;
        default:
            break;
    }

    Thread.Sleep(4000);

    vidaMago = magoHistoria.vida;
}

Console.ReadKey();

class modeloMago
{
    public string? nome { get; set; }
    public List<string> feitico;
    private int espacosFeiticos;
    private float experiencia;
    private int pocao;
    public int vida;
    private int feiticoUsado;
    private int dinheiro;
    private int danoFeiticoPrincipal;
    private int danoFeiticoSecundario;
    private int danoFeiticoTerciario;

    public static int Contador;

    public modeloMago(string _nome, List<string> _feitico)
    {
        nome = _nome;
        feitico = _feitico;

        espacosFeiticos = 3;
        experiencia = 0f;
        pocao = 3;
        vida = 100;
        dinheiro = 20;

        danoFeiticoPrincipal = 7;
        danoFeiticoSecundario = 4;
        danoFeiticoTerciario = 3;

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
                if (numeroFeitico == feiticoUsado)
                {
                    Console.WriteLine("O mago {0} não pode usar o mesmo feitiço duas vezes seguidas.", nome);
                    return -10;
                }
                Console.WriteLine("O mago {0} usou o feitiço de {1}.", nome, feitico[numeroFeitico - 1]);

                feiticoUsado = numeroFeitico;
                espacosFeiticos--;

                Random aumento = new Random();
                float aumentoExperiencia = aumento.Next(1, 5) / 10f;
                experiencia += aumentoExperiencia;

                switch (numeroFeitico)
                {
                    case 1:
                        return danoFeiticoPrincipal + Convert.ToInt32(Math.Floor(experiencia));
                    case 2:
                        return danoFeiticoSecundario + Convert.ToInt32(Math.Floor(experiencia));
                    case 3:
                        return danoFeiticoTerciario + Convert.ToInt32(Math.Floor(experiencia));
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
        int valorAumento = numero.Next(1, 3);
        espacosFeiticos += valorAumento;

        Console.WriteLine("O mago {0} meditou por {1} horas e ganhou {2} espaços de feitiço.", nome, numero.Next(3, 6), valorAumento);
    }

    public void Dados()
    {
        Console.WriteLine("O mago {0} tem {1} pontos de experiência, ele possui {2} espaços de feitiços, {3} poções, {4} pontos de vida e {5} moedas de ouro.", nome, experiencia, espacosFeiticos, pocao, vida, dinheiro);
    }

    public void BeberPocao()
    {
        if (pocao > 0 && vida > 0 && vida < 100)
        {
            Random vidaAumento = new Random();
            int vidaMago = vidaAumento.Next(20, 51);
            pocao--;
            vida += vidaMago;
            if (vida > 100)
            {
                vida = 100;
            }
            Console.WriteLine("O mago {0} bebeu uma poção e recuperou {1} pontos de vida, estejando agora com {2} pontos de vida.", nome, vidaMago, vida);
        }
        else if (vida >= 100)
        {
            Console.WriteLine("Não é possível que o mago tenha mais que 100 pontos de vida.");
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
            if (dano < 0)
            {
                continue;
            }

            Thread.Sleep(2000);
            Console.Clear();

            Console.WriteLine("O {0} tomou {1} de dano.", nomeInimigo, dano);
            vidaInimigo -= dano;
            Console.WriteLine("Vida do {0}: {1}.", nomeInimigo, vidaInimigo);

            if (vidaInimigo <= 0)
            {
                Random aumento = new Random();
                int quantiaPocao = aumento.Next(0, 3);
                float quantiaExperiencia = aumento.Next(6, 9) / 10f;
                int moedaGanha = aumento.Next(10, 31);

                pocao += quantiaPocao;
                experiencia += quantiaExperiencia;
                dinheiro += moedaGanha;

                Console.WriteLine("O mago {0} derrotou um {1} e ganhou {2} poções, {3} de experiência e {4} moedas de ouro.", nome, nomeInimigo, quantiaPocao, quantiaExperiencia, moedaGanha);
                break;
            }
            vida -= danoInimigo;
            Console.WriteLine("O {0} usou um golpe que deu {1} de dano.", nomeInimigo, danoInimigo);
            Console.WriteLine("Vida do mago {0}: {1}.", nome, vida);
            if (vida <= 0)
            {
                Console.WriteLine("O mago {0} morreu na batalha contra um {1}", nome, nomeInimigo);
                break;
            }
            if (espacosFeiticos <= 0)
            {
                Console.WriteLine("O mago {0} não tem mais feitiços e então fugiu da batalha.", nome);
                break;
            }
            if (vida < 40 && pocao > 0)
            {
                Console.WriteLine("O mago {0} está com apenas {1} de vida, você deseja usar uma poção de vida?", nome, vida);
                Console.WriteLine("Sim - Não");
                string resposta = Convert.ToString(Console.ReadLine()).ToLower();
                if (resposta.IndexOf("s") >= 0)
                {
                    BeberPocao();
                }
            }
            else if (vida < 40)
            {
                Console.WriteLine("O mago {0} está com apenas {1} de vida e você não possui poções. Você deseja fugir da batalha?", nome, vida);
                Console.WriteLine("Sim - Não");
                string resposta = Convert.ToString(Console.ReadLine()).ToLower();
                if (resposta.IndexOf("s") >= 0)
                {
                    Console.WriteLine("O mago {0} fugiu da batalha.", nome);
                    break;
                }
            }
            Thread.Sleep(6000);
            Console.Clear();
        }
    }

    public void FazerCompras()
    {
        Console.WriteLine("Bem vindo a loja da magia grande mago! O que você gostaria de comprar?");
        Console.WriteLine("Moedas de ouro que você possui: {0}", dinheiro);

        Console.WriteLine("1 - Espaço de feitiço: 10 moedas\n2 - Poção de vida: 50 moedas\n3 - Poção de experiência: 250 moedas");
        Console.WriteLine("4 - Aumentar dano do feitiço de {0}: 400 moedas\n5 - Aumentar dano do feitiço de {1}: 400 moedas\n6 - Aumentar dano do feitiço de {2}: 350 moedas", feitico[0], feitico[1], feitico[2]);
        int resposta = Convert.ToInt32(Console.ReadLine());

        switch (resposta)
        {
            case 1:
                if (dinheiro >= 10)
                {
                    Console.WriteLine("Você comprou um espaço de feitiço");
                    espacosFeiticos += 1;
                    dinheiro -= 10;
                    break;
                }
                else
                {
                    Console.WriteLine("Você não tem dinheiro suficiente para comprar isso.");
                    break;
                }
            case 2:
                if (dinheiro >= 50)
                {
                    Console.WriteLine("Você comprou uma poção de vida");
                    pocao += 1;
                    dinheiro -= 50;
                    break;
                }
                else
                {
                    Console.WriteLine("Você não tem dinheiro suficiente para comprar isso.");
                    break;
                }
            case 3:
                if (dinheiro >= 250)
                {
                    Console.WriteLine("Você comprou e usou uma poção de experiência");
                    Random aumentar = new Random();
                    experiencia += aumentar.Next(30, 51);
                    dinheiro -= 250;
                    break;
                }
                else
                {
                    Console.WriteLine("Você não tem dinheiro suficiente para comprar isso.");
                    break;
                }
            case 4:
                if (dinheiro >= 400)
                {
                    Console.WriteLine("Você comprou um aumento do dano do feitiço de {0}", feitico[0]);
                    Random aumento = new Random();
                    danoFeiticoPrincipal += aumento.Next(1, 4);
                    dinheiro -= 400;
                    break;
                }
                else
                {
                    Console.WriteLine("Você não tem dinheiro suficiente para comprar isso.");
                    break;
                }
            case 5:
                if (dinheiro >= 400)
                {
                    Console.WriteLine("Você comprou um aumento do dano do feitiço de {0}", feitico[1]);
                    Random aumento2 = new Random();
                    danoFeiticoSecundario += aumento2.Next(1, 4);
                    dinheiro -= 400;
                    break;
                }
                else
                {
                    Console.WriteLine("Você não tem dinheiro suficiente para comprar isso.");
                    break;
                }
            case 6:
                if (dinheiro >= 350)
                {
                    Console.WriteLine("Você comprou um aumento do dano do feitiço de {0}", feitico[2]);
                    Random aumento3 = new Random();
                    danoFeiticoTerciario += aumento3.Next(2, 6);
                    dinheiro -= 350;
                    break;
                }
                else
                {
                    Console.WriteLine("Você não tem dinheiro suficiente para comprar isso.");
                    break;
                }
            default:
                Console.WriteLine("Essa opção não existe.");
                break;
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
