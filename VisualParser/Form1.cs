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
                    in codeText.Text.Split(';')
                    where v.Length > 0
                    select Identifier.Factory.FromString(v.Trim())
                ).ToBinaryTree();

                MessageBox.Show("Исходный код успешно распознан.");

                int i = 1;

                var sb = new StringBuilder();

                foreach (var v in tree)
                    sb.AppendFormat("{0}: {1}" + Environment.NewLine, i++, v);

                outputText.Text = sb.ToString();
            }
            catch (ParserException ex)
            {
                Console.WriteLine(
                    string.Format(
                        "Исходный файл содержит ошибку." //, 
                                                         // tree?.Count + 2
                    )
                );
            }
        }
    }
}
