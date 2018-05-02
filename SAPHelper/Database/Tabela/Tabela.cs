using SAPbobsCOM;
using System.Collections.Generic;
using System.Linq;

namespace SAPHelper
{
    public abstract class Tabela
    {
        private string _nome;

        public Tabela(string nome, string descricao, BoUTBTableType tipo)
        {
            _nome = nome;
            DescricaoTabela = descricao;
            TipoTabela = tipo;
        }

        public string NomeComArroba { get { return _nome[0] == '@' ? _nome : "@" + _nome; } }
        public string NomeSemArroba { get { return _nome[0] == '@' ? _nome.Remove(0, 1) : _nome; } }
        public string DescricaoTabela { get; set; }
        public BoUTBTableType TipoTabela { get; set; }

        public virtual List<Coluna> GetColunas()
        {
            var propOfTypeColuna = GetType().GetProperties().Where(p => typeof(Coluna).IsAssignableFrom(p.PropertyType));
            var res = new List<Coluna>() { };

            foreach (var prop in propOfTypeColuna)
            {
                res.Add((Coluna)prop.GetValue(this));
            }
            return res;
        }
    }
}