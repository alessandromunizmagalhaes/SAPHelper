namespace SAPHelper
{
    public class ComboAtivoForm : ComboForm
    {
        public new string ItemUID { get { return "Ativo"; } }
        public new string Datasource { get { return "U_Ativo"; } }
        public string ValueYes { get { return "Y"; } }
        public string ValueNo { get { return "N"; } }
    }
}
