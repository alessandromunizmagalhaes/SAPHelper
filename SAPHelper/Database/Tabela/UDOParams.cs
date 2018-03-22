using SAPbobsCOM;

namespace SAPHelper
{
    public class UDOParams
    {
        public BoYesNoEnum CanCancel { get; set; }
        public BoYesNoEnum CanClose { get; set; }
        public BoYesNoEnum CanCreateDefaultForm { get; set; }
        public BoYesNoEnum CanDelete { get; set; }
        public BoYesNoEnum CanFind { get; set; }
        public BoYesNoEnum CanLog { get; set; }
        public BoYesNoEnum CanYearTransfer { get; set; }
        public BoYesNoEnum ManageSeries { get; set; }

        public UDOParams(
                BoYesNoEnum canCancel = BoYesNoEnum.tYES
                , BoYesNoEnum canClose = BoYesNoEnum.tYES
                , BoYesNoEnum canCreateDefaultForm = BoYesNoEnum.tYES
                , BoYesNoEnum canDelete = BoYesNoEnum.tYES
                , BoYesNoEnum canFind = BoYesNoEnum.tYES
                , BoYesNoEnum canLog = BoYesNoEnum.tYES
                , BoYesNoEnum canYearTransfer = BoYesNoEnum.tYES
                , BoYesNoEnum manageSeries = BoYesNoEnum.tYES)
        {
            CanCancel = canCancel;
            CanClose = canClose;
            CanCreateDefaultForm = canCreateDefaultForm;
            CanDelete = canDelete;
            CanFind = canFind;
            CanLog = canLog;
            CanYearTransfer = canYearTransfer;
            ManageSeries = manageSeries;
        }
    }
}
