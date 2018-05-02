namespace SAPHelper
{
    public class ComboFormObrigatorio : ComboForm, IItemFormObrigatorio
    {
        public string Mensagem { get; set; }
        public string AbaUID { get; set; }
    }
}
