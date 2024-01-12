namespace CaixaComanda.Classes
{
    public class Usuario
    {
        public string Id { get; set; }
        public string NomeUsuario { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public DateTime DataCriacao { get; set; }
        public string NomeCompleto { get; set; }
        public DateTime Nascimento { get; set; }
        public string Tipo { get; set; }
    }

    public class Produto
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Preco { get; set; }
        public string Grupo { get; set; }
        public string Foto { get; set; }
        public string Peso { get; set; }
        public string Quantidade { get; set; }
        public string Observacao { get; set; }
    }
        
    public class CampoGenerico
    {
        public string Campo { get; set; }
        public string Valor { get; set; }
    }
}