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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(conectaDB.Conexao);

        private void btnAcessar_Click(object sender, EventArgs e)
        {
            if (txtLogin.Text == String.Empty || txtSenha.Text == String.Empty)
            {
                txtInfo.Text = "Campos obrigatórios!";
            }
            else
            {
                try
                {
                    con.Open();
                    string query = "SELECT usu_tipo FROM usuario " +
                        "WHERE usu_login = @usuario AND usu_senha = @senha";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@usuario", txtLogin.Text);
                    cmd.Parameters.AddWithValue("@senha", txtSenha.Text);

                    object result = cmd.ExecuteScalar();

                    // Coleta o id e nome do usuário logado
                    string infoUsuarioQuery = "SELECT usu_id, usu_nome FROM usuario WHERE usu_login = @usuario AND usu_senha = @senha";
                    SqlCommand infoCmd = new SqlCommand(infoUsuarioQuery, con);
                    infoCmd.Parameters.AddWithValue("@usuario", txtLogin.Text);
                    infoCmd.Parameters.AddWithValue("@senha", txtSenha.Text);

                    using (SqlDataReader reader = infoCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuarioLog.Id = reader.GetInt32(reader.GetOrdinal("usu_id"));
                            usuarioLog.Nome = reader.GetString(reader.GetOrdinal("usu_nome"));
                        }
                    }

                    if (result == null)
                    {
                        txtInfo.Text = "Usuário ou senha inválidos!";
                        txtLogin.Focus();
                    }
                    else
                    {
                        string tipoUsuario = result.ToString();
                        if (tipoUsuario.Equals("admin", StringComparison.OrdinalIgnoreCase))
                        {
                            Form3 admin = new Form3(usuarioLog.Nome);
                            this.Hide();
                            admin.ShowDialog();
                        }
                        else if (tipoUsuario.Equals("usuario", StringComparison.OrdinalIgnoreCase))
                        {
                            Form2 colecao = new Form2(usuarioLog.Nome);
                            this.Hide();
                            colecao.ShowDialog();
                        }
                        else
                        {
                            txtInfo.Text = "Tipo de usuário desconhecido!";
                            txtLogin.Focus();
                        }
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao tentar logar: " + ex.Message);
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
        }
    }
}
