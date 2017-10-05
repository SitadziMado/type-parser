using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TypeParser;

namespace VisualParser
{
    public partial class Form1 : Form
    {
        private BinaryTree<Identifier> tree = new BinaryTree<Identifier>();
         
        public Form1()
        {
            InitializeComponent();
        }

        private void parseButton_Click(object sender, EventArgs e)
        {
            try
            {
                tree = (
                    from v
                    in codeText.Text.Trim().Split(';')
                    where v.Length > 0
                    select Identifier.Factory.FromString(v.Trim())
                ).ToBinaryTree();
                
                MessageBox.Show(
                    "Исходный код успешно распознан.",
                    "Успех!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                int i = 1;

                var sb = new StringBuilder();

                foreach (var v in tree)
                    sb.AppendFormat("{0}:\t{1}" + Environment.NewLine + Environment.NewLine, i++, v);

                outputText.Text = sb.ToString();
            }
            catch (ParserException ex)
            {
                MessageBox.Show(
                    string.Format(
                        "Исходный файл содержит ошибку: " + ex.Message
                    ),
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation
                );
            }
        }
    }
}
