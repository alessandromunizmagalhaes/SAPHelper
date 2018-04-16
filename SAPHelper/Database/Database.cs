using SAPbobsCOM;
using System;
using System.Collections.Generic;

namespace SAPHelper
{
    public static class Database
    {
        #region :: Gestão de Tabelas

        public static void CriarTabela(Tabela tabela)
        {
            bool tabela_is_UDO = tabela is TabelaUDO;
            TabelaUDO tabelaUDO = tabela_is_UDO ? (TabelaUDO)tabela : null;

            if (tabela_is_UDO)
            {
                foreach (var tabelaFilha in tabelaUDO.TabelasFilhas)
                {
                    // atenção para a recursividade aqui
                    CriarTabela(tabelaFilha);
                }
            }

            if (!ExisteTabela(tabela.NomeSemArroba))
            {
                CriarUserTable(tabela);

                foreach (var coluna in tabela.Colunas)
                {
                    if (!ExisteColuna(tabela.NomeComArroba, coluna.Nome))
                    {
                        CriarColuna(tabela.NomeComArroba, coluna);
                    }
                }

                // não tem como ao mesmo tempo que criar uma coluna, já marcar ela como udo
                // tem que criar todas as colunas e depois iterar denovo só pra adicionar o UDO
                // regras SAP, senão vc não consegue adicionar o UDO via DI.
                if (tabela_is_UDO)
                {
                    GC.Collect();
                    UserObjectsMD objUserObjectMD = Global.Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);

                    DefinirTabelaComoUDO(objUserObjectMD, tabelaUDO);

                    DefinirColunasComoUDO(objUserObjectMD, tabela.NomeSemArroba, tabela.Colunas, true);

                    DefinirTabelasComoFilhasDoUDO(objUserObjectMD, tabela.NomeSemArroba, tabelaUDO.TabelasFilhas);

                    if (objUserObjectMD.Add() != 0)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(objUserObjectMD);
                        objUserObjectMD = null;
                        GC.Collect();
                        throw new DatabaseException($"Erro ao tentar criar a tabela {tabela.NomeSemArroba} como UDO.\nErro: {Global.Company.GetLastErrorDescription()}");
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(objUserObjectMD);
                    objUserObjectMD = null;
                    GC.Collect();
                }
            }
        }

        private static void DefinirTabelasComoFilhasDoUDO(UserObjectsMD objUserObjectMD, string nomeTabelaPai, List<Tabela> tabelasFilhas)
        {
            foreach (var tabelaFilha in tabelasFilhas)
            {
                objUserObjectMD.ChildTables.TableName = tabelaFilha.NomeSemArroba;
                objUserObjectMD.ChildTables.Add();
            }
        }

        private static void DefinirTabelaComoUDO(UserObjectsMD objUserObjectMD, TabelaUDO tabela)
        {
            objUserObjectMD.TableName = tabela.NomeSemArroba;
            objUserObjectMD.Name = tabela.NomeSemArroba;
            objUserObjectMD.Code = tabela.NomeSemArroba;

            objUserObjectMD.CanCancel = tabela.CanCancel;
            objUserObjectMD.CanClose = tabela.CanClose;
            objUserObjectMD.CanCreateDefaultForm = tabela.CanCreateDefaultForm;
            objUserObjectMD.CanDelete = tabela.CanDelete;
            objUserObjectMD.CanFind = tabela.CanFind;
            objUserObjectMD.CanLog = tabela.CanLog;
            objUserObjectMD.CanYearTransfer = tabela.CanYearTransfer;
            objUserObjectMD.ManageSeries = tabela.ManageSeries;
            objUserObjectMD.ObjectType = tabela.ObjectType;
            objUserObjectMD.EnableEnhancedForm = tabela.EnableEnhancedForm;
        }

