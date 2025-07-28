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
    public partial class Form2 : Form
    {
        //Ajustei o construtor para receber o nome do usuário logado
        private string nomeUsuario;
        public Form2(string nomeUsuario)
        {
            InitializeComponent();
            dgColecao.DataBindingComplete += dgColecao_DataBindingComplete;
            this.nomeUsuario = nomeUsuario;
        }

        private void dgColecao_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgColecao.Columns.Count >= 3)
            {
                //cabeçalho
                dgColecao.Columns[0].HeaderText = "ID";
                dgColecao.Columns[1].HeaderText = "USUARIO";
                dgColecao.Columns[2].HeaderText = "LIVRO";
                dgColecao.Columns[3].HeaderText = "QUANTIDADE";
                dgColecao.Columns[4].HeaderText = "VALOR UNIT.";
                dgColecao.Columns[5].HeaderText = "VALOR TOTAL";

                //largura colunas
                dgColecao.Columns[0].Width = 50;
                dgColecao.Columns[1].Width = 150;
                dgColecao.Columns[2].Width = 150;
                dgColecao.Columns[3].Width = 150;
                dgColecao.Columns[4].Width = 150;
                dgColecao.Columns[5].Width = 150;
            }
        }
        public static DataTable listarColecao(int usuId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conectaDB.Conexao))
                {
                    string sqlListar = @"
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
                        WHERE c.col_usu_id = @usuId
                    ";
                    using (SqlCommand cmd = new SqlCommand(sqlListar, con))
                    {
                        cmd.Parameters.AddWithValue("@usuId", usuId);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (SqlException)
            {
                return null;
            }
        }
        public void atualizarGrid()
        {
            try
            {
                dgColecao.AutoGenerateColumns = true;
                dgColecao.DataSource = listarColecao(usuarioLog.Id);
                dgColecao.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgColecao.AllowUserToAddRows = false;
                dgColecao.AllowUserToDeleteRows = false;
                dgColecao.ReadOnly = true;
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
            listLivros.SelectedIndex = 0;
            txtQuantidade.Text = string.Empty;
            txtValor.Text = string.Empty;
            txtTotal.Text = string.Empty;
        }
        private void AtualizarTotal()
        {
            if (decimal.TryParse(txtValor.Text, out decimal valorUnitario) &&
                int.TryParse(txtQuantidade.Text, out int quantidade))
            {
                decimal total = valorUnitario * quantidade;
                txtTotal.Text = total.ToString("C2");
            }
            else
            {
                txtTotal.Text = "0,00";
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

        SqlConnection con = new SqlConnection(conectaDB.Conexao);
        private void listLivros_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            lblUsuario.Text = "Seja bem vindo: " + nomeUsuario;

            // Criei um DataTable para armazenar os dados dos livros
            DataTable dt = new DataTable();

            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT liv_id, liv_nome FROM livro ORDER BY liv_nome", con))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    // Preenche o DataTable com os dados retornados
                    da.Fill(dt);
                }

                // Define a fonte de dados do ComboBox
                listLivros.DataSource = dt;
                listLivros.DisplayMember = "liv_nome";
                listLivros.ValueMember = "liv_id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar livros: " + ex.Message);
            }
            finally //Usei finally pra fechar uma vez só a conexão
            {
                con.Close();
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            //Pega o ID do livro selecionado
            if (listLivros.SelectedValue == null)
            {
                MessageBox.Show("Selecione um livro.");
                return;
            }
            int livro = Convert.ToInt32(listLivros.SelectedValue);

            //Pega o ID do usuário logado
            int usuId;
            try
            {
                usuId = usuarioLog.Id; //Assume que usuarioLog.Id já foi definido na tela de login - Classe usuarioLog
                //E armazena na variável usuId
            }
            catch
            {
                MessageBox.Show("Usuário não identificado.");
                return;
            }

            //Pega a quantidade e valor e armazena nas variáveis
            if (!int.TryParse(txtQuantidade.Text, out int quantidade) || quantidade <= 0)
            {
                MessageBox.Show("Quantidade inválida.");
                return;
            }
            if (!decimal.TryParse(txtValor.Text, out decimal valor) || valor <= 0)
            {
                MessageBox.Show("Valor unitário inválido.");
                return;
            }

            try
            {
                con.Open();
                string sql = "INSERT INTO colecao (col_usu_id, col_liv_id, col_qtd, col_valor_unit) VALUES (@usuario, @livro, @quantidade, @valor)";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuId);
                    cmd.Parameters.AddWithValue("@livro", livro);
                    cmd.Parameters.AddWithValue("@quantidade", quantidade);
                    cmd.Parameters.AddWithValue("@valor", valor);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                        MessageBox.Show("Livro cadastrado na coleção com sucesso!");
                    else
                        MessageBox.Show("Falha ao cadastrar o livro na coleção.");
                }

                atualizarGrid();
                limparTudo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void dgColecao_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            lblID.Visible = true;
            txtID.Visible = true;

            txtID.Text = dgColecao.Rows[e.RowIndex].Cells[0].Value?.ToString();
            string nomeLivro = dgColecao.Rows[e.RowIndex].Cells[2].Value?.ToString();
            if (!string.IsNullOrEmpty(nomeLivro))
            {
                // Procura o item pelo nome e seleciona
                for (int i = 0; i < listLivros.Items.Count; i++)
                {
                    DataRowView item = listLivros.Items[i] as DataRowView;
                    if (item != null && item["liv_nome"].ToString() == nomeLivro)
                    {
                        listLivros.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                listLivros.SelectedIndex = 0;
            }

            txtQuantidade.Text = dgColecao.Rows[e.RowIndex].Cells[3].Value?.ToString();
            txtValor.Text = dgColecao.Rows[e.RowIndex].Cells[4].Value?.ToString();
            txtTotal.Text = dgColecao.Rows[e.RowIndex].Cells[5].Value?.ToString();
        }

        private void txtTotal_TextChanged(object sender, EventArgs e)
        {
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            limparTudo();
        }

        private void txtQuantidade_TextChanged(object sender, EventArgs e)
        {
            AtualizarTotal();
        }

        private void txtValor_TextChanged(object sender, EventArgs e)
        {
            AtualizarTotal();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Selecione um registro para editar.");
                return;
            }

            try
            {
                using (var con = new SqlConnection(conectaDB.Conexao))
                {
                    string sql = @"UPDATE colecao SET  
                                        col_qtd = @quantidade, 
                                        col_valor_unit = @valor 
                                   WHERE col_id = @id";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtID.Text));
                        cmd.Parameters.AddWithValue("@livro", Convert.ToInt32(listLivros.SelectedValue));
                        cmd.Parameters.AddWithValue("@quantidade", Convert.ToInt32(txtQuantidade.Text));
                        cmd.Parameters.AddWithValue("@valor", Convert.ToDecimal(txtValor.Text));

                        con.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            MessageBox.Show("Registro atualizado com sucesso!");
                        else
                            MessageBox.Show("Falha ao atualizar o registro.");
                    }
                }
                atualizarGrid();
                limparTudo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar: " + ex.Message);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            //deleta o registro selecionado pelo evento cellclick do data grid
            if (string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Selecione um registro para excluir.");
                return;
            }

            var confirmResult = MessageBox.Show(
                "Tem certeza que deseja excluir este registro?",
                "Confirmação de Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmResult != DialogResult.Yes)
                return;

            try
            {
                using (var con = new SqlConnection(conectaDB.Conexao))
                {
                    string sql = "DELETE FROM colecao WHERE col_id = @id";
                    using (var cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtID.Text));
                        con.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            MessageBox.Show("Registro excluído com sucesso!");
                        else
                            MessageBox.Show("Falha ao excluir o registro.");
                    }
                }
                atualizarGrid();
                limparTudo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir: " + ex.Message);
            }
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            atualizarGrid();
            limparTudo();
        }

        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            var resultado = pesquisarNome(txtPesquisa.Text);

            if (resultado != null && resultado.Rows.Count > 0)
            {
                dgColecao.DataSource = resultado;
            }
            else
            {
                dgColecao.DataSource = null; // Limpa o grid se não houver resultados
            }

            dgColecao.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgColecao.AllowUserToAddRows = false;
            dgColecao.AllowUserToDeleteRows = false;
            dgColecao.ReadOnly = true;
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            this.Hide();
            login.ShowDialog();
        }
    }
}
