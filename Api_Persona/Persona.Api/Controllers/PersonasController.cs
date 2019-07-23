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

        /// <summary>
        /// Devuelve un listado de personas.
        /// </summary>
        /// <returns>Listado de persona</returns>
        /// <response code="200">Retorna un listado de persona</response>
        // GET: api/<controller>
        [HttpGet]
        [Produces(typeof(List<dtoPersona>))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
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

        /// <summary>
        /// Devuelve una Persona.
        /// </summary>
        /// <returns>Una persona</returns>
        /// <param name="id">Id de la persona a buscar</param>
        /// <response code="200">Retorna una persona</response>
        // GET api/<controller>/5
        [HttpGet("{id}")]        
        [Produces(typeof(dtoPersona))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                dtoPersona persona = await PersonaDominio.ObtenerPersonaAsync(id);
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

        /// <summary>
        /// Crear una Persona.
        /// </summary>
        /// <returns>Una nueva persona creada</returns>
        /// <param name="persona">Entidad persona a crear</param>
        /// <response code="201">Retorna la nueva persona creada</response>
        // POST api/<controller>
        [HttpPost]       
        [Produces(typeof(dtoPersona))]        
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Post([FromBody]Entidades.Persona persona)
        {
            try
            {
                if (persona == null)
                {
                    return BadRequest();
                }

                dtoPersona personaGuardada = await PersonaDominio.GuardarPersonaAsync(persona);
                return StatusCode((int)HttpStatusCode.Created, personaGuardada);
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

        /// <summary>
        /// Actualiza una Persona.
        /// </summary>
        /// <returns>Persona actualizar</returns>
        /// <param name="id">Id persona a actualizar</param>
        /// <param name="persona">Entidad persona a actualizar</param>
        /// <response code="200">Retorna la persona actualizada</response>
        // PUT api/<controller>/5
        [HttpPut("{id}")]      
        [Produces(typeof(dtoPersona))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Put(int id, [FromBody]Entidades.Persona persona)
        {
            try
            {
                if (persona == null)
                {
                    return BadRequest();
                }

                if (await PersonaDominio.ObtenerPersonaAsync(id) == null)
                {
                    return NotFound();
                }

                persona.Id = id;
                return Ok(await PersonaDominio.GuardarPersonaAsync(persona));
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(Errores.GetModelStateErrores(ex.Data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Elimna una Persona.
        /// </summary>
        /// <returns></returns>
        /// <param name="id">Id persona a eliminar</param>
        /// <response code="200"></response>
        // DELETE api/<controller>/5
        [HttpDelete("{id}")]     
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (await PersonaDominio.ObtenerPersonaAsync(id) == null)
                {
                    return NotFound();
                }

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

        /// <summary>
        /// Crea relacion de padre a hij@
        /// </summary>
        /// <returns>Una nueva relacion creada</returns>
        /// <param name="idPersona1">Id persona padre</param>
        /// <param name="idPersona2">Id persona hij@</param>
        /// <response code="201">Retorna la relacion creada</response>
        // POST api/<controller>/idPersona1/padre/idPersona2
        [HttpPost]      
        [Produces(typeof(dtoPersonaRelacion))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("{idPersona1:int}/padre/{idPersona2:int}")]
        public async Task<IActionResult> PostRelacion([FromRoute]int idPersona1, [FromRoute]int idPersona2)
        {
            try
            {
                if (await PersonaDominio.ObtenerPersonaAsync(idPersona1) == null || await PersonaDominio.ObtenerPersonaAsync(idPersona2) == null)
                {
                    return NotFound();
                }

                dtoPersonaRelacion relacionGuardada = await PersonaDominio.GuardarRelacionPadreAsync(idPersona1, idPersona2);
                return StatusCode((int)HttpStatusCode.Created, relacionGuardada);
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

        /// <summary>
        /// Devuelve relacion entre dos personas.
        /// </summary>
        /// <returns>relacion</returns>
        /// <param name="idPersona1">Id persona 1</param>
        /// <param name="idPersona2">Id persona 2</param>
        /// <response code="200">Retorna la relacion entre dos personas</response>
        // GET: api/<controller>/relaciones/1/2
        [HttpGet]      
        [Produces(typeof(dtoTipoRelacion))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("relaciones/{idPersona1}/{idPersona2}")]
        public async Task<IActionResult> GetRelaciones(int idPersona1, int idPersona2)
        {
            try
            {
                if (await PersonaDominio.ObtenerPersonaAsync(idPersona1) == null || await PersonaDominio.ObtenerPersonaAsync(idPersona2) == null)
                {
                    return NotFound();
                }

                dtoTipoRelacion dtoTipoRelacion = await PersonaDominio.ObtenerRelacionAsync(idPersona1, idPersona2);

                if (dtoTipoRelacion == null)
                {
                    return NotFound();
                }

                return Ok(dtoTipoRelacion);
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