        private static void CriarUserTable(Tabela tabela)
        {
            GC.Collect();
            UserTablesMD oUserTablesMD = Global.Company.GetBusinessObject(BoObjectTypes.oUserTables);

            oUserTablesMD.TableName = tabela.NomeSemArroba;
            oUserTablesMD.TableDescription = tabela.Descricao;
            oUserTablesMD.TableType = tabela.Tipo;

            if (oUserTablesMD.Add() != 0)
            {
                throw new DatabaseException($"Erro ao tentar criar a tabela {tabela.NomeComArroba}.\nErro: {Global.Company.GetLastErrorDescription()}");
            }

            oUserTablesMD = null;
            GC.Collect();
        }

        public static void ExcluirTabela(string nomeSemArroba)
        {
            GC.Collect();
            UserObjectsMD oUDO = Global.Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);

            if (oUDO.GetByKey(nomeSemArroba))
            {
                if (oUDO.Remove() != 0)
                    throw new DatabaseException($"Erro ao tentar remover a definição de UDO da tabela {nomeSemArroba}.\nErro: {Global.Company.GetLastErrorDescription()}");
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUDO);
            oUDO = null;

            UserTablesMD objUserTablesMD = Global.Company.GetBusinessObject(BoObjectTypes.oUserTables);
            if (objUserTablesMD.GetByKey(nomeSemArroba))
            {
                objUserTablesMD.TableName = nomeSemArroba;

                if (objUserTablesMD.Remove() != 0)
                    throw new DatabaseException($"Erro ao tentar remover a tabela {nomeSemArroba}.\nErro: {Global.Company.GetLastErrorDescription()}");
            }
            else
            {
                throw new DatabaseException($"tabela {nomeSemArroba} não encontrada para realizar remoção.");
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(objUserTablesMD);
            objUserTablesMD = null;
        }

        public static bool ExisteTabela(string nome_tabela)
        {
            Recordset rs = Global.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            string sql = "SELECT COUNT(*) FROM OUTB WHERE TableName = '" + nome_tabela + "'";
            rs.DoQuery(sql);

            if (rs.Fields.Item(0).Value <= 0)
            {
                rs = null;
                sql = null;
                return false;
            }
            else
            {
                rs = null;
                sql = null;
                return true;
            }
        }

        #endregion


        #region :: Gestão de Campos

        public static void CriarCampo(string nome_tabela, Coluna coluna)
        {
            if (!ExisteColuna(nome_tabela, coluna.Nome))
            {
                CriarColuna(nome_tabela, coluna);
            }
        }

