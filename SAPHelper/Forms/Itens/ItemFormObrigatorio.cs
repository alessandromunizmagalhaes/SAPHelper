namespace SAPHelper
{
    public class ItemFormObrigatorio : ItemForm, IItemFormObrigatorio
    {
        public string Mensagem { get; set; }
        public string AbaUID { get; set; }
    }
}
