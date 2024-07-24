
Console.WriteLine("Digite o nome do seu mago: ");
string nomeRecebido = Convert.ToString(Console.ReadLine());

Console.WriteLine("Digite o feitiço de seu mago: ");
string feiticoRecebido = Convert.ToString(Console.ReadLine());

CriarMago mago01 = new CriarMago(nomeRecebido, feiticoRecebido);

Console.WriteLine("Existem " + CriarMago.Contador + " magos criados.");

Console.ReadKey();

class CriarMago
{
    public string? nome { get; set; }
    public string? feitico { get; set; }
    private int espacosFeiticos;
    private float experiencia;

    public static int Contador;

    public CriarMago(string _nome, string _feitico)
    {
        nome = _nome;
        feitico = _feitico;
        espacosFeiticos = 2;
        experiencia = 0f;

        Contador ++;
    }

    public void ChamarFeitico()
    {
        if (espacosFeiticos > 0)
        {
            Console.WriteLine("O mago " + nome + " usou o feitiço de " + feitico);
            espacosFeiticos--;
            Random aumento = new Random();
            float aumentoExperiencia = aumento.Next(3, 7) / 10f;
            experiencia += aumentoExperiencia;
        }
        else
        {
            Console.WriteLine("O mago " + nome + " não tem espaços suficiente para usar um feitiço de " + feitico);
        }
    }

    public void Meditar()
    {
        Random numeroHoras = new Random();
        Console.Write("O mago " + nome + " meditou por " + numeroHoras.Next(3, 6) + " horas.\n");
        espacosFeiticos = espacosFeiticos + 2;
    }

    public void Dados()
    {
        Console.Write("O mago " + nome + " tem como habilidade o feitiço de " + feitico);
        Console.Write("Ele tem xp de " + experiencia + " e possui ainda " + espacosFeiticos + " espaços de feitiço.");
    }
}
