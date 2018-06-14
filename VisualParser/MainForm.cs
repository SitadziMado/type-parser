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
using VisualParser.Core;

namespace VisualParser
{
    public partial class MainForm : Form
    {
        private BinaryTree<Identifier> mTree = new BinaryTree<Identifier>();
        private IdentifierParser mParser = new IdentifierParser(); 

        public MainForm()
        {
            InitializeComponent();
        }

        private void ParseButton_Click(object sender, EventArgs e)
        {
            try
            {
                mTree = mParser.FromString(InputText.Text);
                
                MessageBox.Show(
                    "Исходный код успешно распознан.",
                    "Успех!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                int i = 1;

                var sb = new StringBuilder();

                foreach (var v in mTree)
                    sb.AppendFormat("{0}:\t{1}" + Environment.NewLine + Environment.NewLine, i++, v);

                OutputText.Text = sb.ToString();
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
