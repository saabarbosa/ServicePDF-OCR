using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Net;
using System.Configuration;

namespace LibJurisprudencia
{
    public class Util
    {
        // método nao sera utilizado
        public static string GetContentFilePdf(string fileName)
        {
            //string raiz = @"G:\FTP\se";
            string raiz = @ConfigurationManager.AppSettings["PathFiles"].ToString();
            StringBuilder text = new StringBuilder();
            if (!File.Exists(raiz + fileName))
                throw new FileNotFoundException("Arquivo não encontrado", raiz + fileName);

            using (PdfReader reader = new PdfReader(raiz + fileName))
            {

                var strategy = new SimpleTextExtractionStrategy();

                for (int page = 1; page <= reader.NumberOfPages; page++)
                {
                    var currentPageText = PdfTextExtractor.GetTextFromPage(reader, page, strategy);
                    text.Append(RemoverAcentos(currentPageText));
                }
            }

            return text.ToString();
        }

        // metodo para obter o conteudo do arquivo pdf com base num path local
        public static string GetContentFileLocal(string fileName)
        {
            StringBuilder text = new StringBuilder();
            if (!File.Exists(fileName))
                throw new FileNotFoundException("Arquivo não encontrado", fileName);

            using (PdfReader reader = new PdfReader(fileName))
            {

                var strategy = new SimpleTextExtractionStrategy();

                for (int page = 1; page <= reader.NumberOfPages; page++)
                {
                    var currentPageText = PdfTextExtractor.GetTextFromPage(reader, page, strategy);
                    text.Append(RemoverAcentos(currentPageText));
                }
            }

            return text.ToString();
        }

        // metodo para remover acentos e aspas simples/dupla
        public static string RemoverAcentos(string texto)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());

            }
            return texto.Replace("'", "").Replace("\"", "").Replace("“", "").Replace("”", "").Replace("_", "").ToLower();
        }

        // metodo para obter o conteudo do arquivo pdf com base num path ftp
        public static string GetContentFilePdfFTP(string fileName)
        {
            StringBuilder text = new StringBuilder();
            try
            {
                using (WebClient request = new WebClient())
                {
                    request.Credentials = new NetworkCredential("tce\\usr_sharepoint", "@(tce)");
                    byte[] fileData = request.DownloadData("ftp://10.140.100.55/se" + fileName);
                    using (PdfReader reader = new PdfReader(fileData))
                    {

                        var strategy = new SimpleTextExtractionStrategy();

                        for (int page = 1; page <= reader.NumberOfPages; page++)
                        {
                            var currentPageText = PdfTextExtractor.GetTextFromPage(reader, page, strategy);
                            text.Append(RemoverAcentos(currentPageText));
                        }
                    }
                }

            }
            catch (WebException ex)
            {

            }
            int maxPermitidoString = 65535;
            int conteudoPDFLimite = (text.Length > maxPermitidoString) ? maxPermitidoString : text.Length;
            return text.ToString(0, conteudoPDFLimite);
        }

        public static void LerArquivoFTP(string url, string local, string usuario, string senha)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(url));
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(usuario, senha);
                request.UseBinary = true;
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    using (Stream rs = response.GetResponseStream())
                    {
                        using (FileStream ws = new FileStream(local, FileMode.Create))
                        {
                            byte[] buffer = new byte[2048];
                            int bytesRead = rs.Read(buffer, 0, buffer.Length);
                            while (bytesRead > 0)
                            {
                                ws.Write(buffer, 0, bytesRead);
                                bytesRead = rs.Read(buffer, 0, buffer.Length);
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }


    }
}
