using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;

namespace LibJurisprudencia
{
    public class DAL
    {
        string connectionString1 = "Data Source=;Initial Catalog=;User ID=;Password=;"; // homologação
        string connectionString2 = "Data Source=;Initial Catalog=;User ID=;Password=;"; // homologação


        public List<Jurisprudencia> ViewJurisprudencia(int inicial, int total)
        {
            int i = 1;

            List<Jurisprudencia> lista = new List<Jurisprudencia>();
            Jurisprudencia jurisprudencia;
            using (SqlConnection connection = new SqlConnection(connectionString1))
            {
                string queryString = "SELECT * FROM etce.v_jurisprudencia_tcese";
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        if ((i >= inicial) && (i <= total))
                        {
                            
                            jurisprudencia = new Jurisprudencia();
                            jurisprudencia.CodProcesso = reader[0].ToString().TrimEnd();
                            jurisprudencia.Relator = reader[2].ToString().TrimEnd();
                            jurisprudencia.TipoProcesso = reader[3].ToString().TrimEnd();
                            jurisprudencia.TipoDecisao = reader[4].ToString().TrimEnd();
                            jurisprudencia.DataJulgamento =  Convert.ToDateTime(reader[5]);
                            jurisprudencia.UG = reader[6].ToString().TrimEnd();
                            jurisprudencia.Caminho = reader[7].ToString().TrimEnd();
                            jurisprudencia.Cripto = reader[8].ToString().TrimEnd();
                            jurisprudencia.DataPublicacao = Convert.ToDateTime(reader[9]);
                            jurisprudencia.Eletronico = Convert.ToInt32(reader[10]);
                            jurisprudencia.CodSessao = Convert.ToInt32(reader[1]);
                            lista.Add(jurisprudencia);
                        }
                        i++;

                    }

                    reader.Close();
                }
                catch (Exception ex)
                {

                }
                return lista;
            }
        }
        public List<Jurisprudencia> ViewInsertJurisprudencia()
        {
            List<Jurisprudencia> lista = new List<Jurisprudencia>();
            Jurisprudencia jurisprudencia;
            using (SqlConnection connection = new SqlConnection(connectionString1))
            {

                string queryString = "select * from etce.v_jurisprudencia_tcese_insert";

                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {           
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
 
                        jurisprudencia = new Jurisprudencia();
                        jurisprudencia.CodProcesso = reader[0].ToString().TrimEnd();
                        jurisprudencia.Relator = reader[2].ToString().TrimEnd();
                        jurisprudencia.TipoProcesso = reader[3].ToString().TrimEnd();
                        jurisprudencia.TipoDecisao = reader[4].ToString().TrimEnd();

                        string dataJulgamento = Convert.ToString(reader[5]);
                        if (!String.IsNullOrEmpty(dataJulgamento))
                            jurisprudencia.DataJulgamento = Convert.ToDateTime(reader[5]);

                        jurisprudencia.UG = reader[6].ToString().TrimEnd();
                        jurisprudencia.Caminho = reader[7].ToString().TrimEnd();
                        jurisprudencia.Cripto = reader[8].ToString().TrimEnd();

                        string dataPublicacao = Convert.ToString(reader[9]);
                        if (!String.IsNullOrEmpty(dataPublicacao))
                            jurisprudencia.DataPublicacao = Convert.ToDateTime(reader[9]);

                        jurisprudencia.Eletronico = Convert.ToInt32(reader[10]);
                        jurisprudencia.CodSessao = Convert.ToInt32(reader[1]);
                        lista.Add(jurisprudencia);

                    }

                    reader.Close();

                }
                catch (Exception ex)
                {
                    throw ex;

                }
                return lista;
            }
        }
        public List<Jurisprudencia> ViewUpdateJurisprudencia()
        {
            List<Jurisprudencia> lista = new List<Jurisprudencia>();
            Jurisprudencia jurisprudencia;
            using (SqlConnection connection = new SqlConnection(connectionString1))
            {

                string queryString = "select * from etce.v_jurisprudencia_tcese_update";

                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {

                        jurisprudencia = new Jurisprudencia();
                        jurisprudencia.CodProcesso = reader[0].ToString().TrimEnd();
                        jurisprudencia.CodSessao = Convert.ToInt32(reader[1]);
                        string dataPublicacao = Convert.ToString(reader[2]);
                        if (!String.IsNullOrEmpty(dataPublicacao))
                            jurisprudencia.DataPublicacao = Convert.ToDateTime(reader[2]);
                        string dataJulgamento = Convert.ToString(reader[4]);
                        if (!String.IsNullOrEmpty(dataJulgamento))
                            jurisprudencia.DataJulgamento = Convert.ToDateTime(reader[4]);

                        jurisprudencia.Caminho = reader[5].ToString().TrimEnd();

                        lista.Add(jurisprudencia);

                    }

                    reader.Close();

                }
                catch (Exception ex)
                {
                    throw ex;

                }
                return lista;
            }
        }
        public int NovosItensJurisprudencia(List<Jurisprudencia> lista)
        {
            int nrLinhas = 0;
            StringBuilder sentenca_sql = new StringBuilder();
            foreach (Jurisprudencia juris in lista)
            {
                sentenca_sql.Append("INSERT INTO dbo.jurisprudencia");
                sentenca_sql.Append("(COD_PROCESSO");
                sentenca_sql.Append(",RELATOR");
                sentenca_sql.Append(",TIPO_PROCESSO");
                sentenca_sql.Append(",TIPO_DECISAO");
                sentenca_sql.Append(",DT_JULGAMENTO");
                sentenca_sql.Append(",UG");
                sentenca_sql.Append(",CAMINHO");
                sentenca_sql.Append(",CRIPTO");
                sentenca_sql.Append(",CONTEUDO");
                sentenca_sql.Append(",DT_PUBLICACAO");
                sentenca_sql.Append(",ELETRONICO");
                sentenca_sql.Append(",COD_SESSAO)");
                sentenca_sql.Append("VALUES(");

                sentenca_sql.Append("'" + juris.CodProcesso + "', "); // COD_PROCESSO
                sentenca_sql.Append("'" + juris.Relator + "', "); // RELATOR
                sentenca_sql.Append("'" + juris.TipoProcesso + "', "); // TIPO_PROCESSO
                sentenca_sql.Append("'" + juris.TipoDecisao + "', "); // TIPO_DECISAO


                if (juris.DataJulgamento != null)
                    sentenca_sql.Append("'" + Convert.ToString(juris.DataJulgamento.ToString("yyyy-MM-dd hh:mm:ss")) + "', "); // DT_JULGAMENTO
                else
                    sentenca_sql.Append("NULL, "); // DT_JULGAMENTO NULL

                sentenca_sql.Append("'" + juris.UG + "', "); // UG
                sentenca_sql.Append("'" + juris.Caminho + "', "); // CAMINHO
                sentenca_sql.Append("'" + juris.Cripto + "', "); // CRIPTO

                string text = Util.GetContentFilePdfFTP(juris.Caminho);
                //sentenca_sql.Append(text.Substring(0, 5000) + ";");
                sentenca_sql.AppendLine();

                if (!string.IsNullOrEmpty(text))
                {
                    sentenca_sql.Append("'" + text + "', ");
                    sentenca_sql.AppendLine();
                }
                else
                {
                    sentenca_sql.Append("NULL, ");
                }

                if (juris.DataPublicacao != null)
                    sentenca_sql.Append("'" + Convert.ToString(juris.DataPublicacao.ToString("yyyy-MM-dd hh:mm:ss")) + "', "); // DT_PUBLICACAO
                else
                    sentenca_sql.Append("NULL, "); // DT_PUBLICACAO NULL

                sentenca_sql.Append(juris.Eletronico + ", "); // ELETRONICO
                sentenca_sql.Append(juris.CodSessao + ");"); // COD_SESSAO
                sentenca_sql.AppendLine();

            }
            using (SqlConnection connection = new SqlConnection(connectionString2))
            {
                if (sentenca_sql.Length > 0)
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = sentenca_sql.ToString();
                    connection.Open();
                    SqlTransaction transaction;
                    transaction = connection.BeginTransaction("JurisprudenciaTransactionInsert");
                    command.Transaction = transaction;
                    try
                    {
                        nrLinhas = command.ExecuteNonQuery();
                        transaction.Commit();
                        //if (nrLinhas > 0) {

                        //    string pathArquivo = @raizLog + @"\Log_Info.txt";
                        //    StreamWriter writer = new StreamWriter(pathArquivo);
                        //    writer.WriteLine(DateTime.Now.ToString("yyyyMMddHHmmss") + " : " + nrLinhas.ToString() + " linha(s) inserida(s).\n ");
                        //    writer.Close();
                        // }

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ex.Source = "NovosItensJurisprudencia";
                        throw ex;
                        //string pathArquivo = @raizLog + @"\Log_Erro_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                        //StreamWriter writer = new StreamWriter(pathArquivo);
                        //writer.WriteLine(ex.Message.ToString());
                        //writer.WriteLine("\n ------------------------------------------------------------------------- \n");
                        //writer.WriteLine(ex.StackTrace.ToString());
                        //writer.Close();

                    }
                    finally
                    {
                        connection.Close();
                    }

                }
                return nrLinhas;
            }
        }
        public int AtualizaItensJurisprudencia(List<Jurisprudencia> lista)
        {
            int nrLinhas = 0;
            StringBuilder sentenca_sql = new StringBuilder();
            foreach (Jurisprudencia juris in lista)
            {

                sentenca_sql.Append("UPDATE dbo.jurisprudencia ");
                sentenca_sql.Append("SET DT_JULGAMENTO = '" + Convert.ToString(juris.DataJulgamento.ToString("yyyy-MM-dd hh:mm:ss")) + "' ");
                sentenca_sql.Append(",CAMINHO = '" + juris.Caminho + "' ");
                string text = Util.GetContentFilePdfFTP(juris.Caminho);
                sentenca_sql.Append(",CONTEUDO = '" + text + "' ");
                sentenca_sql.Append(",DT_PUBLICACAO = '" + Convert.ToString(juris.DataPublicacao.ToString("yyyy-MM-dd hh:mm:ss")) + "' ");
                sentenca_sql.Append("WHERE COD_PROCESSO = '" + juris.CodProcesso + "' ");
                sentenca_sql.Append("AND COD_SESSAO = '" + juris.CodSessao + "' ");
                sentenca_sql.Append("AND CAST(DT_JULGAMENTO AS DATE) = CAST('" + Convert.ToString(juris.DataJulgamento.ToString("yyyy-MM-dd")) + "' AS DATE);");

                sentenca_sql.AppendLine();

            }
            using (SqlConnection connection = new SqlConnection(connectionString2))
            {
                if (sentenca_sql.Length > 0)
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = sentenca_sql.ToString();
                    connection.Open();
                    SqlTransaction transaction;
                    transaction = connection.BeginTransaction("JurisprudenciaTransactionUpdate");
                    command.Transaction = transaction;
                    try
                    {
                        nrLinhas = command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ex.Source = "AtualizaItensJurisprudencia";
                        throw ex;

                    }
                    finally
                    {
                        connection.Close();
                    }
                }

            }
            return nrLinhas;
        }
 
    }
}
