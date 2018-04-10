using System.Collections.Generic;

namespace SAPHelper
{
    public class ColunaAtivo : ColunaVarchar
    {
        public ColunaAtivo(string nome = "Ativo") : base(nome, "Ativo", 1, false, "Y", new List<ValorValido>(){
                                    new ValorValido("Y","Ativo"),
                                    new ValorValido("N","Inativo"),
                                })
        {

        }
    }
}
