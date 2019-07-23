﻿using System;
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
        /// <summary>
        /// Devuelve un listado de personas.
        /// </summary>
        /// <returns>Listado de persona</returns>
        /// <response code="200">Retorna un listado de persona</response>
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

        // GET api/<controller>/5
        [HttpGet("{id}")]
        /// <summary>
        /// Devuelve una Persona.
        /// </summary>
        /// <returns>Una persona</returns>
        /// <response code="200">Retorna una persona</response>
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

        // POST api/<controller>
        [HttpPost]
        /// <summary>
        /// Crear una Persona.
        /// </summary>
        /// <returns>Una nueva persona creada</returns>
        /// <response code="201">Retorna la nueva persona creada</response>
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

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        /// <summary>
        /// Actualiza una Persona.
        /// </summary>
        /// <returns>Persona actualizar</returns>
        /// <response code="200">Retorna la persona actualizada</response>
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

        // POST api/<controller>/idPersona1/padre/idPersona2
        [HttpPost]
        /// <summary>
        /// Crea relacion de padre.
        /// </summary>
        /// <returns>Una nueva relacion creada</returns>
        /// <response code="201">Retorna la relacion creada</response>
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

        // GET: api/<controller>/relaciones/1/2
        [HttpGet]
        /// <summary>
        /// Devuelve relacion entre dos personas.
        /// </summary>
        /// <returns>relacion</returns>
        /// <response code="200">Retorna la relacion entre dos personas</response>
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
