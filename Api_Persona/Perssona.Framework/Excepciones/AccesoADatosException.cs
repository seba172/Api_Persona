using System;
using System.Collections.Generic;
using System.Text;

namespace Persona.Framework.Excepciones
{
    public class AccesoADatosException : ApplicationException
    {
        public AccesoADatosException(Exception ex)
            : base(ex.Message, ex)
        {
        }
    }
}