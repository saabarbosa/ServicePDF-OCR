using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using LibJurisprudencia;

namespace ServiceJurisprudencia
{
    public partial class i9Service : ServiceBase
    {
        Timer timer;
        string logFile;
        string logFileErro;
        string logPath;
        string startTime;

        public i9Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            try
            {
                logFile     = ConfigurationManager.AppSettings["LogFile"].ToString();
                logFileErro = ConfigurationManager.AppSettings["LogFileErro"].ToString();
                logPath     = ConfigurationManager.AppSettings["LogPath"].ToString();
                startTime   = ConfigurationManager.AppSettings["StartTime"].ToString();
                //verifica se existe o diretorio
                if (System.IO.Directory.Exists(logPath) == false)
                    System.IO.Directory.CreateDirectory(logPath);

                    
                timer = new Timer(new TimerCallback(AtualizarBaseJurisprudencia), null, 1000, 600000);

                GerarLog("Servico iniciado: ");

            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Erro ao iniciar o serviço", ex.Message + Environment.NewLine + ex.StackTrace);
            }


        }

        protected override void OnStop()
        {
            try
            {
                GerarLog("Servico parado: ");
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Erro ao parar o serviço", ex.Message + Environment.NewLine + ex.StackTrace);

            }
        }

        private void AtualizarBaseJurisprudencia(object sender)
        {

            try
            {
                string startTime = ConfigurationManager.AppSettings["StartTime"].ToString();
                //if ((Convert.ToInt32(startTime) == DateTime.Now.Hour) && (DateTime.Now.Minute == 0))
                if (Convert.ToInt32(startTime) == DateTime.Now.Hour)
                {
                    DAL dal = new DAL();

                    List<Jurisprudencia> juris = dal.ViewInsertJurisprudencia();
                    int itensNovos = dal.NovosItensJurisprudencia(juris);
                    juris = dal.ViewUpdateJurisprudencia();
                    int itensAtualizados = dal.AtualizaItensJurisprudencia(juris);
                    GerarLog("Jurisprudencia Service: Serviço de Atualização de Base de dados da Jurisprudencia. [Novos = " + itensNovos + " - Atualizados: " + itensAtualizados + "] ");

                }
     
            }
            catch (Exception ex)
            {

                GerarLog(ex);
                System.Diagnostics.Debug.Print(ex.Message);
            }
            finally
            {

            }
        }

        private void i9eventLog_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }

        private void GerarLog(string msg)
        {
            using (StreamWriter writer = new StreamWriter(@logPath + logFile, true))
            {
                writer.WriteLine(msg + DateTime.Now.ToString());
                writer.Close();
            }
        }

        private void GerarLog(Exception e)
        {
            using (StreamWriter writer = new StreamWriter(@logPath + logFileErro, true))
            {
                writer.WriteLine("************************************************************************ \n");
                writer.WriteLine(DateTime.Now.ToString());
                writer.WriteLine(e.Source.ToString());
                writer.WriteLine(e.Message.ToString());
                writer.WriteLine(e.StackTrace.ToString());
                writer.Close();

            }
        }

    }
}
