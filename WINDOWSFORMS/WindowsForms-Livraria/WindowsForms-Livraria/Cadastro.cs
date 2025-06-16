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

namespace WindowsForms_Livraria
{
    public partial class Cadastro : Form
    {
        public Cadastro()
        {
            InitializeComponent();
            dgUserSistema.DataBindingComplete += dgUserSistema_DataBindingComplete;
        }

        SqlConnection con = new SqlConnection(ConectaDB.Conexao);

        private void dgUserSistema_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgUserSistema.Columns.Count >= 3)
            {
                //cabeçalho
                dgUserSistema.Columns[0].HeaderText = "ID";
                dgUserSistema.Columns[1].HeaderText = "NOME";
                dgUserSistema.Columns[2].HeaderText = "SENHA";
                dgUserSistema.Columns[3].HeaderText = "ATIVO";

                //largura colunas
                dgUserSistema.Columns[0].Width = 50;
                dgUserSistema.Columns[1].Width = 150;
                dgUserSistema.Columns[2].Width = 50;
                dgUserSistema.Columns[3].Width = 20;
            }
        }

        public static DataTable listarUsuarios()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConectaDB.Conexao);
                con.Open();
                string sqlListar = "select * from Login";
                SqlDataAdapter da = new SqlDataAdapter(sqlListar, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
                con.Close();
            }
            catch (SqlException erro)
            {
                return null;
            }
        }

        public void atualizarGrid()
        {
            try
            {
                dgUserSistema.AutoGenerateColumns = true;
                dgUserSistema.DataSource = listarUsuarios();
                dgUserSistema.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                //configuracoes de segurança:
                dgUserSistema.AllowUserToAddRows = false;
                dgUserSistema.AllowUserToDeleteRows = false;
                dgUserSistema.ReadOnly = true;
            }
            catch (SqlException erro)
            {
                MessageBox.Show(erro.Message);
            }
        }

        private void btnTesteDB_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                MessageBox.Show("Conexão com o banco bem sucedida!", "Conexão",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();
            }
            catch (SqlException erro)
            {
                MessageBox.Show(erro.Message, "Conexão",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { con.Close(); }
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            atualizarGrid();
        }

        public void limparTudo()
        {
            txtNome.Text = string.Empty;
            txtSenha.Text = string.Empty;

            txtNome.Focus();
        }

        private void dgUserSistema_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int linha = e.RowIndex;

            lblID.Visible = true;
            txtID.Visible = true;

            txtID.Text = dgUserSistema.Rows[linha].Cells[0].Value?.ToString();
            txtNome.Text = dgUserSistema.Rows[linha].Cells[1].Value?.ToString();
            txtSenha.Text = dgUserSistema.Rows[linha].Cells[2].Value?.ToString();
            bool.TryParse(dgUserSistema.Rows[linha].Cells[3].Value?.ToString(), out bool ativo);
            cbAtivo.Checked = ativo;
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                //validação de campos
                if (txtNome.Text == string.Empty || txtSenha.Text == string.Empty)
                {
                    MessageBox.Show("Preencha os campos obrigatórios!", "Campos obrigatórios",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    con.Close();
                    con.Open();
                    string sql = "Insert into Login (log_usuario, log_senha) " +
                                "values(@usuario, @senha)";

                    SqlCommand cmd = new SqlCommand(sql, con);

                    //Preenchendo as variaveis utilizadas no insert com os campos do formulário que irão ser preenchidos pelo usuario
                    cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = txtNome.Text;
                    cmd.Parameters.Add("@senha", SqlDbType.VarChar).Value = txtSenha.Text;

                    cmd.ExecuteNonQuery();
                    con.Close();

                    limparTudo();
                    atualizarGrid();

                    MessageBox.Show("Cadastro realizado com sucesso!", "Cadastro usuário",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                throw;
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            this.Hide();
            frm.ShowDialog();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            string sql = @"UPDATE Login SET " +
                "log_usuario = @usuario," +
                "log_senha = @senha," +
                "log_ativo = @ativo " +
                "WHERE log_id = @id";

            using (var con = new SqlConnection(ConectaDB.Conexao))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtID.Text));
                cmd.Parameters.AddWithValue("@usuario", (txtNome.Text));
                cmd.Parameters.AddWithValue("@senha", (txtSenha.Text));
                cmd.Parameters.AddWithValue("@ativo", cbAtivo.Checked);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            atualizarGrid();
            limparTudo();
        }

        private void btnDeletar_Click(object sender, EventArgs e)
        {
            var resp = MessageBox.Show("Deseja excluir?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resp == DialogResult.Yes)
            {
                string sql = "DELETE FROM Login WHERE log_id = @id";
                using (var con = new SqlConnection(ConectaDB.Conexao))
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtID.Text));
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                atualizarGrid();
                limparTudo();
            }
        }
    }
}
