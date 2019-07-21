using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class PersonasController : Controller
    {
        private readonly IPersonaDominio PersonaDominio;

        public PersonasController(IPersonaDominio _personaDominio)
        {
            PersonaDominio = _personaDominio;
        }

        // GET: api/<controller>
        [HttpGet]
        [Produces(typeof(List<dtoPersona>))]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await PersonaDominio.ObtenerListadoPersonasAsync());
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

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Produces(typeof(dtoPersona))]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                var persona = await PersonaDominio.ObtenerPersonaAsync(id);
                if (persona == null)
                {
                    return NotFound();
                }

                return Ok(persona);
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

        // POST api/<controller>
        [HttpPost]
        /// <summary>
        /// Crear una Persona.
        /// </summary>
        /// <returns>Una nueva persona creada</returns>
        /// <response code="201">Retorna la nueva persona creada</response>
        public async Task<IActionResult> Post([FromBody]dtoPersona dtoPersona)
        {
            try
            {
                return StatusCode((int)HttpStatusCode.Created, await PersonaDominio.GuardarPersonaAsync(dtoPersona));
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

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]string value)
        {
            return Ok();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await PersonaDominio.EliminarPersonaAsync(id);

                return Ok();
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
