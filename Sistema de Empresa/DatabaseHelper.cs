using System.IO;
using System.Text;

namespace SistemaDeEmpresa
{
    public static class DatabaseHelper
    {
        public const string DatabaseFile = "empresa.json";

        public static void InitializeDatabase()
        {
            // Se o arquivo JSON não existir, cria um array vazio para não dar erro na primeira leitura
            if (!File.Exists(DatabaseFile))
            {
                File.WriteAllText(DatabaseFile, "[]", Encoding.UTF8);
            }
        }
    }
}