using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WFDotNetCoreGravarDadosMySQL
{
    public partial class Form1 : Form
    {
        MySqlConnection Conexao;
        
        // Criar String de para localizar e dar acesso para o banco de dados

        private string data_source = "datasource=localhost;username=root;password=Lucaio10@;database=dados";


        // Criar uma variavel null para a criação do update

        private int? id_contado_selecionado = null;
        public Form1()
        {
            InitializeComponent();




            //Configuração das colunas

            lst_contato.View = View.Details;
            lst_contato.LabelEdit = true;
            lst_contato.AllowColumnReorder = true;
            lst_contato.FullRowSelect = true;
            lst_contato.GridLines = true;





            // Criando colunas, Ordem de criação é titulo da coluna, tamanho e posição

            lst_contato.Columns.Add("ID", 25, HorizontalAlignment.Left);
            lst_contato.Columns.Add("Nome", 160, HorizontalAlignment.Left);
            lst_contato.Columns.Add("NUMERO", 160, HorizontalAlignment.Left);
            lst_contato.Columns.Add("CPF", 160, HorizontalAlignment.Left);
            lst_contato.Columns.Add("EMAIL", 160, HorizontalAlignment.Left);

            // Chamando a função para listar o banco de dados assim que inicia o projeto

            carregar_dados();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                

                // Criar a conexão com MySQL

                Conexao = new MySqlConnection(data_source);
                Conexao.Open();

                //Instanciei uma class command e passei a conexão 
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Conexao;

                //Diferenciação do Insert para o Upgrade. Será necessaário criar um IF e Else para diferenciar a funcionalidade do botão

                if(id_contado_selecionado == null)
                {
                    //Ocorrerá o Insert

                    //Comando SQL de acordo com as documentações
                    cmd.CommandText = "INSERT INTO cadastro (nome, numero,cpf,email) " +
                                      "VALUES" + "(@nome,@numero,@cpf,@email) ";

                    // TIRAR DUVIDA DPS PQ EU APRENDI COM PREPARE E NAO COM CLEAR
                    cmd.Parameters.Clear();

                    cmd.Parameters.AddWithValue("@nome", txt_nome.Text);
                    cmd.Parameters.AddWithValue("@numero", txt_numero.Text);
                    cmd.Parameters.AddWithValue("@cpf", txt_cpf.Text);
                    cmd.Parameters.AddWithValue("@email", txt_email.Text);

                    //Execução feito para Update,Insert e Delete
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Contato Inserido", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    //Ocorrerá o Update nos contatos

                    //Comando SQL para realizar o UPDATE
                    cmd.CommandText = "UPDATE cadastro SET nome=@nome, numero=@numero,cpf=@cpf,email=@email WHERE id=@id ";

                   
                    cmd.Parameters.Clear();

                    cmd.Parameters.AddWithValue("@nome", txt_nome.Text);
                    cmd.Parameters.AddWithValue("@numero", txt_numero.Text);
                    cmd.Parameters.AddWithValue("@cpf", txt_cpf.Text);
                    cmd.Parameters.AddWithValue("@email", txt_email.Text);
                    cmd.Parameters.AddWithValue("@id", id_contado_selecionado);

                    //Execução feito para Update,Insert e Delete
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Contato Atualizado", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);


                }

                // Limpando os campos com a informações que preenchemos

                txt_nome.Text = String.Empty;
                txt_numero.Text = String.Empty;
                txt_cpf.Text = String.Empty;
                txt_email.Text = String.Empty;

                // Chamando a função void para ler o banco e já preencher o campo quando iniciar o sistema
                Zerar_Formulario();
                carregar_dados();
                

            } 
            catch (MySqlException ex) 
            {
                MessageBox.Show(ex.Message);
            }
            catch(Exception ex)  
            { 
               MessageBox.Show("Ocorreu: " + ex.Message, "Erro", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            finally
            {
                Conexao.Close();
            }
        }

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            try
            {
                // Criamos uma conexão igual a do comando insert

                Conexao = new MySqlConnection(data_source);
                Conexao.Open();

                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = Conexao;


                // Criamos o comando SQL

                cmd.CommandText = "SELECT * FROM CADASTRO WHERE NOME LIKE @q OR EMAIL LIKE @q";

                cmd.Parameters.Clear();

                // Como no comando SQL foi apenas para buscar o nome no campo do cmd.parameters

                cmd.Parameters.AddWithValue("@q", "%" + txt_buscar.Text + "%");


                // MuSQLDataReader vai ler o que vem do banco de dados

                MySqlDataReader reader = cmd.ExecuteReader();

                lst_contato.Items.Clear();

                while (reader.Read())
                {
                    string[] row =
                    {
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4),
                     };

                    lst_contato.Items.Add(new ListViewItem(row));
                } 
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error" + ex.Message + "Ocorreu" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            Conexao.Close();
            }
      
        }
        private void carregar_dados()
        {

            // Essa função eu copiei e colei o codigo do button buscar para iniciar o programa com a lista feita
            try
            {
                // Criamos uma conexão igual a do comando insert

                Conexao = new MySqlConnection(data_source);
                Conexao.Open();

                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = Conexao;


                // Criamos o comando SQL

                cmd.CommandText = "SELECT * FROM CADASTRO ORDER BY id DESC";

                cmd.Parameters.Clear();

                // MuSQLDataReader vai ler o que vem do banco de dados

                MySqlDataReader reader = cmd.ExecuteReader();

                lst_contato.Items.Clear();

                while (reader.Read())
                {
                    string[] row =
                    {
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4),
                     };

                    lst_contato.Items.Add(new ListViewItem(row));
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error" + ex.Message + "Ocorreu" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Conexao.Close();
            }
        }

        private void lst_contato_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            // Instanciamos a listview para encontrar o item apontado dentro do campo
            ListView.SelectedListViewItemCollection itens_selecionados = lst_contato.SelectedItems;


            // Vamos percorrer a lista (listviewItens)
            foreach (ListViewItem item in itens_selecionados) 
            {
                // Utilizei a string nula que criei no inicio do programa e converti para int o valor do campo do formulado que era string
                id_contado_selecionado = Convert.ToInt32(item.SubItems[0].Text);
                
                // O indice apontado dentro da arrauy é para selecionar a coluna da tabela
                txt_nome.Text = item.SubItems[1].Text;
                txt_numero.Text = item.SubItems[2].Text;
                txt_cpf.Text = item.SubItems[3].Text;
                txt_email.Text = item.SubItems[4].Text;
                
                btn_excluir.Visible = true;
  
            }

        }

       
        private void button2_Click(object sender, EventArgs e)
        {

            Zerar_Formulario();
        }

        private void Zerar_Formulario()
        {

            // Botão para limpar APENAS os campos de inserção de informações
            id_contado_selecionado = null;

            txt_nome.Text = String.Empty;
            txt_numero.Text = String.Empty;
            txt_cpf.Text = String.Empty;
            txt_email.Text = String.Empty;

            txt_nome.Focus();

            btn_excluir.Visible = false;

        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            excluir_contatos();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            excluir_contatos();
        }

        private void excluir_contatos()
        {
            try
            {
                // Criar uma messageBox para decisão de exclusão utilizando o dialogresult para capturar o evento do usuário
                DialogResult conf = MessageBox.Show("Tem certerza que deseja excluir o registro", "Ops tem certeza?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);


                
                if (conf == DialogResult.Yes)
                {
                    // Abrindo conexão
                    Conexao = new MySqlConnection(data_source);
                    Conexao.Open();

                    MySqlCommand cmd = new MySqlCommand();

                    cmd.Connection = Conexao;

                    // Comando SQL para excluir
                    cmd.CommandText = "DELETE FROM cadastro WHERE id=@id";

                    cmd.Parameters.AddWithValue("@id", id_contado_selecionado);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Contato Excluído", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    carregar_dados();
                    Zerar_Formulario();

                    btn_excluir.Visible = false;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error" + ex.Message + "Ocorreu" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro: " + ex);
            }
            finally
            {
                Conexao.Close();
            }

        }
    }
}
