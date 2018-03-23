using SAPbouiCOM;
using System.Collections.Generic;

namespace SAPHelper
{
    public class MapEventsToForms
    {
        public BoEventTypes EventType { get; set; }
        public List<Form> Forms { get; set; }

        public MapEventsToForms(BoEventTypes eventType, List<Form> Forms)
        {
            this.Forms = Forms;
            EventType = eventType;
        }

        public MapEventsToForms(BoEventTypes eventType, Form Form) : this(eventType, new List<Form>() { Form })
        {

        }
    }
}
