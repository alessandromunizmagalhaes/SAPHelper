using SAPbobsCOM;

namespace SAPHelper
{
    public class TabelaCadastroBasico : TabelaUDO
    {
        public TabelaCadastroBasico(string nome, string descricao) : base(nome, descricao, BoUTBTableType.bott_MasterData, new UDOParams() { CanDelete = BoYesNoEnum.tNO })
        {

        }
    }
}
