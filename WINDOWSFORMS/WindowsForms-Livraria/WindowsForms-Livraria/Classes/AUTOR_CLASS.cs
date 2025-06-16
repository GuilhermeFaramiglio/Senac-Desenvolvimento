using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsForms_Livraria.Classes
{
    internal class AUTOR_CLASS
    {
		private int autor_id;

		public int Autor_id
		{
			get { return autor_id; }
			set { autor_id = value; }
		}

		private string autor_nome;

		public string Autor_nome
		{
			get { return autor_nome; }
			set { autor_nome = value; }
		}

		private string autor_sobrenome;

		public string Autor_sobrenome
		{
			get { return autor_sobrenome; }
			set { autor_sobrenome = value; }
		}

		private string autor_desc;

		public string Autor_desc
		{
			get { return autor_desc; }
			set { autor_desc = value; }
		}
	}
}
