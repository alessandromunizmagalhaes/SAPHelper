using SAPbouiCOM;
using System;
using System.Collections.Generic;

namespace SAPHelper
{
    public static class FormEvents
    {
        private static readonly Dictionary<string, Type> _mappedEventToForms = new Dictionary<string, Type>() { };
        internal static readonly Dictionary<EventosInternos, Dictionary<string, Type>> _mappedInternalEventsToFormTypes = new Dictionary<EventosInternos, Dictionary<string, Type>>() { };

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

                    if (pVal.EventType == BoEventTypes.et_FORM_LOAD)
                    {
                        if (Events.Antes(pVal))
                        {
                            ((Form)FormObjType).OnBeforeFormLoad(FormUID, ref pVal, out BubbleEvent);
                        }
                        else if (Events.Depois(pVal) && pVal.ActionSuccess)
                        {
                            ((Form)FormObjType).OnAfterFormLoad(FormUID, ref pVal, out BubbleEvent);
                        }
                    }

                    #endregion

                    #region :: Form_Draw

                    else if (pVal.EventType == BoEventTypes.et_FORM_DRAW)
                    {
                        if (Events.Antes(pVal))
                        {
                            ((Form)FormObjType).OnBeforeFormDraw(FormUID, ref pVal, out BubbleEvent);
                        }
                        else if (Events.Depois(pVal) && pVal.ActionSuccess)
                        {
                            ((Form)FormObjType).OnAfterFormDraw(FormUID, ref pVal, out BubbleEvent);
                        }
                    }

                    #endregion

                    #region :: Form_Close

                    else if (pVal.EventType == BoEventTypes.et_FORM_CLOSE)
                    {
                        if (Events.Antes(pVal))
                        {
                            ((Form)FormObjType).OnBeforeFormClose(FormUID, ref pVal, out BubbleEvent);
                        }
                        else if (Events.Depois(pVal) && pVal.ActionSuccess)
                        {
                            ((Form)FormObjType).OnAfterFormClose(FormUID, ref pVal, out BubbleEvent);
                        }
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
                        var form = Global.SBOApplication.Forms.Item(pVal.FormUID);

                        if (form.Items.Count == 0)
                        {
                            ((Form)FormObjType).OnBeforeFormVisible(FormUID, ref pVal, out BubbleEvent);
                        }
                        else
                        {
                            ((Form)FormObjType).OnAfterFormVisible(FormUID, ref pVal, out BubbleEvent);
                        }
                    }

                    #endregion

                    #region :: UDO_FORM_OPEN

                    else if (pVal.EventType == BoEventTypes.et_UDO_FORM_OPEN)
                    {
                        if (Events.Antes(pVal))
                        {
                            ((Form)FormObjType).OnBeforeUDOFormOpen(FormUID, ref pVal, out BubbleEvent);
                        }
                        else if (Events.Depois(pVal) && pVal.ActionSuccess)
                        {
                            ((Form)FormObjType).OnAfterUDOFormOpen(FormUID, ref pVal, out BubbleEvent);
                        }
                    }

                    #endregion

                    #region :: CHOOSE_FROM_LIST

                    else if (pVal.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
                    {
                        // o sap tem esse jeito doido de pegar o evento do choose
                        var form = Global.SBOApplication.Forms.Item(pVal.FormUID);
                        var chooseEvent = ((ChooseFromListEvent)pVal);
                        var choose = form.ChooseFromLists.Item(chooseEvent.ChooseFromListUID);

                        if (Events.Antes(pVal))
                        {
                            ((Form)FormObjType).OnBeforeChooseFromList(form, chooseEvent, choose, ref pVal, out BubbleEvent);
                        }
                        else if (Events.Depois(pVal) && pVal.ActionSuccess)
                        {
                            // só dispara o evento depois, se houver itens selecionados
                            // se o cara selecionar o choose, e cancelar a seleção porque quer fazer outra coisa,
                            // este evento after é disparado
                            // então essa verificação de se tem itens em selectedobjects é meio padrão, sempre só vou querer o evento 
                            // se tiver valores lá dentro
                            // então estou sempre já fazendo essa verificação pro usuário final.
                            DataTable dataTable = chooseEvent.SelectedObjects;
                            if (dataTable != null)
                            {
                                ((Form)FormObjType).OnAfterChooseFromList(form, chooseEvent, choose, ref pVal, out BubbleEvent);
                            }
                        }
                    }

                    #endregion

                    #region :: Click

                    else if (pVal.EventType == BoEventTypes.et_CLICK)
                    {
                        if (Events.Antes(pVal))
                        {
                            ((Form)FormObjType).OnBeforeClick(FormUID, ref pVal, out BubbleEvent);
                        }
                        else if (Events.Depois(pVal) && pVal.ActionSuccess)
                        {
                            ((Form)FormObjType).OnAfterClick(FormUID, ref pVal, out BubbleEvent);
                        }
                    }

                    #endregion

                    #region :: Double Click

                    else if (pVal.EventType == BoEventTypes.et_DOUBLE_CLICK)
                    {
                        if (Events.Antes(pVal))
                        {
                            ((Form)FormObjType).OnBeforeDoubleClick(FormUID, ref pVal, out BubbleEvent);
                        }
                        else if (Events.Depois(pVal) && pVal.ActionSuccess)
                        {
                            ((Form)FormObjType).OnAfterDoubleClick(FormUID, ref pVal, out BubbleEvent);
                        }
                    }

                    #endregion

                    #region :: Validate

                    else if (pVal.EventType == BoEventTypes.et_VALIDATE)
                    {
                        if (Events.Antes(pVal))
                        {
                            ((Form)FormObjType).OnBeforeValidate(FormUID, ref pVal, out BubbleEvent);
                        }
                        else if (Events.Depois(pVal) && pVal.ActionSuccess)
                        {
                            ((Form)FormObjType).OnAfterValidate(FormUID, ref pVal, out BubbleEvent);
                        }
                    }

                    #endregion

                    #region :: COMBO_SELECT

                    else if (pVal.EventType == BoEventTypes.et_COMBO_SELECT)
                    {
                        if (Events.Antes(pVal))
                        {
                            ((Form)FormObjType).OnBeforeComboSelect(FormUID, ref pVal, out BubbleEvent);
                        }
                        else if (Events.Depois(pVal) && pVal.ActionSuccess)
                        {
                            ((Form)FormObjType).OnAfterComboSelect(FormUID, ref pVal, out BubbleEvent);
                        }
                    }

                    #endregion

                    #region :: ITEM_PRESSED

                    else if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED)
                    {
                        if (Events.Antes(pVal))
                        {
                            ((Form)FormObjType).OnBeforeItemPressed(FormUID, ref pVal, out BubbleEvent);
                        }
                        else if (Events.Depois(pVal) && pVal.ActionSuccess)
                        {
                            ((Form)FormObjType).OnAfterItemPressed(FormUID, ref pVal, out BubbleEvent);
                        }
                    }

