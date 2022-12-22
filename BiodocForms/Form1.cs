namespace BiodocForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void addFuncionarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addFuncionario add = new addFuncionario();
            add.ShowDialog();
        }

        private void sAIRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}