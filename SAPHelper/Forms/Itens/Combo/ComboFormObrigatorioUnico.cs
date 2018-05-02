namespace SAPHelper
{
    public class ComboFormObrigatorioUnico : ComboForm, IItemFormObrigatorioUnico
    {
        public string MensagemQuandoObrigatorio { get; set; }
        public string MensagemQuandoUnico { get; set; }
        public string AbaUID { get; set; }
    }
}
