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
    public partial class Editora : Form
    {
        public Editora()
        {
            InitializeComponent();
            dgEditoras.DataBindingComplete += dgEditoras_DataBindingComplete;
        }

        private void dgEditoras_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgEditoras.Columns.Count >= 3)
            {
                //cabeçalho
                dgEditoras.Columns[0].HeaderText = "ID";
                dgEditoras.Columns[1].HeaderText = "NOME";
                dgEditoras.Columns[2].HeaderText = "CNPJ";

                //largura colunas
                dgEditoras.Columns[0].Width = 50;
                dgEditoras.Columns[1].Width = 200;
                dgEditoras.Columns[2].Width = 200;
            }
        }

        SqlConnection con = new SqlConnection(ConectaDB.Conexao);

        public void limparTudo()
        {
            txtNome.Text = string.Empty;
            txtCNPJ.Text = string.Empty;

            txtNome.Focus();
        }

        public static DataTable listarUsuarios()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConectaDB.Conexao);
                con.Open();
                string sqlListar = "select * from editora";
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
                dgEditoras.AutoGenerateColumns = true;
                dgEditoras.DataSource = listarUsuarios();
                dgEditoras.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                //configuracoes de segurança:
                dgEditoras.AllowUserToAddRows = false;
                dgEditoras.AllowUserToDeleteRows = false;
                dgEditoras.ReadOnly = true;
            }
            catch (SqlException erro)
            {
                MessageBox.Show(erro.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e) //conexão com o banco
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
                if (txtNome.Text == string.Empty || txtCNPJ.Text == string.Empty)
                {
                    MessageBox.Show("Preencha os campos obrigatórios!", "Campos obrigatórios",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    con.Close();
                    con.Open();
                    string sql = "Insert into Editora (editora_nome, editora_cnpj) " +
                                "values(@nome, @cnpj)";

                    SqlCommand cmd = new SqlCommand(sql, con);

                    //Preenchendo as variaveis utilizadas no insert com os campos do formulário que irão ser preenchidos pelo usuario
                    cmd.Parameters.Add("@nome", SqlDbType.VarChar).Value = txtNome.Text;
                    cmd.Parameters.Add("@cnpj", SqlDbType.VarChar).Value = txtCNPJ.Text;

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

        private void btnListar_Click(object sender, EventArgs e)
        {
            atualizarGrid();
        }

        private void dgEditoras_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int linha = e.RowIndex;

            lblID.Visible = true;
            txtID.Visible = true;

            txtID.Text = dgEditoras.Rows[linha].Cells[0].Value?.ToString();
            txtNome.Text = dgEditoras.Rows[linha].Cells[1].Value?.ToString();
            txtCNPJ.Text = dgEditoras.Rows[linha].Cells[2].Value?.ToString();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            string sql = @"UPDATE Editora SET " +
                "editora_nome = @nome," +
                "editora_cnpj = @cnpj " +
                "WHERE editora_id = @id";

            using (var con = new SqlConnection(ConectaDB.Conexao))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtID.Text));
                cmd.Parameters.AddWithValue("@nome", (txtNome.Text));
                cmd.Parameters.AddWithValue("@cnpj", (txtCNPJ.Text));

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
                string sql = "DELETE FROM Editora WHERE editora_id = @id";
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

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            this.Hide();
            frm.ShowDialog();
        }
    }
}
