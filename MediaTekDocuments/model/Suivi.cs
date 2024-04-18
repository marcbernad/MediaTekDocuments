

namespace MediaTekDocuments.model
{
    public class Suivi
    {
        public int IdSuivi;
        public string Etape;

        public Suivi(int idSuivi, string etape)
        {
            this.IdSuivi = idSuivi;
            this.Etape = etape;
        }
    }
}
