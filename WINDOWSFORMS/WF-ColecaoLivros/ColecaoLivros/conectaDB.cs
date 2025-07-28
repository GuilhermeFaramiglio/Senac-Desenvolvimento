using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace ColecaoLivros
{
    internal class conectaDB
    {
        public static string Conexao
        {
            get
            {
                return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\guilh\OneDrive\Documentos\database\colecaolivros.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False";
            }
        }
    }
}
