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
    public partial class Form4 : Form
    {
        private string nomeUsuario;
        public Form4(string usuarioLogado)
        {
            InitializeComponent();
            this.nomeUsuario = usuarioLogado;
            dgUsuarios.DataBindingComplete += dgUsuarios_DataBindingComplete;
        }

        private void dgUsuarios_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgUsuarios.Columns.Count >= 3)
            {
                //cabeçalho
                dgUsuarios.Columns[0].HeaderText = "ID";
                dgUsuarios.Columns[1].HeaderText = "NOME";
                dgUsuarios.Columns[2].HeaderText = "USUARIO";
                dgUsuarios.Columns[3].HeaderText = "SENHA";
                dgUsuarios.Columns[4].HeaderText = "TIPO";

                //largura colunas
                dgUsuarios.Columns[0].Width = 50;
                dgUsuarios.Columns[1].Width = 150;
                dgUsuarios.Columns[2].Width = 150;
                dgUsuarios.Columns[3].Width = 150;
                dgUsuarios.Columns[4].Width = 150;
            }
        }
        public static string criptografarSenha(string senha)
        {
            if (string.IsNullOrEmpty(senha))
                return string.Empty;

            var bytes = Encoding.UTF8.GetBytes(senha);
            return Convert.ToBase64String(bytes);
        }

        public static DataTable listarUsuarios()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conectaDB.Conexao))
                {
                    con.Open();
                    string sqlListar = @"
                        SELECT 
                            USU_ID AS ID,
                            USU_NOME AS NOME,
                            USU_LOGIN AS USUARIO,
                            USU_SENHA AS SENHA,
                            USU_TIPO AS TIPO
                        FROM 
                            usuario
                    ";
                    SqlDataAdapter da = new SqlDataAdapter(sqlListar, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Criptografa a coluna SENHA
                    foreach (DataRow row in dt.Rows)
                    {
                        if (dt.Columns.Contains("SENHA"))
                        {
                            string senhaOriginal = row["SENHA"]?.ToString();
                            row["SENHA"] = criptografarSenha(senhaOriginal);
                        }
                    }

                    return dt;
                }
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
                dgUsuarios.AutoGenerateColumns = true;
                dgUsuarios.DataSource = listarUsuarios();
                dgUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgUsuarios.AllowUserToAddRows = false;
                dgUsuarios.AllowUserToDeleteRows = false;
                dgUsuarios.ReadOnly = true;
            }
            catch (SqlException erro)
            {
                MessageBox.Show(erro.Message);
            }
        }
        public void limparTudo()
        {
            lblID.Visible = false;
            txtID.Visible = false;
            cbTipo.SelectedIndex = 0;
            txtNome.Text = string.Empty;
            txtUsuario.Text = string.Empty;
            txtSenha.Text = string.Empty;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            cbTipo.SelectedIndex = 0;

            //quando selecionar 0 ou 1 no combobox, o texto muda para "usuario" ou "admin"
            cbTipo.SelectedIndexChanged += (s, ev) =>
            {
                if (cbTipo.SelectedIndex == 0)
                {
                    cbTipo.Text = "usuario";
                }
                else if (cbTipo.SelectedIndex == 1)
                {
                    cbTipo.Text = "admin";
                }
            };
        }
        private void btnListar_Click(object sender, EventArgs e)
        {
            atualizarGrid();
        }
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            limparTudo();
        }
        private void dgUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            lblID.Visible = true;
            txtID.Visible = true;

            txtID.Text = dgUsuarios.Rows[e.RowIndex].Cells[0].Value?.ToString();
            txtNome.Text = dgUsuarios.Rows[e.RowIndex].Cells[1].Value?.ToString();
            txtUsuario.Text = dgUsuarios.Rows[e.RowIndex].Cells[2].Value?.ToString();
            txtSenha.Text = dgUsuarios.Rows[e.RowIndex].Cells[3].Value?.ToString();
            // Seleciona o tipo no ComboBox conforme o valor da célula
            string tipo = dgUsuarios.Rows[e.RowIndex].Cells[4].Value?.ToString();
            if (!string.IsNullOrEmpty(tipo))
            {
                if (tipo.Equals("usuario", StringComparison.OrdinalIgnoreCase))
                {
                    cbTipo.SelectedIndex = 0;
                }
                else if (tipo.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    cbTipo.SelectedIndex = 1;
                }
                else
                {
                    cbTipo.SelectedIndex = 0; // valor padrão caso não reconheça
                }
            }
            else
            {
                cbTipo.SelectedIndex = 0;
            }
        }
        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text.Trim();
            string usuario = txtUsuario.Text.Trim();
            string senha = txtSenha.Text.Trim();
            string tipo = cbTipo.SelectedIndex == 0 ? "usuario" : "admin";

            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Preencha todos os campos obrigatórios.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(conectaDB.Conexao))
                {
                    con.Open();
                    string sql = @"
                        INSERT INTO usuario (USU_NOME, USU_LOGIN, USU_SENHA, USU_TIPO)
                        VALUES (@nome, @usuario, @senha, @tipo)";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@nome", nome);
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@senha", senha);
                        cmd.Parameters.AddWithValue("@tipo", tipo);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Usuário cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            atualizarGrid();
                            limparTudo();
                        }
                        else
                        {
                            MessageBox.Show("Falha ao cadastrar usuário.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Erro ao cadastrar usuário: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Selecione um usuário para editar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string id = txtID.Text.Trim();
            string nome = txtNome.Text.Trim();
            string usuario = txtUsuario.Text.Trim();
            string senha = txtSenha.Text.Trim();
            string tipo = cbTipo.SelectedIndex == 0 ? "usuario" : "admin";

            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Preencha todos os campos obrigatórios.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(conectaDB.Conexao))
                {
                    con.Open();
                    string sql = @"
                        UPDATE usuario
                        SET USU_NOME = @nome,
                            USU_LOGIN = @usuario,
                            USU_SENHA = @senha,
                            USU_TIPO = @tipo
                        WHERE USU_ID = @id";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@nome", nome);
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@senha", senha);
                        cmd.Parameters.AddWithValue("@tipo", tipo);
                        cmd.Parameters.AddWithValue("@id", id);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Usuário editado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            atualizarGrid();
                            limparTudo();
                        }
                        else
                        {
                            MessageBox.Show("Falha ao editar usuário.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Erro ao editar usuário: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("Selecione um usuário para excluir.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show(
                "Tem certeza que deseja excluir este usuário?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult != DialogResult.Yes)
                return;

            string id = txtID.Text.Trim();

            try
            {
                using (SqlConnection con = new SqlConnection(conectaDB.Conexao))
                {
                    con.Open();
                    string sql = "DELETE FROM usuario WHERE USU_ID = @id";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Usuário excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            atualizarGrid();
                            limparTudo();
                        }
                        else
                        {
                            MessageBox.Show("Falha ao excluir usuário.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Erro ao excluir usuário: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static DataTable pesquisarNome(string termo)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conectaDB.Conexao))
                {
                    string sql = @"
                SELECT 
                    c.col_id AS ID,
                    u.usu_nome AS USUARIO,
                    l.liv_nome AS LIVRO,
                    c.col_qtd AS QUANTIDADE,
                    c.col_valor_unit AS [VALOR UNIT.],
                    (c.col_qtd * c.col_valor_unit) AS [VALOR TOTAL]
                FROM 
                    colecao c
                    INNER JOIN usuario u ON c.col_usu_id = u.usu_id
                    INNER JOIN livro l ON c.col_liv_id = l.liv_id
                WHERE l.liv_nome LIKE @termo";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@termo", "%" + termo + "%");

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            return dt;
                        }
                    }
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
            var resultado = pesquisarNome(txtPesquisa.Text);

            if (resultado != null && resultado.Rows.Count > 0)
            {
                dgUsuarios.DataSource = resultado;
            }
            else
            {
                dgUsuarios.DataSource = null; // Limpa o grid se não houver resultados
            }

            dgUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgUsuarios.AllowUserToAddRows = false;
            dgUsuarios.AllowUserToDeleteRows = false;
            dgUsuarios.ReadOnly = true;
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Form3 voltar = new Form3(nomeUsuario);
            this.Hide();
            voltar.ShowDialog();
        }
    }
}
