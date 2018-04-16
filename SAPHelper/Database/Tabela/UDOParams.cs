using SAPbobsCOM;

namespace SAPHelper
{
    public class UDOParams
    {
        public BoYesNoEnum CanCancel { get; set; } = BoYesNoEnum.tYES;
        public BoYesNoEnum CanClose { get; set; } = BoYesNoEnum.tYES;
        public BoYesNoEnum CanCreateDefaultForm { get; set; } = BoYesNoEnum.tYES;
        public BoYesNoEnum CanDelete { get; set; } = BoYesNoEnum.tYES;
        public BoYesNoEnum CanFind { get; set; } = BoYesNoEnum.tYES;
        public BoYesNoEnum CanLog { get; set; } = BoYesNoEnum.tYES;
        public BoYesNoEnum CanYearTransfer { get; set; } = BoYesNoEnum.tYES;
        public BoYesNoEnum ManageSeries { get; set; } = BoYesNoEnum.tYES;
        public BoYesNoEnum EnableEnhancedForm { get; set; } = BoYesNoEnum.tYES;
    }
}
