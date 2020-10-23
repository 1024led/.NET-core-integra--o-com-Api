﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cotacao.Models.Services
{
    public class RequisicaoService
    {
        public string Dia { get; set; }
        public string Mes { get; set; }
        public string Ano { get; set; }

        public string moeda { get; set; }

        private string hold1 => "https://olinda.bcb.gov.br/olinda/servico/PTAX/versao/v1/odata/CotacaoMoedaDia(moeda=@moeda,dataCotacao=@dataCotacao)?@moeda=%27";
        private string hold2 => "%27&@dataCotacao=%27";
        private string hold3 => "%27&$top=100&$format=json&$select=cotacaoCompra,cotacaoVenda,dataHoraCotacao";

        private string url
        {
            get
            {
                return hold1 + moeda + hold2 + Mes + "-" + Dia + "-" + Ano + hold3;

            }
        }


        public string ObterUrl()
        {
            return url;
        }

        public void VerificaFimSemana(Models.Cotacao temp)
        {
            if (temp.Data.DayOfWeek.ToString() == "Sunday")
            {
                

               
                temp.Data = temp.Data.AddDays(1);
                Dia = Convert.ToString(temp.Data.Day, 10);
                Mes = Convert.ToString(temp.Data.Month, 10);
                Ano =  Convert.ToString(temp.Data.Year, 10);
               
                //AtualizarUrl();
            }

            if (temp.Data.DayOfWeek.ToString() == "Saturday")
            {
                
                temp.Data = temp.Data.AddDays(2);
                Dia = Convert.ToString(temp.Data.Day, 10);
                Mes = Convert.ToString(temp.Data.Month, 10);
                Ano = Convert.ToString(temp.Data.Year, 10);
                
                //AtualizarUrl();
            }


        }

    }
}
