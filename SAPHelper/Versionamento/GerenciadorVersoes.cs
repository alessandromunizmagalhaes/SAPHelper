using System.Collections.Generic;
using System.Linq;

namespace SAPHelper
{
    public static class GerenciadorVersoes
    {
        public static void Aplicar(Database db, List<Versionamento> versoes, double versaoAddon)
        {
            var tabelaVersionamento = new TabelaVersionamento();
            var versaoDB = tabelaVersionamento.GetCurrentVersion();
            if (versaoDB < versaoAddon)
            {
                var versoesNaoAplicadas = versoes.OrderBy(v => v.Versao).Where(v => (v.Versao > versaoDB && v.Versao <= versaoAddon));
                foreach (var versao in versoesNaoAplicadas)
                {
                    versao.Aplicar(db);

                    tabelaVersionamento.InserirNovaVersao(versao.Versao);
                }
            }
        }
    }
}
