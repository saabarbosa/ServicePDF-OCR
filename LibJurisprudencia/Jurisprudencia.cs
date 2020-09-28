using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibJurisprudencia
{
    public class Jurisprudencia
    {
        public string CodProcesso { get; set; }
        public string Relator { get; set; }
        public string TipoProcesso { get; set; }
        public string TipoDecisao { get; set; }
        public DateTime DataJulgamento { get; set; }
        public string UG { get; set; }
        public string Caminho { get; set; }
        public string Cripto { get; set; }
        public string Conteudo { get; set; }
        public DateTime DataPublicacao { get; set; }
        public int Eletronico { get; set; }
        public int CodSessao { get; set; }
    }
}
