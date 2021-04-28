using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DioApiMongo.Data.Collections;
using DioApiMongo.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Annotations;

namespace DioApiMongo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectadoController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Infectado> _infectadosCollection;

        public InfectadoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }
        /// <summary>
        /// Permite cadastrar um novo infectado
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [SwaggerResponse(statusCode: 201, description: "Sucesso ao cadastrar um novo infectado", Type = typeof(InfectadoViewModel))]
        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] InfectadoViewModel viewModel)
        {
            var infectado = new Infectado(viewModel.DataNascimento, viewModel.Sexo, viewModel.Latitude, viewModel.Longitude);
            _infectadosCollection.InsertOne(infectado);
            return Created("Infectado adicionado com sucesso",viewModel);
        }

        /// <summary>
        /// Retorna uma lista com todos os infectados
        /// </summary>
        /// <returns>Lista de Infectados</returns>

        [SwaggerResponse(statusCode: 200, description: "Sucesso ao obter os infectados")]
        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();

            return Ok(infectados);
        }

        /// <summary>
        /// Permite atualizar um infectado
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao atualizar um infectado")]
        [HttpPut]
        public ActionResult AtualizarInfectado([FromBody] InfectadoViewModel viewModel, string Id)
        {
            // Atualizar apenas um campo
            
            _infectadosCollection.UpdateOne(Builders<Infectado>.Filter.Where(r => r.Id == Id),
                Builders<Infectado>.Update.Set("sexo",viewModel.Sexo));
            

            return Ok("Infectado atualizado com sucesso");
        }


        /// <summary>
        /// Permite excluir um infectado
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao excluir um infectado")]
        [HttpDelete]
        public ActionResult DeletarInfectado(string Id)
        {
            _infectadosCollection.DeleteOne(Builders<Infectado>.Filter.Where(r => r.Id == Id));
            return Ok("Infectado excluído com sucesso");
        }


    }
}
