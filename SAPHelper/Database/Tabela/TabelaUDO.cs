﻿using SAPbobsCOM;
using System.Collections.Generic;

namespace SAPHelper
{
    public class TabelaUDO : Tabela
    {
        public TabelaUDO(string nome, string descricao, BoUTBTableType tipo, List<Coluna> colunas, UDOParams udoParams, List<Tabela> tabelasFilhas = null) : base(nome, descricao, tipo, colunas)
        {
            if (!TipoUDOValido(this))
                throw new DatabaseException($"Erro ao instanciar tabela UDO. O tipo {tipo} não pode ser utilizado em tabelas UDO.");

            CanCancel = udoParams.CanCancel;
            CanClose = udoParams.CanClose;
            CanCreateDefaultForm = udoParams.CanCreateDefaultForm;
            CanDelete = udoParams.CanDelete;
            CanFind = udoParams.CanFind;
            CanLog = udoParams.CanLog;
            CanYearTransfer = udoParams.CanYearTransfer;
            ManageSeries = udoParams.ManageSeries;

            if (tabelasFilhas != null)
            {
                foreach (var tabelaFilha in tabelasFilhas)
                {
                    if (tabelaFilha is TabelaUDO)
                    {
                        throw new DatabaseException($"A tabela filha {tabelaFilha.NomeSemArroba} não pode ser do tipo UDO na declaração do objeto");
                    }

                    if (!TipoTabelaFilhaIgualTipoTabelaPai(tabelaFilha, this))
                    {
                        throw new DatabaseException($"O tipo da tabela filha {tabelaFilha.NomeSemArroba} é diferente do tipo da tabela pai {this.NomeSemArroba}");
                    }
                }

                TabelasFilhas = tabelasFilhas;
            }
        }

        private static bool TipoUDOValido(Tabela tabela)
        {
            return tabela.Tipo == BoUTBTableType.bott_Document || tabela.Tipo == BoUTBTableType.bott_MasterData;
        }

        private bool TipoTabelaFilhaIgualTipoTabelaPai(Tabela tabelaFilha, Tabela tabelaPai)
        {
            bool res = false;
            if (tabelaPai.Tipo == BoUTBTableType.bott_MasterData)
            {
                res = tabelaFilha.Tipo == BoUTBTableType.bott_MasterDataLines;
            }
            else if (tabelaPai.Tipo == BoUTBTableType.bott_Document)
            {
                res = tabelaFilha.Tipo == BoUTBTableType.bott_DocumentLines;
            }
            return res;
        }

        public List<Tabela> TabelasFilhas { get; set; } = new List<Tabela>() { };
        public BoYesNoEnum CanCancel { get; set; }
        public BoYesNoEnum CanClose { get; set; }
        public BoYesNoEnum CanCreateDefaultForm { get; set; }
        public BoYesNoEnum CanDelete { get; set; }
        public BoYesNoEnum CanFind { get; set; }
        public BoYesNoEnum CanLog { get; set; }
        public BoYesNoEnum CanYearTransfer { get; set; }
        public BoYesNoEnum ManageSeries { get; set; }
        public BoUDOObjType ObjectType
        {
            get
            {
                if (Tipo == BoUTBTableType.bott_Document || Tipo == BoUTBTableType.bott_DocumentLines)
                {
                    return BoUDOObjType.boud_Document;
                }
                else if (Tipo == BoUTBTableType.bott_MasterData || Tipo == BoUTBTableType.bott_MasterDataLines)
                {
                    return BoUDOObjType.boud_MasterData;
                }
                else
                {
                    return BoUDOObjType.boud_MasterData;
                }
            }
        }
    }
}
