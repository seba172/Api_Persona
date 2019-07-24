using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Persona.Entidades.Dtos
{
    public class DtoEstadisticas
    {
        public int CantidadMujeres { get; set; }
        public int CantidadHombres { get; set; }
        public double PorcentajeArgentinos { get; set; }
    }
}
