using System.Text.RegularExpressions;
using TDC.Extensions;

namespace TDC.Data
{
    public class DadosPais
    {
        private static Regex _myRegex = new Regex("\"(.*?)\"", RegexOptions.IgnoreCase);

        public string nome { get; set; }
        public string regiao { get; set; }
        public double populacao { get; set; }
        public double populacao_densidade { get; set; }

        public static DadosPais TryNew(string linha)
        {
            try
            {
                linha = linha.Replace("'", "");
                bool achou = false;
                do
                {
                    var match = _myRegex.Match(linha);
                    if (match.Success)
                    {
                        linha = linha.Replace(match.Value, match.Value.Replace(",", "|").Replace("\"", ""));
                        achou = true;
                    }
                    else
                        achou = false;
                }
                while (achou);

                var campos = linha.Split(",");
                //country,Region,Surface area (km2),Population in thousands (2017),"Population density (per km2, 2017)"
                return new DadosPais()
                {
                    nome = campos[0],
                    regiao = campos[1],
                    populacao = campos[3].ToDouble() ?? 0,
                    populacao_densidade = campos[4].ToDouble() ?? 0
                };
            }
            catch { return null; }
        }
    }
}