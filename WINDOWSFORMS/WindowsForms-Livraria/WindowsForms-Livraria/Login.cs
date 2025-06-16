using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForms_Livraria
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(ConectaDB.Conexao);

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            if (txtUsuario.Text == String.Empty || txtSenha.Text == String.Empty)
            {
                lblInformacao.Text = "Campos obrigatórios!";
            }
            else
            {
                try
                {
                    con.Open();
                    string query = "SELECT log_ativo FROM Login " +
                        "WHERE log_usuario = @usuario AND log_senha = @senha";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@usuario", txtUsuario.Text);
                    cmd.Parameters.AddWithValue("@senha", txtSenha.Text);

                    object result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        lblInformacao.Text = "Usuário ou senha inválidos!";
                        txtUsuario.Focus();
                        con.Close();
                    }
                    else if (Convert.ToBoolean(result) == false)
                    {
                        lblInformacao.Text = "Usuário inativo!";
                        txtUsuario.Focus();
                        con.Close();
                    }
                    else
                    {
                        Form1 principal = new Form1();
                        this.Hide();
                        principal.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao tentar logar: " + ex.Message);
                }
            }
        }
    }
}
