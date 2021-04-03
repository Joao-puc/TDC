
using System;
using TDC.Extensions;

namespace TDC.Data
{
    public class RegistroVacinacaoPaisCompleto
    {
        public string country { get; set; }
        public string iso_code { get; set; }
        public string date { get; set; }
        public double? total_vaccinations { get; set; }
        public double? people_vaccinated { get; set; }
        public double? people_fully_vaccinated { get; set; }
        public double? daily_vaccinations_raw { get; set; }
        public double? daily_vaccinations { get; set; }
        public double? total_vaccinations_per_hundred { get; set; }
        public double? people_vaccinated_per_hundred { get; set; }
        public double? people_fully_vaccinated_per_hundred { get; set; }
        public double? daily_vaccinations_per_million { get; set; }
        public int? vaccines_count { get; set; }
        public string vaccines { get; set; }
        public string source_name { get; set; }
        public string source_website { get; set; }

        public static RegistroVacinacaoPaisCompleto TryNew(string linha)
        {
            var valores = linha.Split(',');

            try
            {

                if (valores.Length == 15)
                {
                    return new RegistroVacinacaoPaisCompleto()
                    {
                        country = valores[0],
                        iso_code = valores[1],
                        date = valores[2],
                        total_vaccinations = valores[3].ToDouble(),
                        people_vaccinated = valores[4].ToDouble(),
                        people_fully_vaccinated = valores[5].ToDouble(),
                        daily_vaccinations_raw = valores[6].ToDouble(),
                        daily_vaccinations = valores[7].ToDouble(),
                        total_vaccinations_per_hundred = valores[8].ToDouble(),
                        people_vaccinated_per_hundred = valores[9].ToDouble(),
                        people_fully_vaccinated_per_hundred = valores[10].ToDouble(),
                        daily_vaccinations_per_million = valores[11].ToDouble(),
                        vaccines = valores[12],
                        source_name = valores[13],
                        source_website = valores[14],
                        vaccines_count = valores[12].Split("|")?.Length ?? 0
                    };
                }
                else
                {
                    Console.WriteLine(linha);
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(linha);
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            return null;
        }
    }
}