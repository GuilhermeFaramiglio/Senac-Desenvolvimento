using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsForms_Livraria.Classes
{
    internal class USER_CLASS
    {
		private int user_id;

		public int User_id
		{
			get { return user_id; }
			set { user_id = value; }
		}

		private string user_nome;

		public string User_nome
		{
			get { return user_nome; }
			set { user_nome = value; }
		}

		private string user_cpf;

		public string User_cpf
		{
			get { return user_cpf; }
			set { user_cpf = value; }
		}

		private string user_endereco;

		public string User_endereco
		{
			get { return user_endereco; }
			set { user_endereco = value; }
		}

		private int user_endereco_num;

		public int User_endereco_num
		{
			get { return user_endereco_num; }
			set { user_endereco_num = value; }
		}

		private string user_cidade;

		public string User_cidade
		{
			get { return user_cidade; }
			set { user_cidade = value; }
		}

		private string user_estado_uf;

		public string User_estado_uf
		{
			get { return user_estado_uf; }
			set { user_estado_uf = value; }
		}

		private DateTime user_datanascimento;

		public DateTime User_datanascimento
		{
			get { return user_datanascimento; }
			set { user_datanascimento = value; }
		}

		private string user_sexo;

		public string User_sexo
		{
			get { return user_sexo; }
			set { user_sexo = value; }
		}

        private string user_telefone;

		public string User_telefone
		{
			get { return user_telefone; }
			set { user_telefone = value; }
		}
	}
}
