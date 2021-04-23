using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TDC.Data;
using TDC.Util;

namespace TDC
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!File.Exists("country_vaccinations.csv"))
                return;

            //Carrega o arquivo linha a linha
            var arquivo = File.ReadAllLines("country_vaccinations.csv");

            //Define o pattern de busca para tratar quando houver aspas dentro da linha
            Regex myRegex = new Regex("\"(.*?)\"", RegexOptions.IgnoreCase);

            //instancia uma lista onde será armazenado os registros do arquivo original
            var listaDados = new List<RegistroVacinacaoPaisCompleto>();

            for (int i = 1; i < arquivo.Length; i++)
            {
                var linha = arquivo[i];

                //remove do texto o caracter ' (aspas simples) pois o mesmo apresenta erro quando é processado no weeka
                linha = linha.Replace("'", "");

                //flag de controle do loop
                bool achou = false;
                do
                {
                    //enquanto não achar o pattern na linha continua o processamento, desta forma todos os textos que estão sendo tratados entre aspas podem ser processados como string e ter o caracter de separação tratado
                    var match = myRegex.Match(linha);
                    if (match.Success)
                    {
                        //substitui a linha para que as virgulas dentro do campo de texto (entre aspas) sejam removidas, para que não apresente erro ao separar as linhas em colunas
                        linha = linha.Replace(match.Value, match.Value.Replace(",", "|").Replace("\"", ""));
                        achou = true;
                    }
                    else
                        achou = false;
                }
                while (achou);

                //converte a linha em objeto, inserindo o registro na lista de registros
                var item = RegistroVacinacaoPaisCompleto.TryNew(linha);
                if (item != null)
                    listaDados.Add(item);
            }

            Console.WriteLine($"{listaDados.Count} linhas validas encontradas de {arquivo.Length - 1}");

            //Salva o arquivo original com tratamento dos dados para poder ser agrupado
            var saida = CsvUtil.ToCsv(",", listaDados);
            File.WriteAllText($"country_vaccinations_{DateTime.Now:yyyy-MM-ddTHHmmss}.csv", saida);

            var dadosPais = File.ReadAllLines("country_profile_variables.csv");
            //Carrega a lista com os dados do país
            var listaDadosPais = new List<DadosPais>();
            foreach (var item in dadosPais)
            {
                var dados = DadosPais.TryNew(item);
                if (dados != null)
                    listaDadosPais.Add(dados);
            }

            //Realiza o agrupamento das informações por pais, integrando os dados com a regiao do pais
            var agrupamentoPais = new List<ItemGrupoPais>();
            agrupamentoPais = listaDados.GroupBy(x => x.country).Select(y => new ItemGrupoPais()
            {
                country = y.Key,
                daily_vaccinations = y.Max(x => x.daily_vaccinations),
                daily_vaccinations_per_million = y.Max(x => x.daily_vaccinations_per_million),
                people_fully_vaccinated = y.Max(x => x.people_fully_vaccinated),
                people_fully_vaccinated_per_hundred = y.Max(x => x.people_fully_vaccinated_per_hundred),
                total_vaccinations = y.Max(x => x.total_vaccinations),
                total_vaccinations_per_hundred = y.Max(x => x.total_vaccinations_per_hundred),
                vaccines_count = y.Max(x => x.vaccines_count),
                people_vaccinated = y.Max(x => x.people_vaccinated),
                people_vaccinated_per_hundred = y.Max(x => x.people_vaccinated_per_hundred),
                populacao = listaDadosPais.FirstOrDefault(x => x.nome.ToUpper() == y.Key.ToUpper())?.populacao ?? 0,
                populacao_densidade = listaDadosPais.FirstOrDefault(x => x.nome.ToUpper() == y.Key.ToUpper())?.populacao_densidade ?? 0,
                regiao = listaDadosPais.FirstOrDefault(x => x.nome.ToUpper() == y.Key.ToUpper())?.regiao ?? "Nao identificada"
            }).ToList();

            //Salva o arquivo com os dados agrupados por pais
            var saidaPais = CsvUtil.ToCsv<ItemGrupoPais>(",", agrupamentoPais);
            File.WriteAllText($"group_by_country_{DateTime.Now:yyyy-MM-ddTHHmmss}.csv", saidaPais);

            //Realiza o agrupamento das informações por regiao
            var agrupamentoRegiao = new List<ItemGrupoRegiao>();
            agrupamentoRegiao = agrupamentoPais.GroupBy(x => x.regiao).Select(y => new ItemGrupoRegiao()
            {
                regiao = y.Key,
                daily_vaccinations = y.Sum(x => x.daily_vaccinations),
                daily_vaccinations_per_million = y.Sum(x => x.daily_vaccinations_per_million),
                people_fully_vaccinated = y.Sum(x => x.people_fully_vaccinated),
                people_fully_vaccinated_per_hundred = y.Average(x => x.people_fully_vaccinated_per_hundred),
                total_vaccinations = y.Sum(x => x.total_vaccinations),
                total_vaccinations_per_hundred = y.Average(x => x.total_vaccinations_per_hundred),
                people_vaccinated = y.Sum(x => x.people_vaccinated),
                people_vaccinated_per_hundred = y.Average(x => x.people_vaccinated_per_hundred),
                populacao = y.Sum(x => x.populacao),
                populacao_densidade = y.Sum(x => x.populacao_densidade),
            }).ToList();

            Console.WriteLine($"Encontradas {agrupamentoRegiao.Count} regioes");
            //Salva o arquivo com os dados agrupados por pais
            var saidaRegiao = CsvUtil.ToCsv<ItemGrupoRegiao>(",", agrupamentoRegiao);
            File.WriteAllText($"group_by_regiao_{DateTime.Now:yyyy-MM-ddTHHmmss}.csv", saidaRegiao);

            Console.WriteLine("Processamento finalizado");
            Console.ReadLine();
        }
    }
}
