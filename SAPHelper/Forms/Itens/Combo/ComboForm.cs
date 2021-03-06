﻿using SAPbouiCOM;
using System;
using System.Collections.Generic;

namespace SAPHelper
{
    public class ComboForm : ItemForm
    {
        public string SQL { get; set; }
        public Dictionary<string, string> ValoresPadrao { get; set; }

        public void Popular(SAPbouiCOM.Form form)
        {
            ComboBox comboBox = ((ComboBox)form.Items.Item(ItemUID).Specific);
            Popular(comboBox.ValidValues);
        }

        public void Popular(SAPbouiCOM.Form form, string matrixUID)
        {
            Popular(((Matrix)form.Items.Item(matrixUID).Specific).Columns.Item(ItemUID).ValidValues);
        }

        public void Popular(Matrix mtx)
        {
            Popular(mtx.Columns.Item(ItemUID).ValidValues);
        }

        public void Popular(Column coluna)
        {
            Popular(coluna.ValidValues);
        }

        public void Popular(ValidValues validValues)
        {
            RemoverTodosValoresValidados(validValues);
            AcrescentarValoresValidados(validValues);
        }

        public void AcrescentarValoresValidados(ValidValues validValues)
        {
            if (!string.IsNullOrEmpty(SQL))
            {
                using (var recordset = new RecordSet())
                {
                    var rs = recordset.DoQuery(SQL);
                    while (!rs.EoF)
                    {
                        validValues.Add(rs.Fields.Item(0).Value.ToString(), rs.Fields.Item(1).Value.ToString());

                        rs.MoveNext();
                    }
                }
            }
            else if (ValoresPadrao != null)
            {
                foreach (var item in ValoresPadrao)
                {
                    validValues.Add(item.Key, item.Value);
                }
            }
            else
            {
                throw new MissingFieldException($"Nenhum método de população (SQL ou ValorPadrão) foi definido para o Item {ItemUID}");
            }
        }

        public void RemoverTodosValoresValidados(ValidValues validValues, bool removerValorDefault = false)
        {
            var count = validValues.Count;
            for (int i = 0; i < count; i++)
            {
                if (!removerValorDefault)
                {
                    string validValue = validValues.Item(0).Value;
                    if (string.IsNullOrEmpty(validValue))
                    {
                        continue;
                    }
                }

                validValues.Remove(0, BoSearchKey.psk_Index);
            }
        }

    }
}
