using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForms_Livraria
{
    public partial class Livros : Form
    {
        public Livros()
        {
            InitializeComponent();
            atualizarGrid();
            dgLivros.DataBindingComplete += dgLivros_DataBindingComplete;

            //CARREGAR OS CAMPOS PARA A COMBOBOX - Autor
            cbAutor.DataSource = ListarAutor();
            cbAutor.DisplayMember = "autor_nome";
            cbAutor.ValueMember = "autor_id";

            //CARREGAR OS CAMPOS PARA A COMBOBOX - editora
            cbEditora.DataSource = ListarEditora();
            cbEditora.DisplayMember = "editora_nome";
            cbEditora.ValueMember = "editora_id";
        }

        SqlConnection con = new SqlConnection(ConectaDB.Conexao);

        private void dgLivros_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgLivros.Columns.Count >= 9)
            {
                //cabeçalho
                dgLivros.Columns[0].HeaderText = "ID";
                dgLivros.Columns[1].HeaderText = "NOME";
                dgLivros.Columns[2].HeaderText = "AUTOR";
                dgLivros.Columns[3].HeaderText = "EDITORA";
                dgLivros.Columns[4].HeaderText = "DESCRICAO";
                dgLivros.Columns[5].HeaderText = "Nº PAGINAS";
                dgLivros.Columns[6].HeaderText = "DATA PUBLICACAO";
                dgLivros.Columns[7].HeaderText = "VALOR";
                dgLivros.Columns[8].HeaderText = "CAPA";

                //largura colunas
                dgLivros.Columns[0].Width = 50;
                dgLivros.Columns[1].Width = 150;
                dgLivros.Columns[2].Width = 50;
                dgLivros.Columns[3].Width = 100;
                dgLivros.Columns[4].Width = 100;
                dgLivros.Columns[5].Width = 100;
                dgLivros.Columns[6].Width = 100;
                dgLivros.Columns[7].Width = 100;
                dgLivros.Columns[8].Width = 100;
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            this.Hide();
            form1.ShowDialog();
        }
        public static DataTable ListarAutor()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConectaDB.Conexao);
                con.Open();
                string sql = "Select autor_id, autor_nome FROM autor";
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                return dt;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        //FUNÇÃO DE BUSCAR AS EDITORAS
        public static DataTable ListarEditora()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConectaDB.Conexao);
                con.Open();
                string sql = "Select editora_id, editora_nome FROM editora";
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                return dt;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public static DataTable listarLivros()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConectaDB.Conexao);
                con.Open();
                string sqlListar = "select * from Livros";
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
                dgLivros.AutoGenerateColumns = true;
                dgLivros.DataSource = listarLivros();
                dgLivros.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                //configurações de segurança abaixo
                dgLivros.AllowUserToAddRows = false;
                dgLivros.AllowUserToDeleteRows = false;
                dgLivros.ReadOnly = true;
            }
            catch (SqlException erro)
            {
                MessageBox.Show(erro.Message);
            }
        }

        public void LimparTudo()
        {
            //date time e capa não são necessarios;
            txtID.Clear();
            txtNome.Clear();
            txtDescricao.Clear();
            txtValor.Clear();
            cbAutor.SelectedIndex = -1;
            cbEditora.SelectedIndex = -1;
            txtPaginas.Clear();
            txtNome.Focus();
        }

        private void btnTesteCon_Click(object sender, EventArgs e)
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

        public static DataTable pesquisarNome(string termo)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConectaDB.Conexao))
                {
                    string sql = "SELECT * FROM Livro WHERE livro_nome like @termo";
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
            dgLivros.DataSource = pesquisarNome(txtPesquisa.Text);

            dgLivros.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgLivros.AllowUserToAddRows = false;
            dgLivros.AllowUserToDeleteRows = false;
            dgLivros.ReadOnly = true;
        }

        private void btnLista_Click(object sender, EventArgs e)
        {
            atualizarGrid();
        }

        private void dgLivros_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int linha = e.RowIndex;

            lblID.Visible = true;
            txtID.Visible = true;

            //obtem o ID da celula 0
            int idLivro = Convert.ToInt32(dgLivros.Rows[linha].Cells[0].Value);
            //chamar a função de carregar img do banco
            CarregarIMG(idLivro);

            //preenchimento dos campos do select do dgLivros
            txtID.Text = dgLivros.Rows[linha].Cells[1].Value?.ToString();
            txtNome.Text = dgLivros.Rows[linha].Cells[2].Value?.ToString();
            //converte o id para o nome (fk_autor e fk_editora):
            cbAutor.SelectedValue = Convert.ToInt32(dgLivros.Rows[linha].Cells[3].Value);
            cbEditora.SelectedValue = Convert.ToInt32(dgLivros.Rows[linha].Cells[4].Value);
            txtDescricao.Text = dgLivros.Rows[linha].Cells[5].Value?.ToString();
            txtPaginas.Text = dgLivros.Rows[linha].Cells[6].Value?.ToString();
            //data publicação:
            var valorData = dgLivros.Rows[linha].Cells[7].Value;
            if (valorData != null && DateTime.TryParse(
                valorData.ToString(), out DateTime dataPub))
            {
                dtPublicacao.Text = dataPub.ToString("dd/MM/yyyy");
            }
            else
            {
                dtPublicacao.Text = string.Empty;
            }
            txtValor.Text = dgLivros.Rows[linha].Cells[8].Value?.ToString();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Arquivos de Imagem|*.jpg;*.jpeg;*.png;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                picCapa.Image = Image.FromFile(ofd.FileName);
            }
        }

        private byte[] ConverterIMG()
        {
            if (picCapa.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    // Clonar a imagem para evitar bloqueio de stream
                    using (Image clone = (Image)picCapa.Image.Clone())
                    {
                        // Definir um formato de imagem fixo (ex.: PNG)
                        clone.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    return ms.ToArray();
                }
            }
            return null;
        }


        //CARREGAR IMG DO BANCO
        private void CarregarIMG(int idLivro)
        {
            using (SqlConnection con = new SqlConnection(ConectaDB.Conexao))
            {
                string sql = "SELECT livro_capa FROM livro WHERE livro_id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("id", idLivro);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read() && reader["livro_capa"] != DBNull.Value)
                    {
                        byte[] imagemBytes = (byte[])reader["livro_capa"];
                        using (MemoryStream ms = new MemoryStream(imagemBytes))
                        {
                            picCapa.Image = Image.FromStream(ms);
                        }
                    }
                }
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            //CHAMAR A CONVERSÃO DE IMG
            byte[] imagemBytes = ConverterIMG();

            try
            {
                //validação de campos se estão preenchidos
                if (txtNome.Text == "" || txtValor.Text == string.Empty)
                {
                    MessageBox.Show("Preencha ps campos obrigatórios", "Importante!",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    con.Close();
                    con.Open(); 
                    
                    string sql = "Insert into Livro(livro_nome,livro_autor_fk, livro_editora_fk, livro_descricao, " +
                        "livro_npaginas, livro_publicacao, livro_valor, livro_capa) values (@nome, @autor, @editora, @descricao, " +
                        "@paginas, @publicacao, @valor, @capa)";

                    SqlCommand cmd = new SqlCommand(sql, con); //aplicando o insert

                    //PEGANDO AS VARIAVEIS NOS CAMPOS
                    cmd.Parameters.Add("@nome", SqlDbType.VarChar).Value = txtNome.Text;
                    cmd.Parameters.AddWithValue("@autor", cbAutor.SelectedValue);
                    cmd.Parameters.AddWithValue("@editora", cbEditora.SelectedValue);
                    cmd.Parameters.Add("@descricao", SqlDbType.VarChar).Value = txtDescricao.Text;
                    cmd.Parameters.Add("@paginas", SqlDbType.Int).Value = Convert.ToInt32(txtPaginas.Text);
                    cmd.Parameters.Add("@publicacao", SqlDbType.DateTime).Value = Convert.ToDateTime(dtPublicacao.Text);
                    cmd.Parameters.Add("@valor", SqlDbType.Int).Value = Convert.ToInt32(txtValor.Text);
                    cmd.Parameters.Add("@capa", SqlDbType.VarBinary).Value = imagemBytes;
                                        
                    //EXECUTANDO O QUERY
                    cmd.ExecuteNonQuery();
                    con.Close();

                    LimparTudo();  
                    atualizarGrid();

                    MessageBox.Show("Cadastro OK", "Sistema Funcional",
                        MessageBoxButtons.OK, MessageBoxIcon.Information); ;
                }
            }
            catch (SqlException erro) { MessageBox.Show("Erro:" + erro.Message); }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            byte[] imagemBytes = ConverterIMG();

            string sql = @"UPDATE Livro SET livro_nome = @nome, livro_autor_fk = @autor, livro_editora_fk = @editora, livro_descricao = @descricao, 
            livro_npaginas = @paginas, livro_publicacao = @publicacao, livro_valor = @valor, livro_capa = @capa 
            WHERE livro_id = @id";

            using (var con = new SqlConnection(ConectaDB.Conexao))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtID.Text));
                cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                cmd.Parameters.AddWithValue("@autor", cbAutor.SelectedValue);
                cmd.Parameters.AddWithValue("@editora", cbEditora.SelectedValue);
                cmd.Parameters.AddWithValue("@descricao", txtDescricao.Text);
                cmd.Parameters.AddWithValue("@paginas", Convert.ToInt32(txtPaginas.Text));
                cmd.Parameters.AddWithValue("@publicacao", Convert.ToDateTime(dtPublicacao.Text));
                cmd.Parameters.AddWithValue("@valor", Convert.ToInt32(txtValor.Text));
                cmd.Parameters.Add("@capa", SqlDbType.VarBinary).Value = (object)imagemBytes ?? DBNull.Value;
                

                con.Open();
                cmd.ExecuteNonQuery();
            }
            atualizarGrid();
            LimparTudo();
        }

        private void btnDeletar_Click(object sender, EventArgs e)
        {
            //Pergunta de confirmação
            var resp = MessageBox.Show(
                "Tem certeza da exclusão?", "Confirmação",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resp == DialogResult.Yes)
            {
                //Executar o delete sem sim
                string sql = "DELETE from livro WHERE livro_id = @id";
                using (var con = new SqlConnection(ConectaDB.Conexao))
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtID.Text));
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                atualizarGrid();
                LimparTudo();
            }
        }

        private void btnAddAutor_Click(object sender, EventArgs e)
        {
            // Abre o formulário de autores como modal
            using (Autor frmAut = new Autor())
            {
                frmAut.ShowDialog();

                // Após fechar o frm_autores, atualiza o combo de autores
                cbAutor.DataSource = null; // limpa a fonte de dados para evitar erros
                cbAutor.DataSource = ListarAutor();
                cbAutor.DisplayMember = "autor_nome";
                cbAutor.ValueMember = "autor_id";
            }
        }

        private void btnAddEditora_Click(object sender, EventArgs e)
        {
            // Abre o formulário de autores como modal
            using (Editora frmEdit = new Editora())
            {
                frmEdit.ShowDialog();

                // Após fechar o frm_autores, atualiza o combo de autores
                cbEditora.DataSource = null; // limpa a fonte de dados para evitar erros
                cbEditora.DataSource = ListarEditora();
                cbEditora.DisplayMember = "editora_nome";
                cbEditora.ValueMember = "editora_id";
            }
        }
    }
}
