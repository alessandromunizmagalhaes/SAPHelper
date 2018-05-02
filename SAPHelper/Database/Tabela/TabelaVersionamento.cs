using SAPbobsCOM;

namespace SAPHelper
{
    internal class TabelaVersionamento : Tabela
    {
        public Coluna Versao { get { return new ColunaQuantity("Versao", "Número da Versão", true); } }
        public Coluna Data { get { return new ColunaDate("Data", "Data de Criação", true); } }

        public TabelaVersionamento() : base("UPD_VERSIONAMENTO", "Internal table structure", BoUTBTableType.bott_NoObject)
        {

        }

        public void InserirNovaVersao(double novaVersao)
        {
            string insert =
                $@"DECLARE @table_id INT;
					IF(SELECT COUNT(*) FROM [{NomeComArroba}]) > 0
						SELECT @table_id = (SELECT TOP (1) (CAST(Code as INT) + 1) FROM [{NomeComArroba}] ORDER BY CAST(Code as INT) DESC) 
					ELSE
						SELECT @table_id = '1'; 

							INSERT INTO [{NomeComArroba}]
							    (Code, name, {Data.NomeComU_NaFrente}, {Versao.NomeComU_NaFrente})
							VALUES
							    (@table_id, @table_id, GETDATE(), {Helpers.ToString(novaVersao)});";

            Helpers.DoQuery(insert);
        }

        public double GetCurrentVersion()
        {
            var rs = Helpers.DoQuery($@"SELECT MAX({Versao.NomeComU_NaFrente}) as versao FROM [{NomeComArroba}]");
            if (rs.Fields.Item("versao").IsNull() == BoYesNoEnum.tYES)
            {
                // quando for a primeira vez, o max do banco retornará null e essa função
                // retornará um número maior para que rode todas as versões
                return 9999.9;
            }
            else
            {
                return rs.Fields.Item("versao").Value;
            }
        }
    }
}