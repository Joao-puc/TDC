namespace TDC.Data
{
    public class ItemGrupoPais
    {
        public string country { get; set; }
        public double? total_vaccinations { get; set; }
        public double? people_vaccinated { get; set; }
        public double? people_fully_vaccinated { get; set; }
        public double? daily_vaccinations { get; set; }
        public double? total_vaccinations_per_hundred { get; set; }
        public double? people_vaccinated_per_hundred { get; set; }
        public double? people_fully_vaccinated_per_hundred { get; set; }
        public double? daily_vaccinations_per_million { get; set; }
        public int? vaccines_count { get; set; }
        public string regiao { get; set; }
        public double populacao { get; set; }
        public double populacao_densidade { get; set; }
    }
}