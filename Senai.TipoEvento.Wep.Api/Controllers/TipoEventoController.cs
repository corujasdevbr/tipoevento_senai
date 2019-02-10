using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Senai.TipoEvento.Wep.Api.Dominio;
using System.IO;

namespace Senai.TipoEvento.Wep.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoEventoController : ControllerBase
    {
        [HttpGet]
        public  IActionResult ListarTodos()
        {
            List<TipoEventoDominio> lsTipoEventos = new List<TipoEventoDominio>();

            string[] linhas = System.IO.File.ReadAllLines("tipoeventos.csv");

            TipoEventoDominio usuario;

            foreach (var item in linhas)
            {

                //Verifica se a linha é vazia
                if (string.IsNullOrEmpty(item))
                {
                    //Retorna para o foreach
                    continue;
                }

                string[] linha = item.Split(';');

                usuario = new TipoEventoDominio(
                                            id: int.Parse(linha[0]),
                                            nome: linha[1]
                                        );

                lsTipoEventos.Add(usuario);
            }

            return Ok(lsTipoEventos);
        }


        [HttpGet("{id}")]
        public IActionResult BuscarPorId(int id)
        {
            string[] linhas = System.IO.File.ReadAllLines("tipoeventos.csv");

            foreach (var item in linhas)
            {
                string[] dados = item.Split(';');

                if (id.ToString() == dados[0])
                {
                    TipoEventoDominio tipoEvento = new TipoEventoDominio(
                                            id: int.Parse(dados[0]),
                                            nome: dados[1]
                                        );

                    return Ok(tipoEvento);
                }
            }

            return NotFound();
        }


        [HttpPost]
        public IActionResult Post([FromBody] TipoEventoDominio tipoEvento)
        {
            //Verifica se o arquivo existe
            if (System.IO.File.Exists("tipoeventos.csv"))
            {
                //Se arquivo existe Pega a quantidade de linhas e incrementa 1
                tipoEvento.Id = System.IO.File.ReadAllLines("tipoeventos.csv").Length + 1;
            }
            else
            {
                //caso não exista seta como 1
                tipoEvento.Id = 1;
            }

            //Grava as informações no arquivo usuarios.csv
            using (StreamWriter sw = new StreamWriter("tipoeventos.csv", true))
            {
                sw.WriteLine($"{tipoEvento.Id};{tipoEvento.Nome}");
            }

            return Ok(tipoEvento);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] TipoEventoDominio tipoEvento)
        {
            string[] linhas = System.IO.File.ReadAllLines("tipoeventos.csv");

            for (int i = 0; i < linhas.Length; i++)
            {
                if (string.IsNullOrEmpty(linhas[i]))
                {
                    continue;
                }

                string[] dados = linhas[i].Split(';');

                //Verifica se o id do formulário é igual ao da linha
                if (tipoEvento.Id.ToString() == dados[0])
                {
                    //Altera os dados da linha
                    linhas[i] = $"{tipoEvento.Id};{tipoEvento.Nome}";
                    break;
                }
            }

            //Altera o arquivo usuarios.csv
            System.IO.File.WriteAllLines("tipoeventos.csv", linhas);

            return Ok(tipoEvento);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //Pega os dados do arquivo usuario.csv
            string[] linhas = System.IO.File.ReadAllLines("tipoeventos.csv");

            //Percorre as linhas do arquivo
            for (int i = 0; i < linhas.Length; i++)
            {
                //Separa as colunas da linha
                string[] linha = linhas[i].Split(';');

                //Verifica se o id da linha é o id passado
                if (id.ToString() == linha[0])
                {
                    //Defino a linha como vazia
                    linhas[i] = "";
                    break;
                }
            }

            //Armazeno no arquivo csv todas as linhas
            System.IO.File.WriteAllLines("tipoeventos.csv", linhas);

            return Ok();
        }
    }
}