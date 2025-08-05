namespace teste_tecnico.Models
{
    public class PedidoItem
    {
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; }
        public decimal ValorUnitario { get; set; }
        public int Quantidade { get; set; }
        public decimal SubTotal => ValorUnitario * Quantidade;
    }
}