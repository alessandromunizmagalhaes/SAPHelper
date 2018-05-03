namespace SAPHelper
{
    public abstract class Versionamento
    {
        public abstract double Versao { get; }

        public abstract void Aplicar(Database db);
    }
}
