using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibJurisprudencia;

namespace InputBaseJurisprudencia
{
    class Program
    {
        static void Main(string[] args)
        {

            int inicial = 1;
            //int total = 7125;
            int total = 10;
            bool ftp = false;

            if (args.Length > 0)
            {
                inicial = Convert.ToInt32(args[0]);
                total = Convert.ToInt32(args[1]);
                string flagFTP = args[2];
                if (!String.IsNullOrEmpty(flagFTP))
                    ftp = true;
            }
            DAL dal = new DAL();
            List<Jurisprudencia> juris = dal.ViewInsertJurisprudencia(); 
            int itensNovos = dal.NovosItensJurisprudencia(juris);
            juris = dal.ViewUpdateJurisprudencia();
            int itensAtualizados = dal.AtualizaItensJurisprudencia(juris);
            //List<Jurisprudencia> juris = dal.ViewJurisprudencia(inicial, total);
            //Util.MontarScriptINSERT_SQL(juris, ftp, inicial, total);

        }
    }
}