                    #endregion

                    #region :: MATRIX_LINK_PRESSED

                    else if (pVal.EventType == BoEventTypes.et_MATRIX_LINK_PRESSED)
                    {
                        if (Events.Antes(pVal))
                        {
                            ((Form)FormObjType).OnBeforeMatrixLinkPressed(FormUID, ref pVal, out BubbleEvent);
                        }
                        else if (Events.Depois(pVal) && pVal.ActionSuccess)
                        {
                            ((Form)FormObjType).OnAfterMatrixLinkPressed(FormUID, ref pVal, out BubbleEvent);
                        }
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

        public static void DeclararEventosInternos(EventosInternos eventoInterno, Form form)
        {
            DeclararEventosInternos(eventoInterno, new List<Form>() { form });
        }

        public static void DeclararEventosInternos(EventosInternos eventoInterno, List<Form> forms)
        {
            if (!_mappedInternalEventsToFormTypes.ContainsKey(eventoInterno))
            {
                _mappedInternalEventsToFormTypes.Add(eventoInterno, new Dictionary<string, Type>() { });
            }

            foreach (var form in forms)
            {
                _mappedInternalEventsToFormTypes[eventoInterno].Add(form.FormType, form.GetType());
            }
        }

        public static void FormDataEvent(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (_mappedEventToForms.ContainsKey(BusinessObjectInfo.FormTypeEx))
                {
                    var FormObjType = Activator.CreateInstance(_mappedEventToForms[BusinessObjectInfo.FormTypeEx]);

                    #region :: FORM_DATA_ADD

                    if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                    {
                        if (Events.Antes(BusinessObjectInfo))
                        {
                            ((Form)FormObjType).OnBeforeFormDataAdd(ref BusinessObjectInfo, out BubbleEvent);
                        }
                        else if (Events.Depois(BusinessObjectInfo) && BusinessObjectInfo.ActionSuccess)
                        {
                            ((Form)FormObjType).OnAfterFormDataAdd(ref BusinessObjectInfo, out BubbleEvent);
                        }
                    }

                    #endregion

                    #region :: FORM_DATA_UPDATE

                    else if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_UPDATE)
                    {
                        if (Events.Antes(BusinessObjectInfo))
                        {
                            ((Form)FormObjType).OnBeforeFormDataUpdate(ref BusinessObjectInfo, out BubbleEvent);
                        }
                        else if (Events.Depois(BusinessObjectInfo) && BusinessObjectInfo.ActionSuccess)
                        {
                            ((Form)FormObjType).OnAfterFormDataUpdate(ref BusinessObjectInfo, out BubbleEvent);
                        }
                    }

                    #endregion

                    #region :: FORM_DATA_DELETE

                    else if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_DELETE)
                    {
                        if (Events.Antes(BusinessObjectInfo))
                        {
                            ((Form)FormObjType).OnBeforeFormDataDelete(ref BusinessObjectInfo, out BubbleEvent);
                        }
                        else if (Events.Depois(BusinessObjectInfo) && BusinessObjectInfo.ActionSuccess)
                        {
                            ((Form)FormObjType).OnAfterFormDataDelete(ref BusinessObjectInfo, out BubbleEvent);
                        }
                    }

                    #endregion

                    #region :: FORM_DATA_LOAD

                    else if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
                    {
                        if (Events.Antes(BusinessObjectInfo))
                        {
                            ((Form)FormObjType).OnBeforeFormDataLoad(ref BusinessObjectInfo, out BubbleEvent);
                        }
                        else if (Events.Depois(BusinessObjectInfo) && BusinessObjectInfo.ActionSuccess)
                        {
                            ((Form)FormObjType).OnAfterFormDataLoad(ref BusinessObjectInfo, out BubbleEvent);
                        }
                    }

                    #endregion

                }
            }
            catch (Exception e)
            {
                Dialogs.PopupError(
                    $@"Erro interno. Erro ao lidar com Evento de Form Data.
                    Erro: {e.Message}"
                    );
            }
        }
    }
}
