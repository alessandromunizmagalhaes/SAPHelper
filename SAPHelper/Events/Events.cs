namespace SAPHelper
{
    public enum EventosInternos
    {
        AdicionarNovo = 1282,
        Pesquisar = 1281,
        Cancelar = 1284,
        Duplicar = 1287
    }

    public static class Events
    {
        public static bool Antes(SAPbouiCOM.BusinessObjectInfo pVal)
        {
            return pVal.BeforeAction;
        }

        public static bool Depois(SAPbouiCOM.BusinessObjectInfo pVal)
        {
            return !pVal.BeforeAction;
        }

        public static bool Antes(SAPbouiCOM.MenuEvent pVal)
        {
            return pVal.BeforeAction;
        }

        public static bool Depois(SAPbouiCOM.MenuEvent pVal)
        {
            return !pVal.BeforeAction;
        }

        public static bool Antes(SAPbouiCOM.ItemEvent pVal)
        {
            return pVal.BeforeAction;
        }

        public static bool Depois(SAPbouiCOM.ItemEvent pVal)
        {
            return !pVal.BeforeAction;
        }
    }
}
