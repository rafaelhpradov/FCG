namespace FCG.Models
{
    public class Game : EntityBase
    {
        /// <summary>
        /// Produtora do jogo. Máximo 100 caracteres.
        /// </summary>
        public required string Produtora { get; set; }

        /// <summary>
        /// Descrição do jogo. Máximo 500 caracteres.
        /// </summary>
        public required string Descricao { get; set; }

        /// <summary>
        /// Preço do jogo. Formato: 0.00
        /// </summary>
        public required decimal Preco { get; set; }

        /// <summary>
        /// Data de lançamento do jogo. Formato: yyyy-MM-dd
        /// </summary>
        public required DateTime DataLancamento { get; set; }

        #region [Navegacao]
        public virtual ICollection<Pedido> Pedidos { get; set; }
        #endregion
    }
}
