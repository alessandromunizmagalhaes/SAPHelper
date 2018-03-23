using SAPbouiCOM;
using SAPHelper;
using System;
using System.Collections.Generic;

namespace CotaFacil
{
    public static class FormEvents
    {
        private static readonly Dictionary<string, Type> _mappedEventToForms = new Dictionary<string, Type>() { };

        public static void DeclararEventos(List<MapEventsToForms> mappedEventsToForms)
        {
            var eventFilters = new EventFilters();
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

            if (_mappedEventToForms.ContainsKey(pVal.FormTypeEx))
            {
                var FormObjType = Activator.CreateInstance(_mappedEventToForms[pVal.FormTypeEx]);

                if (pVal.EventType == BoEventTypes.et_FORM_LOAD && Events.Antes(pVal))
                {
                    ((SAPHelper.Form)FormObjType).OnBeforeFormLoad(FormUID, ref pVal, out BubbleEvent);
                }
                else if (pVal.EventType == BoEventTypes.et_FORM_LOAD && Events.Depois(pVal))
                {
                    ((SAPHelper.Form)FormObjType).OnAfterFormLoad(FormUID, ref pVal, out BubbleEvent);
                }

                else if (pVal.EventType == BoEventTypes.et_CLICK && Events.Antes(pVal))
                {
                    ((SAPHelper.Form)FormObjType).OnBeforeClick(FormUID, ref pVal, out BubbleEvent);
                }
                else if (pVal.EventType == BoEventTypes.et_FORM_LOAD && Events.Depois(pVal))
                {
                    ((SAPHelper.Form)FormObjType).OnAfterClick(FormUID, ref pVal, out BubbleEvent);
                }

                else if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED && Events.Antes(pVal))
                {
                    ((SAPHelper.Form)FormObjType).OnBeforeItemPressed(FormUID, ref pVal, out BubbleEvent);
                }
                else if (pVal.EventType == BoEventTypes.et_ITEM_PRESSED && Events.Depois(pVal))
                {
                    ((SAPHelper.Form)FormObjType).OnAfterItemPressed(FormUID, ref pVal, out BubbleEvent);
                }
            }
        }
    }
}
