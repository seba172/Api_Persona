using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper Mapper;

        public PersonasController(IPersonaDominio _personaDominio, IMapper _mapper)
        {
            Mapper = _mapper;
            PersonaDominio = _personaDominio;
        }

        /// <summary>
        /// Devuelve un listado de personas.
        /// </summary>
        /// <returns>Listado de persona</returns>
        /// <response code="200">Retorna un listado de persona</response>
        // GET: api/<controller>
        [HttpGet]
        [Produces(typeof(List<DtoPersona>))]
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
        [HttpGet("{id}", Name = "PersonaById")]
        [Produces(typeof(DtoPersona))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                DtoPersona persona = await PersonaDominio.ObtenerPersonaAsync(id);
                if (persona == null)
                    return NotFound();

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
        [Produces(typeof(DtoPersona))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Post([FromBody]PersonaViewModelNueva personaViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (personaViewModel == null)
                    return BadRequest();

                DtoPersona personaGuardada = await PersonaDominio.InsertarPersonaAsync(Mapper.Map<Entidades.Persona>(personaViewModel));
                return CreatedAtRoute("PersonaById", new { id = personaGuardada.Id }, personaGuardada);
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
        [Produces(typeof(DtoPersona))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Put(int id, [FromBody]PersonaViewModelActualizar personaViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (personaViewModel == null)
                    return BadRequest();

                if (await PersonaDominio.ObtenerPersonaAsync(id) == null)
                    return NotFound();

                return Ok(await PersonaDominio.ActualizarPersonaAsync(id, Mapper.Map<Entidades.Persona>(personaViewModel)));
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
        /// Elimna una Persona.
        /// </summary>
        /// <returns></returns>
        /// <param name="id">Id persona a eliminar</param>
        /// <response code="200"></response>
        // DELETE api/<controller>/5
        [HttpDelete("{id}")]     
        [ProducesResponseType(204)]
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
                return NoContent();
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
        [HttpPost("{idPersona1:int}/padre/{idPersona2:int}")]      
        [Produces(typeof(DtoPersonaRelacion))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostRelacion([FromRoute]int idPersona1, [FromRoute]int idPersona2)
        {
            try
            {
                if (await PersonaDominio.ObtenerPersonaAsync(idPersona1) == null || await PersonaDominio.ObtenerPersonaAsync(idPersona2) == null)
                    return NotFound();

                DtoPersonaRelacion relacionGuardada = await PersonaDominio.GuardarRelacionPadreAsync(idPersona1, idPersona2);
                return CreatedAtRoute("GetRelaciones", null, relacionGuardada);
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
        [HttpGet("relaciones/{idPersona1:int}/{idPersona2:int}", Name = "GetRelaciones")]      
        [Produces(typeof(DtoTipoRelacion))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRelaciones(int idPersona1, int idPersona2)
        {
            try
            {
                if (await PersonaDominio.ObtenerPersonaAsync(idPersona1) == null || await PersonaDominio.ObtenerPersonaAsync(idPersona2) == null)
                    return NotFound();

                DtoTipoRelacion dtoTipoRelacion = await PersonaDominio.ObtenerRelacionAsync(idPersona1, idPersona2);

                if (dtoTipoRelacion == null)
                    return NotFound();

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
