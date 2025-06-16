using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsForms_Livraria.Classes
{
    internal class EDIT_CLASS
    {
		private int editora_id;

		public int Editora_id
		{
			get { return editora_id; }
			set { editora_id = value; }
		}

		private string editora_nome;

		public string Editora_nome
		{
			get { return editora_nome; }
			set { editora_nome = value; }
		}

		private string editora_cnpj;

		public string Editora_cnpj
		{
			get { return editora_cnpj; }
			set { editora_cnpj = value; }
		}
	}
}
