using SAPbouiCOM;
using SAPHelper;
using System;
using System.Collections.Generic;

namespace CotaFacil
{
    public static class FormEvents
    {
        private static readonly Dictionary<string, Type> mappedEventsToObjects = new Dictionary<string, Type>() { };

        public static void DeclararEventos(BoEventTypes eventType, Dictionary<string, SAPHelper.Form> mappedEvents)
        {
            var eventFilters = new SAPbouiCOM.EventFilters();
            var eventFilter = eventFilters.Add(eventType);
            foreach (var mappedEvent in mappedEvents)
            {
                AddmappedEventsToObjects(eventFilter, eventType, mappedEvent.Key, mappedEvent.Value);
            }
        }

        private static void AddmappedEventsToObjects(EventFilter eventFilter, BoEventTypes eventType, string FormType, SAPHelper.Form tipoClasse)
        {
            eventFilter.AddEx(FormType);

            if (!mappedEventsToObjects.ContainsKey(FormType))
            {
                mappedEventsToObjects.Add(FormType, tipoClasse.GetType());
            }
            else
            {
                throw new Exception($"evento {eventType} está sendo declarado mais de uma vez.");
            }
        }

        public static void ItemEvent(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (pVal.EventType == BoEventTypes.et_FORM_LOAD && Events.Antes(pVal))
            {
                if (mappedEventsToObjects.ContainsKey(pVal.FormTypeEx))
                {
                    var FormObjType = Activator.CreateInstance(mappedEventsToObjects[pVal.FormTypeEx]);
                    ((SAPHelper.Form)FormObjType).OnBeforeFormOpen(FormUID, ref pVal, out BubbleEvent);
                }
            }
        }
    }
}
