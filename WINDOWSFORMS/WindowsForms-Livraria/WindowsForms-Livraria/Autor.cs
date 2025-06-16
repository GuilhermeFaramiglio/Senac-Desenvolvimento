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
using WindowsForms_Livraria.Classes;

namespace WindowsForms_Livraria
{
    public partial class Autor : Form
    {
        public Autor()
        {
            InitializeComponent();
            dgAutores.DataBindingComplete += dgAutores_DataBindingComplete;
        }

        private void dgAutores_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgAutores.Columns.Count >= 4)
            {
                //cabeçalho
                dgAutores.Columns[0].HeaderText = "ID";
                dgAutores.Columns[1].HeaderText = "NOME";
                dgAutores.Columns[2].HeaderText = "SOBRENOME";
                dgAutores.Columns[3].HeaderText = "DESCRIÇÃO";

                //largura colunas
                dgAutores.Columns[0].Width = 50;
                dgAutores.Columns[1].Width = 200;
                dgAutores.Columns[2].Width = 200;
                dgAutores.Columns[3].Width = 200;
            }
        }

        SqlConnection con = new SqlConnection(ConectaDB.Conexao);

        public void limparTudo()
        {
            txtNome.Text = string.Empty;
            txtSobrenome.Text = string.Empty;
            txtDescricao.Text = string.Empty;

            txtNome.Focus();
        }

        public static DataTable listarUsuarios()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConectaDB.Conexao);
                con.Open();
                string sqlListar = "select * from autor";
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
                dgAutores.AutoGenerateColumns = true;
                dgAutores.DataSource = listarUsuarios();
                dgAutores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                //configuracoes de segurança:
                dgAutores.AllowUserToAddRows = false;
                dgAutores.AllowUserToDeleteRows = false;
                dgAutores.ReadOnly = true;
            }
            catch (SqlException erro)
            {
                MessageBox.Show(erro.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e) //botão teste de conexão com o banco
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

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                //validação de campos
                if (txtNome.Text == string.Empty || txtSobrenome.Text == string.Empty)
                {
                    MessageBox.Show("Preencha os campos obrigatórios!", "Campos obrigatórios",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    con.Close();
                    con.Open();
                    string sql = "Insert into Autor (autor_nome, autor_sobrenome, autor_desc) " +
                                "values(@nome, @sobrenome, @descricao)";

                    SqlCommand cmd = new SqlCommand(sql, con);

                    //Preenchendo as variaveis utilizadas no insert com os campos do formulário que irão ser preenchidos pelo usuario
                    cmd.Parameters.Add("@nome", SqlDbType.VarChar).Value = txtNome.Text;
                    cmd.Parameters.Add("@sobrenome", SqlDbType.VarChar).Value = txtSobrenome.Text;
                    cmd.Parameters.Add("@descricao", SqlDbType.VarChar).Value = txtDescricao.Text;

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

        private void dgAutores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int linha = e.RowIndex;

            lblID.Visible = true;
            txtID.Visible = true;

            txtID.Text = dgAutores.Rows[linha].Cells[0].Value?.ToString();
            txtNome.Text = dgAutores.Rows[linha].Cells[1].Value?.ToString();
            txtSobrenome.Text = dgAutores.Rows[linha].Cells[2].Value?.ToString();
            txtDescricao.Text = dgAutores.Rows[linha].Cells[3].Value?.ToString();
        }

        private void dgAutores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            string sql = @"UPDATE Autor SET " +
                "autor_nome = @nome," +
                "autor_sobrenome = @sobrenome," +
                "autor_desc = @descricao " +
                "WHERE autor_id = @id";

            using (var con = new SqlConnection(ConectaDB.Conexao))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtID.Text));
                cmd.Parameters.AddWithValue("@nome", (txtNome.Text));
                cmd.Parameters.AddWithValue("@sobrenome", (txtSobrenome.Text));
                cmd.Parameters.AddWithValue("@descricao", (txtDescricao.Text));

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
                string sql = "DELETE FROM Autor WHERE autor_id = @id";
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

        private void btnListar_Click(object sender, EventArgs e)
        {
            atualizarGrid();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            this.Hide();
            frm.ShowDialog();
        }
    }
}
