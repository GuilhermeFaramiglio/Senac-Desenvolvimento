using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using WindowsForms_Livraria.Classes;

namespace WindowsForms_Livraria
{
    public partial class Usuario : Form
    {
        public Usuario()
        {
            InitializeComponent();
            dgUsuarios.DataBindingComplete += dgUsuarios_DataBindingComplete;
        }

        //conexão com o banco - sql connection
        SqlConnection con = new SqlConnection(ConectaDB.Conexao);

        public void limparTudo()
        {
            txtNome.Text = string.Empty;
            txtCPF.Text = string.Empty;
            txtTelefone.Text = string.Empty;
            txtDatanascimento.Text = DateTime.Today.ToShortDateString();
            cbSexo.SelectedIndex = 0;
            txtEndereco.Text = string.Empty;
            txtNumero.Text = string.Empty;
            txtCidade.Text = string.Empty;
            cbEstado.SelectedIndex = 0;

            txtNome.Focus();
        }

        private void dgUsuarios_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgUsuarios.Columns.Count >= 10)
            {
                //cabeçalho
                dgUsuarios.Columns[0].HeaderText = "ID";
                dgUsuarios.Columns[1].HeaderText = "NOME";
                dgUsuarios.Columns[2].HeaderText = "CPF";
                dgUsuarios.Columns[3].HeaderText = "ENDEREÇO";
                dgUsuarios.Columns[4].HeaderText = "Nº";
                dgUsuarios.Columns[5].HeaderText = "CIDADE";
                dgUsuarios.Columns[6].HeaderText = "ESTADO";
                dgUsuarios.Columns[7].HeaderText = "DATA NASCIMENTO";
                dgUsuarios.Columns[8].HeaderText = "SEXO";
                dgUsuarios.Columns[9].HeaderText = "TELEFONE";

                //largura colunas
                dgUsuarios.Columns[0].Width = 50;
                dgUsuarios.Columns[1].Width = 200;
                dgUsuarios.Columns[2].Width = 90;
                dgUsuarios.Columns[3].Width = 200;
                dgUsuarios.Columns[4].Width = 50;
                dgUsuarios.Columns[5].Width = 80;
                dgUsuarios.Columns[6].Width = 100;
                dgUsuarios.Columns[7].Width = 150;
                dgUsuarios.Columns[8].Width = 150;
                dgUsuarios.Columns[9].Width = 90;
            }
        }

        //listar usuarios
        public static DataTable listarUsuarios()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConectaDB.Conexao);
                con.Open();
                string sqlListar = "select * from usuario";
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

        //Atualizar grid
        public void atualizarGrid()
        {
            try
            {
                dgUsuarios.AutoGenerateColumns = true;
                dgUsuarios.DataSource = listarUsuarios();
                dgUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                //configuracoes de segurança:
                dgUsuarios.AllowUserToAddRows = false;
                dgUsuarios.AllowUserToDeleteRows = false;
                dgUsuarios.ReadOnly = true;
            }
            catch (SqlException erro)
            {
                MessageBox.Show(erro.Message);
            }
        }

        private void cbSexo_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e) //BOTÃO DE TESTE DE CONEXÃO
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
                if (txtNome.Text == string.Empty || txtCPF.Text == string.Empty ||
                    txtEndereco.Text == string.Empty || txtTelefone.Text == string.Empty)
                {
                    MessageBox.Show("Preencha os campos obrigatórios!", "Campos obrigatórios",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                } else
                {
                    con.Close();
                    con.Open();
                    string sql = "Insert into " +
                                "Usuario(user_nome, user_cpf, user_endereco, user_endereco_num, user_cidade, user_estado_uf, user_datanascimento, user_sexo, user_telefone) " +
                                "values (@nome, @cpf, @endereco, @endereco_num, @cidade, @estado_uf, @datanascimento, @sexo, @telefone)";

                    SqlCommand cmd = new SqlCommand(sql, con);

                    //Preenchendo as variaveis utilizadas no isert com os campos do formulário que irão ser preenchidos pelo usuario
                    cmd.Parameters.Add("@nome", SqlDbType.VarChar).Value = txtNome.Text;
                    cmd.Parameters.Add("@cpf", SqlDbType.VarChar).Value = txtCPF.Text;
                    cmd.Parameters.Add("@endereco", SqlDbType.VarChar).Value = txtEndereco.Text;
                    cmd.Parameters.Add("@endereco_num", SqlDbType.VarChar).Value = Convert.ToInt32(txtNumero.Text);
                    cmd.Parameters.Add("@cidade", SqlDbType.VarChar).Value = txtCidade.Text;
                    cmd.Parameters.Add("@estado_uf", SqlDbType.VarChar).Value = cbEstado.Text;
                    cmd.Parameters.Add("@datanascimento", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDatanascimento.Text);
                    cmd.Parameters.Add("@sexo", SqlDbType.VarChar).Value = cbSexo.Text;
                    cmd.Parameters.Add("@telefone", SqlDbType.VarChar).Value = txtTelefone.Text;

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

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            var resp = MessageBox.Show("Deseja excluir?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resp == DialogResult.Yes)
            {
                string sql = "DELETE FROM Usuario WHERE user_id = @id";
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

        private void dgUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int linha = e.RowIndex;

            lblID.Visible = true;
            txtID.Visible = true;

            txtID.Text = dgUsuarios.Rows[linha].Cells[0].Value?.ToString();
            txtNome.Text = dgUsuarios.Rows[linha].Cells[1].Value?.ToString();
            txtCPF.Text = dgUsuarios.Rows[linha].Cells[2].Value?.ToString();
            txtEndereco.Text = dgUsuarios.Rows[linha].Cells[3].Value?.ToString();
            txtNumero.Text = dgUsuarios.Rows[linha].Cells[4].Value?.ToString();
            txtCidade.Text = dgUsuarios.Rows[linha].Cells[5].Value?.ToString();
            cbEstado.Text = dgUsuarios.Rows[linha].Cells[6].Value?.ToString();
            //txtDatanascimento.Text = dgUsuarios.Rows[linha].Cells[4].Value?.ToString();
            var valorData = dgUsuarios.Rows[linha].Cells[7].Value;
            if (valorData != null && DateTime.TryParse(valorData.ToString(), out DateTime dataNasc))
            {
                txtDatanascimento.Text = dataNasc.ToString("dd/MM/yyyy");
            }
            else
            {
                txtDatanascimento.Text = string.Empty;
            }
            cbSexo.Text = dgUsuarios.Rows[linha].Cells[8].Value?.ToString();
            txtTelefone.Text = dgUsuarios.Rows[linha].Cells[9].Value?.ToString();

        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            string sql = @"UPDATE Usuario SET " +
                "user_nome = @nome," +
                "user_cpf = @cpf," +
                "user_endereco = @endereco," +
                "user_endereco_num = @endereco_num," +
                "user_cidade = @cidade," +
                "user_estado_uf = @estado_uf," +
                "user_datanascimento = @datanascimneto," +
                "user_sexo = @sexo, " +
                "user_telefone = @telefone" +
                "WHERE user_id = @id";

            using (var con = new SqlConnection(ConectaDB.Conexao))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtID.Text));
                cmd.Parameters.AddWithValue("@nome", (txtNome.Text));
                cmd.Parameters.AddWithValue("@cpf", (txtCPF.Text));
                cmd.Parameters.AddWithValue("@endereco", (txtEndereco.Text));
                cmd.Parameters.AddWithValue("@endereco_num", Convert.ToInt32(txtNumero.Text));
                cmd.Parameters.AddWithValue("@cidade", (txtCidade.Text));
                cmd.Parameters.AddWithValue("@estado_uf", (cbEstado.Text));
                cmd.Parameters.AddWithValue("@datanascimento", Convert.ToDateTime(txtDatanascimento.Text));
                cmd.Parameters.AddWithValue("@sexo", (cbSexo.Text));
                cmd.Parameters.AddWithValue("@telefone", (txtTelefone.Text));

                con.Open();
                cmd.ExecuteNonQuery();
            }
            atualizarGrid();
            limparTudo();
        }

        private void btnlista_Click(object sender, EventArgs e)
        {
            atualizarGrid();
        }

        //função de pesquisa
        public static DataTable pesquisarNome(string termo)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConectaDB.Conexao))
                {
                    string sql = "SELECT * FROM Usuario WHERE user_nome like @termo";
                    SqlDataAdapter da = new SqlDataAdapter(sql, con);
                    da.SelectCommand.Parameters.AddWithValue("@termo", "%" + termo + "%");
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            dgUsuarios.DataSource = pesquisarNome(txtPesquisa.Text);

            dgUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgUsuarios.AllowUserToAddRows = false;
            dgUsuarios.AllowUserToDeleteRows = false;
            dgUsuarios.ReadOnly = true;
        }

        private void Usuario_Load(object sender, EventArgs e)
        {

        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            this.Hide();
            frm.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

