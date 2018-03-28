using SAPbouiCOM;
using System;

namespace SAPHelper
{
    public static class SAPConnection
    {
        public static Application SBOApplication;
        public static SAPbobsCOM.Company oCompany;

        public delegate void SBOApplicationHandler(SAPbouiCOM.Application SBOApplication);
        public static SBOApplicationHandler applicationHandler;

        public delegate void CompanyHandler(SAPbobsCOM.Company Company);
        public static CompanyHandler companyHandler;

        public static void Connect()
        {
            SetApplication();

            //Dialogs.Info(":: " + addonName + " :: Iniciando ...");

            //Dialogs.Info(":: " + addonName + " :: Conectando com DI API ...");
            if (SetConnectionContext() != 0)
            {
                //Dialogs.Error(":: " + addonName + " :: Falha ao conectar com DI API ", true);
            }

            //Dialogs.Info(":: " + addonName + " :: Conectando com Banco de Dados ...");
            if (ConnectToCompany() != 0)
            {
                //Dialogs.Error(":: " + addonName + " :: Falha ao conectar com o Banco de Dados", true);
            }
        }

        private static void SetApplication()
        {
            SboGuiApi sboGuiApi;
            string connectionString = null;
            sboGuiApi = new SboGuiApi();

            try
            {
                if (Environment.GetCommandLineArgs().Length > 1)
                    connectionString = Environment.GetCommandLineArgs().GetValue(1).ToString();
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível buscar a string de conexão com o SAP.\nErro: " + e.Message);
            }

            try
            {
                sboGuiApi.Connect(connectionString);
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível estabelecer uma conexão com o SAP.\nErro: " + e.Message + "\nConnString: " + connectionString);
            }
            SBOApplication = sboGuiApi.GetApplication();
            applicationHandler(SBOApplication);
        }

        private static int SetConnectionContext()
        {
            string cookie;
            string connectionContext = "";

            try
            {

                // First initialize the Company object
                oCompany = new SAPbobsCOM.Company();

                // Acquire the connection context cookie from the DI API.
                cookie = oCompany.GetContextCookie();

                // Retrieve the connection context string from the UI API using the acquired cookie.
                connectionContext = SBOApplication.Company.GetConnectionContext(cookie);

                // before setting the SBO Login Context make sure the company is not connected
                if (oCompany.Connected)
                {
                    oCompany.Disconnect();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao tentar conectar com a DI API.\nErro: " + e.Message);
            }

            companyHandler(oCompany);

            // Set the connection context information to the DI API.
            return oCompany.SetSboLoginContext(connectionContext);
        }

        private static int ConnectToCompany()
        {
            // Establish the connection to the company database.
            return oCompany.Connect();
        }
    }
}
