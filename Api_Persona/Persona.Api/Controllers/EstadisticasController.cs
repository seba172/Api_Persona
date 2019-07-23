using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Persona.Api.Models;
using Persona.Entidades.Dtos;
using Persona.Framework.Excepciones;
using Persona.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Persona.Api.Controllers
{
    [Route("api/[controller]")]
    public class EstadisticasController : Controller
    {
        private readonly IPersonaDominio PersonaDominio;

        public EstadisticasController(IPersonaDominio _personaDominio)
        {
            PersonaDominio = _personaDominio;
        }

        // GET: api/<controller>
        [HttpGet]
        /// <summary>
        /// Devuelve estadisticas de personas.
        /// </summary>
        /// <returns>estadisticas</returns>
        /// <response code="200">Retorna las estadisticas de personas</response>
        [Produces(typeof(dtoEstadisticas))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEstadisticas()
        {
            try
            {
                return Ok(await PersonaDominio.ObtenerEstadisticasAsync());
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(Errores.GetModelStateErrores(ex.Data));
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
