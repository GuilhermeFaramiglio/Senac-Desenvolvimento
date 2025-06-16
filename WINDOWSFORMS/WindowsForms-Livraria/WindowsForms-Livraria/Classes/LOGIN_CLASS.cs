using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsForms_Livraria.Classes
{
    internal class LOGIN_CLASS
    {
		private int log_id;

		public int Log_id
		{
			get { return log_id; }
			set { log_id = value; }
		}

		private string log_usuario;

		public string Log_usuario
		{
			get { return log_usuario; }
			set { log_usuario = value; }
		}

		private string log_senha;

		public string Log_senha
		{
			get { return log_senha; }
			set { log_senha = value; }
		}

		private int log_ativo;

		public int Log_ativo
		{
			get { return log_ativo; }
			set { log_ativo = value; }
		}
	}
}
