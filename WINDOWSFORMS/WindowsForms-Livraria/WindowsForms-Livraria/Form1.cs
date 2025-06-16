using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForms_Livraria
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void usuáriosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Usuario usuario = new Usuario();
            this.Hide();
            usuario.ShowDialog();

        }

        private void autorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Autor autor = new Autor();
            this.Hide();
            autor.ShowDialog();
        }

        private void editoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editora editora = new Editora();
            this.Hide();
            editora.ShowDialog();
        }

        private void usuárioSistemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cadastro cadastro = new Cadastro(); 
            this.Hide();
            cadastro.ShowDialog();
        }

        private void livrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Livros livros = new Livros();
            this.Hide();
            livros.ShowDialog();
        }
    }
}
