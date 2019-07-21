using System;
using System.Collections.Generic;
using System.Text;

namespace Persona.Framework.Excepciones
{
    public class DatosInvalidosException : ApplicationException
    {
        public DatosInvalidosException()
            : base()
        {
        }

        public DatosInvalidosException(string Mensaje) : base(Mensaje)
        {
        }
    }
}
