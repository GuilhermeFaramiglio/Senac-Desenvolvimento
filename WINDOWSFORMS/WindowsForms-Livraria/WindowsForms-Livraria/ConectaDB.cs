using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsForms_Livraria
{
    internal class ConectaDB
    {
        //string de conexão com o banco
        public static string Conexao
        {
            get
            {
                return @"Data Source=(LocalDB)\MSSQLLocalDB;
            AttachDbFilename=C:\Users\guilherme.bfaramigli\OneDrive - SENAC - SP\Desenvolvimento\UC12 - HUGO\WindowsForms-Livraria\BANCO\DBLivraria.mdf;
            Integrated Security=False;Connect Timeout=30";
            }
        }
    }
}
