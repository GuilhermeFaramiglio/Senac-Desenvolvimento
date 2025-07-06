using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColecaoLivros
{
    public partial class Form3 : Form
    {
        private string nomeUsuario;

        public Form3(string usuarioLogado)
        {
            InitializeComponent();
            this.nomeUsuario = usuarioLogado;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            lblUsuario.Text = "Usuário logado: " + nomeUsuario;
        }

        private void btnCadUsuario_Click(object sender, EventArgs e)
        {
            Form4 cadusuario = new Form4(nomeUsuario);
            this.Hide();
            cadusuario.ShowDialog();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            this.Hide();
            login.ShowDialog();
        }
    }
}
