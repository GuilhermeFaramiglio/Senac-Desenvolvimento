using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsForms_Livraria.Classes
{
    internal class LIVRO_CLASS
    {
		private int livro_id;

		public int Livro_id
		{
			get { return livro_id; }
			set { livro_id = value; }
		}

		private string livro_nome;

		public string Livro_nome
		{
			get { return livro_nome; }
			set { livro_nome = value; }
		}

		private string livro_descricao;

		public string Livro_descricao
		{
			get { return livro_descricao; }
			set { livro_descricao = value; }
		}

		private string livro_publicacao;

		public string Livro_publicacao
		{
			get { return livro_publicacao; }
			set { livro_publicacao = value; }
		}

		private int livro_npaginas;

		public int Livro_npaginas
		{
			get { return livro_npaginas; }
			set { livro_npaginas = value; }
		}

		private decimal livro_valor;

		public decimal Livro_valor
		{
			get { return livro_valor; }
			set { livro_valor = value; }
		}

		private int livro_autor_fk;

		public int Livro_autor_fk
		{
			get { return livro_autor_fk; }
			set { livro_autor_fk = value; }
		}

		private int livro_editora_fk;

		public int Livro_editora_fk
		{
			get { return livro_editora_fk; }
			set { livro_editora_fk = value; }
		}
	}
}
