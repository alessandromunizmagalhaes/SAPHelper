using SAPbobsCOM;
using System.Collections.Generic;

namespace SAPHelper
{
    public class Coluna
    {
        protected Coluna(string nome, string descricao, bool obrigatoria = false, string tabelaNoObjectVinculada = "", string tabelaUDOVinculada = "", BoObjectTypes tabelaSistemaVinculada = BoObjectTypes.BoRecordset)
        {
            Nome = nome;
            Descricao = descricao;
            Obrigatoria = obrigatoria;
            TabelaNoObjectVinculada = tabelaNoObjectVinculada;
            TabelaUDOVinculada = tabelaUDOVinculada;
            TabelaSistemaVinculada = tabelaSistemaVinculada;
        }

        public string NomeComU_NaFrente
        {
            get { return "U_" + Nome; }
        }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Obrigatoria { get; set; } = false;
        public string ValorPadrao { get; set; } = "";
        public List<ValorValido> ValoresValidos { get; set; } = new List<ValorValido>() { };
        public int Tamanho { get; set; } = -1;
        public string TabelaNoObjectVinculada { get; set; }
        public string TabelaUDOVinculada { get; set; }

        // BoObjectTypes.BoRecordset usado como default. não pode usar ele, então eu uso como se não tivesse nenhum setado
        public BoObjectTypes TabelaSistemaVinculada { get; set; }
    }
}