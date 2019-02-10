namespace Senai.TipoEvento.Wep.Api.Dominio
{
    public class TipoEventoDominio
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public TipoEventoDominio(int id, string nome)
        {
            Id = id;
            Nome = nome;
        }
    }
}
