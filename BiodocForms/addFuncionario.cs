using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BiodocForms
{
    public partial class addFuncionario : Form
    {
        public addFuncionario()
        {
            InitializeComponent();

            

        }


        //CONNECTION DB

        SqlConnection sqlCon = null;
        private string strcon = "Server=localhost\\sqlexpress;Initial Catalog=BiotecForms;TrustServerCertificate=true;Integrated Security=true";
        private string strSql = string.Empty;


        private void btn_add_Click(object sender, EventArgs e)
        {
            
        }

        // SALVAR

        private void btn_salvar_Click(object sender, EventArgs e)
        {
            strSql = "insert into biodocFuncionarios (Nome,Telefone,Celular,Empresa,Endereco,Numero,CEP,Bairro,Cargo,Salario) values  (@Nome,@Telefone,@Celular,@Empresa,@Endereco,@Numero,@CEP,@Bairro,@Cargo,@Salario)";

            sqlCon = new SqlConnection(strcon);

            SqlCommand cmd = new SqlCommand(strSql,sqlCon);

            cmd.Parameters.Add("@Nome",SqlDbType.VarChar).Value= txt_nome.Text;
            cmd.Parameters.Add("@Telefone", SqlDbType.VarChar).Value = maskedTextBox1.Text;
            cmd.Parameters.Add("Celular", SqlDbType.VarChar).Value = maskedTextBox2.Text;
            cmd.Parameters.Add("@Empresa", SqlDbType.VarChar).Value = txt_empresa.Text;
            cmd.Parameters.Add("@Endereco", SqlDbType.VarChar).Value = txt_endereco.Text;
            cmd.Parameters.Add("@Numero", SqlDbType.VarChar).Value = txt_numero.Text;
            cmd.Parameters.Add("@CEP", SqlDbType.VarChar).Value = txt_cep.Text;
            cmd.Parameters.Add("@Bairro", SqlDbType.VarChar).Value = txt_bairro.Text;
            cmd.Parameters.Add("@Cargo", SqlDbType.VarChar).Value = checkedListBox1.Text;
            cmd.Parameters.Add("@Salario", SqlDbType.VarChar).Value = txt_salario.Text;

            try
            {
                sqlCon.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("CADASTRO EFETUADO COM SUCESSO!");

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { sqlCon.Close(); }

            txt_nome.Clear();
            maskedTextBox1.Clear();
            maskedTextBox2.Clear();
            txt_empresa.Clear();
            txt_endereco.Clear();
            txt_numero.Clear();
            txt_bairro.Clear();
            checkedListBox1.Items.Clear();
            txt_salario.Clear();



        }

        // SELECT Pesquisar

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            strSql = "SELECT * FROM biodocFuncionarios WHERE Nome=@pesquisar";

            sqlCon = new SqlConnection(strcon);

            SqlCommand cmd = new SqlCommand(strSql, sqlCon);

            cmd.Parameters.Add("@pesquisar", SqlDbType.VarChar).Value = txt_pesquisar.Text;

            try
            {
                if(txt_pesquisar.Text == string.Empty)
                {
                    MessageBox.Show("VOCÊ NÃO DIGITOU UM NOME");
                }

                sqlCon.Open();
                SqlDataReader dataReader= cmd.ExecuteReader();

                if (dataReader.HasRows == false)
                {
                    throw new Exception("ESTE NOME NÃO ESTÁ CADASTRADO");
                }
                dataReader.Read();

                txt_nome.Text = Convert.ToString(dataReader["Nome"]);
                maskedTextBox1.Text = Convert.ToString(dataReader["Telefone"]);
                maskedTextBox2.Text = Convert.ToString(dataReader["Celular"]);
                txt_empresa.Text = Convert.ToString(dataReader["Empresa"]);
                txt_endereco.Text = Convert.ToString(dataReader["Endereco"]);
                txt_numero.Text = Convert.ToString(dataReader["Numero"]);
                txt_cep.Text = Convert.ToString(dataReader["CEP"]);
                txt_bairro.Text = Convert.ToString(dataReader["Bairro"]);
                checkedListBox1.Text = Convert.ToString(dataReader["Cargo"]);
                txt_salario.Text = Convert.ToString(dataReader["Salario"]);

            }
            catch(Exception ex)

            {
                MessageBox.Show(ex.Message);
            }
            finally { sqlCon.Close(); }

            txt_pesquisar.Clear();
        }

        // VIACEP CEP

        private void btn_buscarCep_Click(object sender, EventArgs e)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://viacep.com.br/ws/" + txt_cep.Text + "/json/");
            request.AllowAutoRedirect = false;
            HttpWebResponse ChecaServidor = (HttpWebResponse)request.GetResponse();

            if (ChecaServidor.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show("Servidor indisponível");
                return; // Sai da rotina
            }

            using (Stream webStream = ChecaServidor.GetResponseStream())
            {
                if (webStream != null)
                {
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        string response = responseReader.ReadToEnd();
                        response = Regex.Replace(response, "[{},]", string.Empty);
                        response = response.Replace("\"", "");

                        String[] substrings = response.Split('\n');

                        int cont = 0;
                        foreach (var substring in substrings)
                        {
                            if (cont == 1)
                            {
                                string[] valor = substring.Split(":".ToCharArray());
                                if (valor[0] == "  erro")
                                {
                                    MessageBox.Show("CEP não encontrado");
                                    txt_cep.Focus();
                                    return;
                                }
                            }


                            //Bairro
                            if (cont == 4)
                            {
                                string[] valor = substring.Split(":".ToCharArray());
                                txt_bairro.Text = valor[1];
                            }

                            //Logradouro
                            if (cont == 2)
                            {
                                string[] valor = substring.Split(":".ToCharArray());
                                txt_endereco.Text = valor[1];
                            }

                            cont++;
                        }
                    }
                }
            }
        }


    

    // update editar

    private void btn_editar_Click(object sender, EventArgs e)
        {
            strSql = "update biodocFuncionarios set Nome=@Nome, Telefone=@Telefone, Celular=@Celular, Empresa=@Empresa," +
                "Endereco=@Endereco, Numero=@Numero, CEP=@CEP, Bairro=@Bairro, Cargo=@Cargo, Salario=@Salario";
            


            sqlCon = new SqlConnection(strcon);

            SqlCommand cmd = new SqlCommand(strSql, sqlCon);

            cmd.Parameters.Add("@Nome", SqlDbType.VarChar).Value = txt_nome.Text;
            cmd.Parameters.Add("@Telefone", SqlDbType.VarChar).Value = maskedTextBox1.Text;
            cmd.Parameters.Add("Celular", SqlDbType.VarChar).Value = maskedTextBox2.Text;
            cmd.Parameters.Add("@Empresa", SqlDbType.VarChar).Value = txt_empresa.Text;
            cmd.Parameters.Add("@Endereco", SqlDbType.VarChar).Value = txt_endereco.Text;
            cmd.Parameters.Add("@Numero", SqlDbType.VarChar).Value = txt_numero.Text;
            cmd.Parameters.Add("@CEP", SqlDbType.VarChar).Value = txt_cep.Text;
            cmd.Parameters.Add("@Bairro", SqlDbType.VarChar).Value = txt_bairro.Text;
            cmd.Parameters.Add("@Cargo", SqlDbType.VarChar).Value = checkedListBox1.Text;
            cmd.Parameters.Add("@Salario", SqlDbType.VarChar).Value = txt_salario.Text;

            try
            {
                sqlCon.Open();

                cmd.ExecuteNonQuery();
                MessageBox.Show("CADASTRO EDITADO COM SUCESSO!");


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally 
            { 
                sqlCon.Close(); 
            }

            txt_nome.Clear();
            maskedTextBox1.Clear();
            maskedTextBox2.Clear();
            txt_empresa.Clear();
            txt_endereco.Clear();
            txt_numero.Clear();
            txt_bairro.Clear();
            checkedListBox1.Items.Clear();
            txt_salario.Clear();
        }

        //Delete

        private void btn_excluir_Click(object sender, EventArgs e)
        {
            strSql = "delete from biodocFuncionarios where Nome=@Nome";


            sqlCon = new SqlConnection(strcon);

            SqlCommand cmd = new SqlCommand(strSql, sqlCon);

            cmd.Parameters.Add("@Nome", SqlDbType.VarChar).Value = txt_nome.Text;

            try
            {
                sqlCon.Open();
                cmd.ExecuteNonQuery();

                MessageBox.Show("CADASTRO EXCLUIDO COM SUCESSO");
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { sqlCon.Close();}

            txt_nome.Clear();
            maskedTextBox1.Clear();
            maskedTextBox2.Clear();
            txt_empresa.Clear();
            txt_endereco.Clear();
            txt_numero.Clear();
            txt_bairro.Clear();
            checkedListBox1.Items.Clear();
            txt_salario.Clear();
        }
    }
}
