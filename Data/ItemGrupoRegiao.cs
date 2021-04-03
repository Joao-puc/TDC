namespace TDC.Data
{
    public class ItemGrupoRegiao
    {
        public string regiao { get; set; }
        public double? total_vaccinations { get; set; }
        public double? people_vaccinated { get; set; }
        public double? people_fully_vaccinated { get; set; }
        public double? daily_vaccinations { get; set; }
        public double? total_vaccinations_per_hundred { get; set; }
        public double? people_vaccinated_per_hundred { get; set; }
        public double? people_fully_vaccinated_per_hundred { get; set; }
        public double? daily_vaccinations_per_million { get; set; }
        public double populacao { get; set; }
        public double populacao_densidade { get; set; }
    }
}