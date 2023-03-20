using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Oracle.DataAccess.Client;
using IBM.Data.DB2;
 
namespace DACampos
{
 
    public partial class Campos : Form
    {
        // Declara componentes de conexão
        private static OracleConnection connORA; // ODAC 12c
        private static DB2Connection connDB2;   // IBM Data Server Provider
        private static SqlConnection connMSSQL; // ADO .NET
 
        // Declara variável do banco de dados
        private static string DBconexao;
 
        public Campos()
        {
            InitializeComponent();
        }
 
        // Cria método de conexão
        public void conectarDB(string banco)
        {
            DBconexao = banco;
 
            if (banco == "oracle")
            {
 
                try
                {
                    // String de Conexao
                    string connectionString =
 
                    // Usuario
                    "User Id=daberto"+
 
                    // Senha
                    ";Password=p@55w0rd" +
 
                    // TNSnames
                    ";Data Source=XE";
 
                    //Conecta ao datasource usando a conexão Oracle
                    connORA = new OracleConnection(connectionString);
 
                    //Abre a conexão com o banco de dados
                    connORA.Open();
                }
                    // Retorna erro
                catch (Exception ex)
                {
                    // Mostra menssagem de erro
                    MessageBox.Show(ex.ToString());
                }
            }
 
            if (banco == "db2")
            {
                try
                {
                    // String de Conexao
                    string connectionString =
 
                        // Servidor
                        "Server=localhost" + 
 
                        // Banco de dados
                        ";Database=DEVA" +
 
                        // Usuario
                        ";UID=db2admin" + 
 
                        // Senha
                        ";PWD=p@55w0rd" + 
 
                        // Timeout
                        ";Connect Timeout=40";
 
                    //Conecta ao datasource usando a conexão DB2
                    connDB2 = new DB2Connection(connectionString);
 
                    //Abre a conexão com o banco de dados
                    connDB2.Open();
 
                }
                // Retorna erro
                catch (Exception ex)
                {
                    // Mostra menssagem de erro
                    MessageBox.Show(ex.ToString());
 
                }
 
            }
 
            if (banco == "mssql")
            {
                try
                {
                    // String de Conexao
                    string connectionString =
 
                        // Servidor
                        "Data Source=localhost" + 
 
                        // Banco de dados
                        ";Initial Catalog=DevAberto" +
 
                        // Usuario
                        ";User ID =devaberto" + 
 
                        // Senha
                        ";Password=p@55w0rd" +
 
                        // Timeout
                        ";Connect Timeout=40";
 
                    //Conecta ao datasource usando a conexão Padrão
                    connMSSQL = new SqlConnection(connectionString);
 
                    //Abre a conexão com o banco de dados
                    connMSSQL.Open();
 
                }
                // Retorna erro
                catch (Exception ex)
                {
                    // Mostra menssagem de erro
                    MessageBox.Show(ex.ToString());
 
                }
 
            }
 
        }
 
        // Evento de clique do botão
        private void button1_Click(object sender, EventArgs e)
        {
            // Cria instancia do objeto
            Campos campos = new Campos();
 
            // A variável abaixo:
            // Define banco de dados
            // oracle = Oracle Database
            // db2    = IBM DB2 Database
            // mssql  = Microsoft SQL Server
            campos.conectarDB("mssql");
 
            // Define instrução SQL
            string sql = "Select * From FUNCIONARIOS Where  ID_FUNCIONARIO = " + textBox6.Text;
 
            // Usando um procedimento de conexão para o driver especifico.
 
            // Oracle - ODAC
            if (DBconexao == "oracle")
            {
                label1.Text = "Database - fields : " + DBconexao;
 
                OracleCommand oracmd = new OracleCommand(sql, connORA);
                OracleDataReader orareader = oracmd.ExecuteReader();
 
                if (orareader.HasRows)
                {
                    while (orareader.Read())
                    {
                        textBox1.Text = Convert.ToString(orareader.GetInt32(0));
                        textBox2.Text = orareader.GetString(1);
                        textBox3.Text = orareader.GetString(2);
                        textBox4.Text = orareader.GetString(3);
                        textBox5.Text = Convert.ToString(orareader.GetDecimal(4));
                    }
                }
 
            }
 
            // IBM Data Server Provider
            if (DBconexao == "db2")
            {
                label1.Text = "Database - fields : " + DBconexao;
 
                DB2Command db2cmd = new DB2Command(sql);
                db2cmd.Connection = connDB2;
                DB2DataReader db2reader = db2cmd.ExecuteReader();
 
                if (db2reader.HasRows)
                {
                    while(db2reader.Read())
                    {
                        textBox1.Text = Convert.ToString(db2reader.GetInt32(0));
                        textBox2.Text = db2reader.GetString(1);
                        textBox3.Text = db2reader.GetString(2);
                        textBox4.Text = db2reader.GetString(3);
                        textBox5.Text = Convert.ToString(db2reader.GetDecimal(4));
                    }
                }
 
            }
 
            // microsoft ADO.NET
           if (DBconexao == "mssql")
           {
               label1.Text = "Database - fields : " + DBconexao;
 
               SqlCommand mssqlcmd = new SqlCommand(sql);
               mssqlcmd.Connection = connMSSQL;
               SqlDataReader mssqlreader = mssqlcmd.ExecuteReader();
 
               if (mssqlreader.HasRows)
               {
                   while (mssqlreader.Read())
                   {
                       textBox1.Text = Convert.ToString(mssqlreader.GetInt32(0));
                       textBox2.Text = mssqlreader.GetString(1);
                       textBox3.Text = mssqlreader.GetString(2);
                       textBox4.Text = mssqlreader.GetString(3);
                       textBox5.Text = Convert.ToString(mssqlreader.GetDecimal(4));
                   }
               }
 
               // Aqui nota-se uma diferença entre os drivers:
               // Microsoft, Oracle e IBM
               // ADO.NET É necessário fechar o DataReader antes de executar um sqlcommand.
               // ODAC não é necessário.
               // IBM DATA Server não é necessário.
 
               // Enquanto o DataReader está em uso, 
               // o Connection associado está ocupado servindo o DataReader.
               // Enquanto estiver neste estado, 
               // nenhuma outra operação pode ser realizada sobre o Connection além de fechá-lo.
               // Os drivers da Oracle e IBM não possuem esta arquitetura e não ocupam a conexão
               // permitindo ainda múltiplas operações sobre ela.
 
               mssqlreader.Close();
           }
        }
 