        private static void CriarColuna(string nome_tabela, Coluna coluna)
        {
            CriarUserField(nome_tabela, coluna);

            if (coluna.Obrigatoria)
            {
                SetarCampoComoObrigatorio(nome_tabela, coluna.Nome);
            }

            foreach (var valor_valido in coluna.ValoresValidos)
            {
                AdicionarValorValido(nome_tabela, coluna.Nome, valor_valido.Valor, valor_valido.Descricao);
            }

            if (!String.IsNullOrEmpty(coluna.ValorPadrao))
            {
                SetarValorPadrao(nome_tabela, coluna.Nome, coluna.ValorPadrao);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nome_tabela"></param>
        /// <param name="colunas"></param>
        /// <param name="criar_campo_code_antes">
        /// quando se está criando uma nova tabela
        /// tem que fazer essa gambiarra horrível, porque o primeiro elemento a ser colocado como UDO,
        /// tem que obrigatóriamente ser o campo CODE.
        /// como eu não passo ele como um campo que eu quero usar na definição de Lista de Colunas da minha Tabela
        /// sempre que for o primeiro, inventa uma coluna ficticia e passa o Code.
        /// horroroso mas é o jeito.
        /// </param>
        public static void DefinirColunasComoUDO(UserObjectsMD objUserObjectMD, string nome_tabela, List<Coluna> colunas, bool criar_campo_code_antes = false)
        {
            // quando se está criando uma nova tabela
            // tem que fazer essa gambiarra horrível, porque o primeiro elemento a ser colocado como UDO,
            // tem que obrigatóriamente ser o campo CODE.
            // como eu não passo ele como um campo que eu quero usar na definição de Lista de Colunas da minha Tabela
            // sempre que for o primeiro, inventa uma coluna ficticia e passa o Code.
            // horroroso mas é o jeito.
            if (criar_campo_code_antes)
            {
                AdicionarFindColumnsAoObjeto(objUserObjectMD, "Code", "Código");
                AdicionarFindColumnsAoObjeto(objUserObjectMD, "Name", "Descrição");
            }

            foreach (var coluna in colunas)
            {
                AdicionarFindColumnsAoObjeto(objUserObjectMD, coluna.NomeComU_NaFrente, coluna.Descricao);
            }
        }

        private static void CriarUserField(string nome_tabela, Coluna coluna)
        {
            GC.Collect();
            UserFieldsMD objUserFieldsMD = Global.Company.GetBusinessObject(BoObjectTypes.oUserFields);
            objUserFieldsMD.TableName = nome_tabela;
            objUserFieldsMD.Name = coluna.Nome;
            objUserFieldsMD.Description = coluna.Descricao;

            if (coluna is ColunaVarchar)
            {
                objUserFieldsMD.Type = BoFieldTypes.db_Alpha;
            }
            else if (coluna is ColunaText)
            {
                objUserFieldsMD.Type = BoFieldTypes.db_Memo;
            }
            else if (coluna is ColunaDate)
            {
                objUserFieldsMD.Type = BoFieldTypes.db_Date;
            }
            else if (coluna is ColunaInt)
            {
                objUserFieldsMD.Type = BoFieldTypes.db_Numeric;
            }
            else if (coluna is ColunaTime)
            {
                objUserFieldsMD.Type = BoFieldTypes.db_Date;
                objUserFieldsMD.SubType = BoFldSubTypes.st_Time;
            }
            else if (coluna is ColunaPercent)
            {
                objUserFieldsMD.Type = BoFieldTypes.db_Float;
                objUserFieldsMD.SubType = BoFldSubTypes.st_Percentage;
            }
            else if (coluna is ColunaSum)
            {
                objUserFieldsMD.Type = BoFieldTypes.db_Float;
                objUserFieldsMD.SubType = BoFldSubTypes.st_Sum;
            }
            else if (coluna is ColunaQuantity)
            {
                objUserFieldsMD.Type = BoFieldTypes.db_Float;
                objUserFieldsMD.SubType = BoFldSubTypes.st_Percentage;
            }
            else if (coluna is ColunaPrice)
            {
                objUserFieldsMD.Type = BoFieldTypes.db_Float;
                objUserFieldsMD.SubType = BoFldSubTypes.st_Price;
            }
            else if (coluna is ColunaLink)
            {
                objUserFieldsMD.Type = BoFieldTypes.db_Memo;
                objUserFieldsMD.SubType = BoFldSubTypes.st_Link;
            }
            else if (coluna is ColunaLink)
            {
                objUserFieldsMD.Type = BoFieldTypes.db_Alpha;
                objUserFieldsMD.SubType = BoFldSubTypes.st_Image;
            }

            if (!String.IsNullOrEmpty(coluna.TabelaNoObjectVinculada))
            {
                objUserFieldsMD.LinkedTable = coluna.TabelaNoObjectVinculada;
            }
            else if (!String.IsNullOrEmpty(coluna.TabelaUDOVinculada))
            {
                objUserFieldsMD.LinkedUDO = coluna.TabelaUDOVinculada;
            }
            else if (coluna.TabelaSistemaVinculada != BoObjectTypes.BoRecordset)
            {
                objUserFieldsMD.LinkedSystemObject = coluna.TabelaSistemaVinculada;
            }

            if (coluna.Tamanho > 0)
            {
                objUserFieldsMD.EditSize = coluna.Tamanho;
            }


            if (objUserFieldsMD.Add() != 0)
            {
                throw new DatabaseException($"Erro ao tentar criar o campo {coluna.Nome}.\nErro: {Global.Company.GetLastErrorDescription()}");
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(objUserFieldsMD);
            objUserFieldsMD = null;
        }

        private static void AdicionarFindColumnsAoObjeto(UserObjectsMD objUserObjectMD, string nome_coluna, string descricao_coluna)
        {
            objUserObjectMD.FindColumns.ColumnAlias = nome_coluna;
            objUserObjectMD.FindColumns.ColumnDescription = descricao_coluna;
            objUserObjectMD.FindColumns.Add();
        }

        private static void AdicionarFormColumnsAoObjeto(UserObjectsMD objUserObjectMD, string nome_coluna, string descricao_coluna)
        {
            objUserObjectMD.FormColumns.FormColumnAlias = nome_coluna;
            objUserObjectMD.FormColumns.FormColumnDescription = descricao_coluna;
            objUserObjectMD.FormColumns.Add();
        }

        public static void ExcluirColuna(string nome_tabela, string nome_campo)
        {
            int FieldId = GetFieldId(nome_tabela, nome_campo);

            GC.Collect();
            UserFieldsMD oUserFieldsMD = Global.Company.GetBusinessObject(BoObjectTypes.oUserFields);
            if (oUserFieldsMD.GetByKey(nome_tabela, FieldId))
            {
                if (oUserFieldsMD.Remove() != 0)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                    throw new DatabaseException($"Erro ao tentar remover o campo {nome_campo} da tabela {nome_tabela}.\nErro: {Global.Company.GetLastErrorDescription()}");
                }
            }
            else
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                throw new DatabaseException($"Campo {nome_campo} não encontrado na tabela {nome_tabela} para realizar a exclusão.");
            }
        }

        public static bool ExisteColuna(string nome_tabela, string nome_campo)
        {
            Recordset rs = Global.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            var sql = $"SELECT COUNT(*) FROM CUFD (NOLOCK) WHERE TableID='{nome_tabela}' and AliasID='{nome_campo}'";

            rs.DoQuery(sql);
            if (rs.Fields.Item(0).Value <= 0)
            {
                rs = null;
                return false;
            }
            else
            {
                rs = null;
                return true;
            }
        }

        #endregion


        #region :: Valores válidos, Valores Padrão e Obrigatoriedade

        public static void AdicionarValorValido(string nome_tabela, string nome_campo, string valor, string descricao)
        {
            bool valorExiste = false;
            int campoID = GetFieldId(nome_tabela, nome_campo);

            if (ExisteValorValido(nome_tabela, campoID, valor))
            {
                valorExiste = true;
            }
            else
            {
                GC.Collect();
                UserFieldsMD objUserFieldsMD = Global.Company.GetBusinessObject(BoObjectTypes.oUserFields);
                objUserFieldsMD.GetByKey(nome_tabela, campoID);
                ValidValuesMD oValidValues;
                oValidValues = objUserFieldsMD.ValidValues;

                var error_code = 0;
                if (!valorExiste)
                {
                    if (oValidValues.Value != "")
                    {
                        oValidValues.Add();
                        oValidValues.SetCurrentLine(oValidValues.Count - 1);
                        oValidValues.Value = valor;
                        oValidValues.Description = descricao;
                        error_code = objUserFieldsMD.Update();
                    }
                    else
                    {
                        oValidValues.SetCurrentLine(oValidValues.Count - 1);
                        oValidValues.Value = valor;
                        oValidValues.Description = descricao;
                        error_code = objUserFieldsMD.Update();
                    }

                    if (error_code != 0)
                    {
                        throw new DatabaseException($"Erro ao tentar adicionar valor válido {valor} na coluna {nome_campo} na tabela {nome_tabela}.\nErro: {Global.Company.GetLastErrorDescription()}");
                    }
                }
                else
                {
                    error_code = objUserFieldsMD.Update();
                    if (error_code != 0)
                    {
                        throw new DatabaseException($"Erro ao tentar atualizar valor válido {valor} na coluna {nome_campo} na tabela {nome_tabela}.\nErro: {Global.Company.GetLastErrorDescription()}");
                    }
                }
            }
        }

        public static bool SetarCampoComoObrigatorio(string nome_tabela, string nome_campo)
        {
            int campoID = GetFieldId(nome_tabela, nome_campo);
            UserFieldsMD objUserFieldsMD;
            GC.Collect();
            objUserFieldsMD = Global.Company.GetBusinessObject(BoObjectTypes.oUserFields);

            if (objUserFieldsMD.GetByKey(nome_tabela, campoID))
            {
                objUserFieldsMD.Mandatory = BoYesNoEnum.tYES;

                if (objUserFieldsMD.Update() != 0)
                {
                    throw new DatabaseException($"Erro ao tentar tornar o campo {nome_campo} da tabela {nome_tabela} obrigatório.\nErro: {Global.Company.GetLastErrorDescription()}");
                }
            }
            return true;
        }

        public static bool SetarValorPadrao(string nome_tabela, string nome_campo, string valor)
        {
            bool valorExiste = false;
            int campoID = GetFieldId(nome_tabela, nome_campo);

            if (ExisteValorValido(nome_tabela, campoID, valor))
            {
                valorExiste = true;
            }

            //se existe esse valor válido
            if (valorExiste && (ExisteValorPadraoSetado(nome_tabela, campoID, valor)) == false)
            {
                GC.Collect();
                UserFieldsMD objUserFieldsMD;
                objUserFieldsMD = Global.Company.GetBusinessObject(BoObjectTypes.oUserFields);

                if (objUserFieldsMD.GetByKey(nome_tabela, campoID))
                    objUserFieldsMD.DefaultValue = valor;

                if (objUserFieldsMD.Update() != 0)
                {
                    objUserFieldsMD = null;
                    throw new DatabaseException($"Erro ao tentar setar o valor padrão {valor} para o campo {nome_campo} da tabela {nome_tabela}.\nErro: {Global.Company.GetLastErrorDescription()}");
                }
                else
                {
                    objUserFieldsMD = null;
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool ExisteValorValido(string nome_tabela, int campoID, string valor)
        {
            Recordset rs = Global.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            string sql =
                $@"SELECT COUNT(*) FROM UFD1 (NOLOCK) 
                    WHERE TableID='{nome_tabela}' AND
                        FieldID='{campoID}' AND
                        FldValue='{valor}'";

            rs.DoQuery(sql);
            if (rs.Fields.Item(0).Value > 0)
            {
                rs = null;
                return true;
            }

            rs = null;
            return false;
        }

        public static bool ExisteValorPadraoSetado(string nome_tabela, int campoID, string valor)
        {
            Recordset rs = Global.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            string sql = $@"SELECT COUNT(*) FROM CUFD (NOLOCK) 
            Where TableID='{nome_tabela}' AND
                  FieldID='{campoID}' AND
                  dflt='{valor}'";
            rs.DoQuery(sql);

            if ((rs.Fields.Item(0).Value) <= 0)
            {
                rs = null;
                return false;
            }
            else
            {
                rs = null;
                return true;
            }
        }

        #endregion


        #region :: Helpers

        public static int GetFieldId(string nome_tabela, string nome_campo)
        {
            Recordset rs = Global.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            string sql = $@" SELECT FieldID FROM CUFD (NOLOCK)  WHERE TableID='{nome_tabela}' AND AliasID='{nome_campo}'";
            rs.DoQuery(sql);
            if (rs.Fields.Item(0).Value >= 0)
            {
                return rs.Fields.Item(0).Value;
            }
            else
            {
                rs = null;
                return -1;
            }
        }

        #endregion

    }
}