using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Interfaz_Compilador
{
    public partial class Form1 : Form
    {

        private string currentFilePath = "";
        Color commentColor = Color.FromArgb(170,170,170);
        Color symbolColor = Color.FromArgb(4,51,250);
        Color stringColor = Color.FromArgb(68,140,39);
        Color numberColor = Color.FromArgb(156,95,43);
        Color keywordColor = Color.FromArgb(122,62,157);


        public Form1()
        {
            InitializeComponent();

        }


        // Evento Click del botón "Nuevo"
        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentFilePath = "";
            richTextBox1.Text = "";
        }

        // Evento Click del botón "Abrir"
        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = openFileDialog1.FileName;
                try
                {
                    richTextBox1.Text = File.ReadAllText(currentFilePath);
                }
                catch (IOException)
                {
                    MessageBox.Show("Error al abrir el archivo");
                }
            }

        }

        // Evento Click del botón "Guardar"
        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                guardarComoToolStripMenuItem_Click(sender, e);
            }
            else
            {
                try
                {
                    File.WriteAllText(currentFilePath, richTextBox1.Text);
                }
                catch (IOException)
                {
                    MessageBox.Show("Error al guardar el archivo");
                }
            }
        }

        // Evento Click del botón "Guardar Como"
        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = saveFileDialog1.FileName;

                try
                {
                    File.WriteAllText(currentFilePath, richTextBox1.Text);
                }
                catch (IOException)
                {
                    MessageBox.Show("Error al guardar el archivo");
                }
            }

        }


        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void modoOscuroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            commentColor = Color.FromArgb(122, 135, 182);
            symbolColor = Color.FromArgb(255, 215, 5);
            stringColor = Color.FromArgb(194, 237, 127);
            numberColor = Color.FromArgb(120, 204, 240);
            keywordColor = Color.FromArgb(199, 146, 232);
            this.BackColor = Color.FromArgb(49,54,74);
            this.ForeColor = Color.White;
            richTextBox1.BackColor = Color.FromArgb(41,45,62);
            richTextBox1.ForeColor = this.ForeColor;
            menuStrip1.BackColor = Color.FromArgb(37, 41, 58);
            menuStrip1.ForeColor = this.ForeColor;
        }


        private void modoClaroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            commentColor = Color.FromArgb(136, 136, 136);
            symbolColor = Color.FromArgb(4, 51, 250);
            stringColor = Color.FromArgb(68, 140, 39);
            numberColor = Color.FromArgb(156, 95, 43);
            keywordColor = Color.FromArgb(115,87,154);
            this.BackColor = Color.FromArgb(237,237,245);
            this.ForeColor = Color.Black;
            richTextBox1.BackColor = Color.FromArgb(245,245,245);
            richTextBox1.ForeColor = this.ForeColor;
            menuStrip1.BackColor = Color.FromArgb(196,183,215);
            menuStrip1.ForeColor = this.ForeColor;
        }

      

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            string[] keywords = { "int", "float", "double", "string", "using", "namespace", "class", "public", "private", "protected", "void", "return" };
            int selectionStart = richTextBox1.SelectionStart;
            int selectionLength = richTextBox1.SelectionLength;

            // Colorea las palabras clave

            foreach (string palabraClave in keywords)
            {
                int index = 0;
                while (index < richTextBox1.TextLength)
                {
                    int indexOfKeyword = richTextBox1.Find(palabraClave, index, richTextBox1.TextLength, RichTextBoxFinds.WholeWord);
                    if (indexOfKeyword != -1)
                    {
                        richTextBox1.SelectionStart = indexOfKeyword;
                        richTextBox1.SelectionLength = palabraClave.Length;
                        richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
                        richTextBox1.SelectionColor = keywordColor;
                        index = indexOfKeyword + palabraClave.Length;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            for (int i = 0; i < richTextBox1.Text.Length; i++)
            {
               

                // Colorea los símbolos
                if (!char.IsLetterOrDigit(richTextBox1.Text[i]))
                {
                    richTextBox1.Select(i, 1);
                    richTextBox1.SelectionColor = symbolColor;
                }
                // Colorea las cadenas de texto encerradas en comillas dobles
                if (richTextBox1.Text[i] == '\"')
                {
                    int j = i + 1;
                    while (j < richTextBox1.Text.Length && richTextBox1.Text[j] != '\"')
                    {
                        j++;
                    }
                    if (j < richTextBox1.Text.Length && richTextBox1.Text[j] == '\"')
                    {
                        richTextBox1.Select(i, j - i + 1);
                        richTextBox1.SelectionColor = stringColor;
                        i = j;
                    }
                }
                // Colorea las cadenas de texto encerradas en comillas simples
                if (richTextBox1.Text[i] == '\'')
                {
                    int j = i + 1;
                    while (j < richTextBox1.Text.Length && richTextBox1.Text[j] != '\'')
                    {
                        j++;
                    }
                    if (j < richTextBox1.Text.Length && richTextBox1.Text[j] == '\'')
                    {
                        richTextBox1.Select(i, j - i + 1);
                        richTextBox1.SelectionColor = stringColor;
                        i = j;
                    }
                }
                // Colorea los números
                if (char.IsDigit(richTextBox1.Text[i]))
                {
                    int j = i + 1;
                    while (j < richTextBox1.Text.Length && char.IsDigit(richTextBox1.Text[j]))
                    {
                        j++;
                    }
                    if (j < richTextBox1.Text.Length && richTextBox1.Text[j] == '.')
                    {
                        j++;
                        while (j < richTextBox1.Text.Length && char.IsDigit(richTextBox1.Text[j]))
                        {
                            j++;
                        }
                    }
                    if (j < richTextBox1.Text.Length && (richTextBox1.Text[j] == 'f' || richTextBox1.Text[j] == 'F'))
                    {
                        j++;
                    }
                    richTextBox1.Select(i, j - i);
                    richTextBox1.SelectionColor = numberColor;
                    i = j - 1;
                }               

                // Colorea los comentarios de una línea
                if (richTextBox1.Text[i] == '/' && i + 1 < richTextBox1.Text.Length && richTextBox1.Text[i + 1] == '/')
                {
                    int j = i + 2;
                    while (j < richTextBox1.Text.Length && richTextBox1.Text[j] != '\n')
                    {
                        j++;
                    }
                    richTextBox1.Select(i, j - i);
                    richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Italic);
                    richTextBox1.SelectionColor = commentColor;
                    i = j - 1;
                }
                // Colorea los comentarios de varias líneas
                if (richTextBox1.Text[i] == '/' && i + 1 < richTextBox1.Text.Length && richTextBox1.Text[i + 1] == '*')
                {
                    int j = i + 2;
                    while (j < richTextBox1.Text.Length && (richTextBox1.Text[j] != '*' || j + 1 >= richTextBox1.Text.Length || richTextBox1.Text[j + 1] != '/'))
                    {
                        j++;
                    }
                    if (j + 1 < richTextBox1.Text.Length && richTextBox1.Text[j] == '*' && richTextBox1.Text[j + 1] == '/')
                    {
                        j += 2;
                    }

                    
                    richTextBox1.Select(i, j - i);
                    richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Italic);
                    richTextBox1.SelectionColor = commentColor;
                    i = j - 1;

                }
                // Restaura la selección original
               
                richTextBox1.SelectionStart = selectionStart;
                richTextBox1.SelectionLength = selectionLength;
                richTextBox1.SelectionColor = Color.Black;

            }
        }
    }
}
    






