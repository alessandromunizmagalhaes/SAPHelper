using SAPbouiCOM;
using System;
using System.Collections.Generic;

namespace SAPHelper
{
    public static class FormEvents
    {
        private static readonly Dictionary<string, Type> _mappedEventToForms = new Dictionary<string, Type>() { };

        public static void DeclararEventos(EventFilters eventFilters, List<MapEventsToForms> mappedEventsToForms)
        {
            foreach (var mappedEventToForms in mappedEventsToForms)
            {
                var eventFilter = eventFilters.Add(mappedEventToForms.EventType);
                foreach (var Form in mappedEventToForms.Forms)
                {
                    AddmappedEventToForm(eventFilter, mappedEventToForms.EventType, Form.FormType, Form);
                }
            }
        }

        private static void AddmappedEventToForm(EventFilter eventFilter, BoEventTypes eventType, string FormType, SAPHelper.Form FormObject)
        {
            eventFilter.AddEx(FormType);

            if (!_mappedEventToForms.ContainsKey(FormType))
            {
                _mappedEventToForms.Add(FormType, FormObject.GetType());
            }
        }

        public static void ItemEvent(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (_mappedEventToForms.ContainsKey(pVal.FormTypeEx))
                {
                    var FormObjType = Activator.CreateInstance(_mappedEventToForms[pVal.FormTypeEx]);

                    #region :: Form_LOAD

                    if (pVal.EventType == BoEventTypes.et_FORM_LOAD && Events.Antes(pVal))
                    {
                        ((SAPHelper.Form)FormObjType).OnBeforeFormLoad(FormUID, ref pVal, out BubbleEvent);
                    }
                    else if (pVal.EventType == BoEventTypes.et_FORM_LOAD && Events.Depois(pVal) && pVal.ActionSuccess)
                    {
                        ((SAPHelper.Form)FormObjType).OnAfterFormLoad(FormUID, ref pVal, out BubbleEvent);
                    }

                    #endregion

                    #region :: Form_Draw

                    else if (pVal.EventType == BoEventTypes.et_FORM_DRAW && Events.Antes(pVal))
                    {
                        ((SAPHelper.Form)FormObjType).OnBeforeFormDraw(FormUID, ref pVal, out BubbleEvent);
                    }
                    else if (pVal.EventType == BoEventTypes.et_FORM_DRAW && Events.Depois(pVal) && pVal.ActionSuccess)
                    {
                        ((SAPHelper.Form)FormObjType).OnAfterFormDraw(FormUID, ref pVal, out BubbleEvent);
                    }

                    #endregion

                    #region :: Form_Visible

                    // esse evento é muito louco.
                    // a propriedade BeforeAction vem as duas vezes como false,
                    // então não existe beforeaction, vem sempre como after
                    // só que a primeira vez que passa, não existe coleção de itens no form
                    // aí se eu tentar acessar um item neste evento, vai dar pau
                    // a solução foi verificar a quantidade de itens no form pra saber se é antes ou depois.
                    else if (pVal.EventType == BoEventTypes.et_FORM_VISIBLE)
                    {
                        SAPbouiCOM.Form form = Global.SBOApplication.Forms.Item(pVal.FormUID);

                        if (form.Items.Count == 0)
                        {
                            ((SAPHelper.Form)FormObjType).OnBeforeFormVisible(FormUID, ref pVal, out BubbleEvent);
                        }
                        else
                        {
                            ((SAPHelper.Form)FormObjType).OnAfterFormVisible(FormUID, ref pVal, out BubbleEvent);
                        }
                    }

                    #endregion

                    #region :: UDO_FORM_OPEN

                    else if (pVal.EventType == BoEventTypes.et_UDO_FORM_OPEN && Events.Antes(pVal))
                    {
                        ((SAPHelper.Form)FormObjType).OnBeforeUDOFormOpen(FormUID, ref pVal, out BubbleEvent);
                    }
                    else if (pVal.EventType == BoEventTypes.et_UDO_FORM_OPEN && Events.Depois(pVal) && pVal.ActionSuccess)
                    {
                        ((SAPHelper.Form)FormObjType).OnAfterUDOFormOpen(FormUID, ref pVal, out BubbleEvent);
                    }

                    #endregion

                    #region :: Click

                    else if (pVal.EventType == BoEventTypes.et_CLICK && Events.Antes(pVal))
                    {
                        ((SAPHelper.Form)FormObjType).OnBeforeClick(FormUID, ref pVal, out BubbleEvent);
                    }
                    else if (pVal.EventType == BoEventTypes.et_FORM_LOAD && Events.Depois(pVal) && pVal.ActionSuccess)
                    {
                        ((SAPHelper.Form)FormObjType).OnAfterClick(FormUID, ref pVal, out BubbleEvent);
                    }

                    #endregion

                    #region :: ITEM_PRESSED

                    else if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED && Events.Antes(pVal))
                    {
                        ((SAPHelper.Form)FormObjType).OnBeforeItemPressed(FormUID, ref pVal, out BubbleEvent);
                    }
                    else if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED && Events.Depois(pVal) && pVal.ActionSuccess)
                    {
                        ((SAPHelper.Form)FormObjType).OnAfterItemPressed(FormUID, ref pVal, out BubbleEvent);
                    }

                    #endregion

                }
            }
            catch (Exception e)
            {
                Dialogs.PopupError(
                    $@"Erro interno. Erro ao lidar com Evento de Item.
                    Erro: {e.Message}"
                    );
            }
        }
    }
}
