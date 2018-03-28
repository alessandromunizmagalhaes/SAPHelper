using SAPbobsCOM;
using System.Collections.Generic;

namespace SAPHelper
{
    public class ColunaVarchar : Coluna
    {
        public ColunaVarchar(string nome, string descricao, int tamanho = 0, bool obrigatorio = false, string valor_padrao = "", List<ValorValido> valorValido = null, string tabelaNoObjectVinculada = "", string tabelaUDOVinculada = "", BoObjectTypes tabelaSistemaVinculada = BoObjectTypes.BoRecordset) : base(nome, descricao, obrigatorio, tabelaNoObjectVinculada, tabelaUDOVinculada, tabelaSistemaVinculada)
        {
            Tamanho = tamanho;
            ValorPadrao = ValorPadrao;
            if (valorValido != null)
            {
                ValoresValidos = valorValido;
            }
        }
    }
}