        private static void executaSQL(string sql)
        {
            // Declara comandos em diferentes drivers
            OracleCommand oracmd;
            DB2Command db2cmd;
            SqlCommand sqlcmd;
 
            // Define banco de dados e executa comandos SQL
            if (DBconexao == "oracle")
            {
                oracmd = new OracleCommand();
                oracmd.Connection = connORA;
                oracmd.CommandText = sql;
 
                try
                {
                    oracmd.ExecuteNonQuery();
                    MessageBox.Show("Ação requerida executada com sucesso!");
                }
                catch (Exception ex)
                {
                    // Mostra menssagem de erro
                    MessageBox.Show(ex.ToString());
                }
 
            }
 
            if (DBconexao == "db2")
            {
                db2cmd = new DB2Command();
                db2cmd.Connection = connDB2;
                db2cmd.CommandText = sql;
                try
                {
                    db2cmd.ExecuteNonQuery();
                    MessageBox.Show("Ação requerida executada com sucesso!");
                }
                catch (Exception ex)
                {
                    // Mostra menssagem de erro
                    MessageBox.Show(ex.ToString());
                }
            }
 
            if (DBconexao == "mssql")
            {
                sqlcmd = new SqlCommand();
                sqlcmd.Connection = connMSSQL;
                sqlcmd.CommandText = sql;
 
                try
                {
                    sqlcmd.ExecuteNonQuery();
                    MessageBox.Show("Ação requerida executada com sucesso!");
                }
                catch (Exception ex)
                {
                    // Mostra menssagem de erro
                    MessageBox.Show(ex.ToString());
                }
            }           
 
        }
 
        private static String trocaDecimal(string conteudo)
        {
            // Substitui decimal na manipulação de SQL
            string troca = conteudo.Replace(",", ".");
            return troca;
        }
 
        // Novo registro
        private void button2_Click(object sender, EventArgs e)
        {
            // Limpa componentes
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            textBox5.Text = null;
 
            // Define foco
            textBox1.Focus();
        }
 
        // Insere registro
        private void button3_Click(object sender, EventArgs e)
        {
            // Cria instrução SQL
            string  sql = "Insert into FUNCIONARIOS values (" +
                textBox1.Text + ", \'" +
                textBox2.Text + "\', \'" +
                textBox3.Text + "\', \'" +
                textBox4.Text + "\', " +
                trocaDecimal(textBox5.Text) + ")";
 
            // Executa sql
            executaSQL(sql);
        }
 
        // Altera registro
        private void button4_Click(object sender, EventArgs e)
        {
            // Cria instrução SQL
            string sql = "Update FUNCIONARIOS SET " +
                            "ID_FUNCIONARIO = " + textBox1.Text + ", " +
                            "NOME = \'" + textBox2.Text + "\', " +
                            "SOBRENOME = \'" + textBox3.Text + "\', " +
                            "CARGO = \'" + textBox4.Text + "\', " +
                            "SALARIO = " + trocaDecimal(textBox5.Text) + " " +
                            "Where ID_FUNCIONARIO = " + textBox1.Text;
 
            // Executa sql
            executaSQL(sql);
        }
 
        // Deleta registro
        private void button5_Click(object sender, EventArgs e)
        {
            // Cria instrução SQL
            string sql = "Delete from FUNCIONARIOS Where ID_FUNCIONARIO = " + textBox1.Text;
 
            // Executa sql
            executaSQL(sql);
            // Executa clique no botão
            button2.PerformClick();
        }
    }
}