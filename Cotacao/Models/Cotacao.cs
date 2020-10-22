using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cotacao.Models
{
    public class Cotacao
    {
        [Key]
        public int CotacaoId { get; set; }

        //[Column("varchar(20)")]
        public string MoedaOrigem { get; set; }

        //[Column("varchar(20)")]
        public string MoedaDestino { get; set; }

        //[Column("varchar(6)")]
        public string ValorCompra { get; set; }

        //[Column("varchar(6)")]
        public string ValorVenda { get; set; }

        
        [Required]
        ////[Column("nvarchar(2)")]
        public int DataDia { get; set; }

        [Required]
        ////[Column("nvarchar(2)")]
        public int DataMes { get; set; }
        
        [Required]
        ////[Column("nvarchar(4)")]
        public int DataAno { get; set; }

        public DateTime Data { get; set; }

        public string DataStr { get; set; }
        

    }
}
