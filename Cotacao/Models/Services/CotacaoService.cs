using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;
using Newtonsoft.Json;

namespace Cotacao.Models
{
    public class root
    {
        [JsonProperty("value")]
        public List<CotacaoService> Results { get; set; }
    }
    public class CotacaoService
    {
        //[{"cotacaoCompra":5.62200,"cotacaoVenda":5.62260,"dataHoraCotacao":"2020-10-16 13:04:22.018"}]
        //[Key]
        //public int id { get; set; }

        [JsonProperty("cotacaoCompra")]
        public string cotacaoCompra { get; set; }

        [JsonProperty("cotacaoVenda")]
        public string cotacaoVenda { get; set; }

        [JsonProperty("dataHoraCotacao")]
        public string dataHoraCotacao { get; set; }


        
        public static async Task<CotacaoService> Coletar(Models.Services.RequisicaoService requisicao)
        {
            CotacaoService coletor = new CotacaoService();
            string errorString;

            string hold1 = "https://olinda.bcb.gov.br/olinda/servico/PTAX/versao/v1/odata/CotacaoMoedaDia(moeda=@moeda,dataCotacao=@dataCotacao)?@moeda=%27";
            string hold2 = "%27&@dataCotacao=%27";
            string hold3 = "%27&$top=100&$format=json&$select=cotacaoCompra,cotacaoVenda,dataHoraCotacao";

            string url = hold1 + requisicao.moeda + hold2 +  requisicao.Mes + "-" + requisicao.Dia + "-" + requisicao.ano + hold3;

            using (HttpClient client = new HttpClient())
            {
                var content = new HttpRequestMessage(HttpMethod.Get, url);

                HttpResponseMessage response = await client.SendAsync(content);

                Console.WriteLine(url);



                if (response.IsSuccessStatusCode)
                {
                    await response.Content.ReadFromJsonAsync<CotacaoService>();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    root raiz = JsonConvert.DeserializeObject<root>(responseBody);
                    
                    errorString = null;

                    if (raiz.Results.Count > 0)
                    {
                        int index = raiz.Results.Count - 1;
                        coletor.cotacaoCompra = raiz.Results[index].cotacaoCompra;
                        coletor.cotacaoVenda = raiz.Results[index].cotacaoVenda;
                        coletor.dataHoraCotacao = raiz.Results[index].dataHoraCotacao;

                    }



                    Console.WriteLine(responseBody);
                    Console.WriteLine(coletor.cotacaoCompra);
                }
                else
                {
                    Console.WriteLine("FRACASSO");
                    errorString = $"algo deu errado: {response.ReasonPhrase}";
                }
            }

            return coletor;
        }



    }


}
