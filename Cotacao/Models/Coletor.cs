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
        public List<Coletor> Results { get; set; }
    }
    public class Coletor
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


        //Coletor holder;

        //public static Coletor Response(int dia, int mes, int ano)
        //{

        //Coletor temp = new Coletor();

        // string url = "";

        //temp = Coletar(url, temp).Result;

        //return temp;

        //}

        public static async Task<Coletor> Coletar(string url)
        {
            Coletor coletor = new Coletor();
            string errorString;

            using (HttpClient client = new HttpClient())
            {
                var content = new HttpRequestMessage(HttpMethod.Get, url);

                //var client = _clientFactory.CreateClient();

                HttpResponseMessage response = await client.SendAsync(content);

                //var response = await client.PostAsync(url, content);

                Console.WriteLine(url);



                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("SUCESSO");
                    await response.Content.ReadFromJsonAsync<Coletor>();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    root raiz = JsonConvert.DeserializeObject<root>(responseBody);
                    //List<root> myDeserializedObjList = (List<root>)Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody), typeof(List<root>));
                    errorString = null;
                    coletor.cotacaoCompra = raiz.Results[0].cotacaoCompra;
                    coletor.cotacaoVenda = raiz.Results[0].cotacaoVenda;
                    coletor.dataHoraCotacao = raiz.Results[0].dataHoraCotacao;


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
