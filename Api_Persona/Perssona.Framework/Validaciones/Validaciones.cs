using Persona.Framework.Excepciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Persona.Framework.Validaciones
{
    public static class Validaciones
    {
        public static Boolean Validar<TEntity>(TEntity Entidad) where TEntity : class
        {
            DatosInvalidosException datosInvalidos = null;

            foreach (var prop in typeof(TEntity).GetProperties())
            {
                object[] attrs = prop.GetCustomAttributes(true);
                if (attrs == null || attrs.Length == 0)
                {
                    continue;
                }

                foreach (Attribute attr in attrs)
                {
                    if (attr is RequiredAttribute)
                    {
                        if ((attr as RequiredAttribute).IsValid(prop.GetValue(Entidad)) == false)
                        {
                            if (datosInvalidos == null)
                            {
                                datosInvalidos = new DatosInvalidosException();
                            }
                            datosInvalidos.Data.Add(prop.Name, (attr as RequiredAttribute).ErrorMessage ?? String.Format("El campo {0} es obligatorio", prop.Name));
                        }
                    }
                    else if (attr is MinLengthAttribute)
                    {
                        if ((attr as MinLengthAttribute).IsValid(prop.GetValue(Entidad)) == false)
                        {
                            if (datosInvalidos == null)
                            {
                                datosInvalidos = new DatosInvalidosException();
                            }
                            datosInvalidos.Data.Add(prop.Name, (attr as MinLengthAttribute).ErrorMessage ?? String.Format("El campo {0} debe tener una longitud mínima de '{1}'.", prop.Name, (attr as MinLengthAttribute).Length));
                        }
                    }
                    else if (attr is MaxLengthAttribute)
                    {
                        if ((attr as MaxLengthAttribute).IsValid(prop.GetValue(Entidad)) == false)
                        {
                            if (datosInvalidos == null)
                            {
                                datosInvalidos = new DatosInvalidosException();
                            }
                            datosInvalidos.Data.Add(prop.Name, (attr as MaxLengthAttribute).ErrorMessage ?? String.Format("El campo {0} debe tener una longitud máxima de '{1}'.", prop.Name, (attr as MaxLengthAttribute).Length));
                        }
                    }
                    else if (attr is RangeAttribute)
                    {
                        if ((attr as RangeAttribute).IsValid(prop.GetValue(Entidad)) == false)
                        {
                            if (datosInvalidos == null)
                            {
                                datosInvalidos = new DatosInvalidosException();
                            }
                            datosInvalidos.Data.Add(prop.Name, (attr as RangeAttribute).ErrorMessage ?? String.Format("El campo {0} debe estar en el rango {1} - {2}.", prop.Name, (attr as RangeAttribute).Minimum, (attr as RangeAttribute).Maximum));
                        }
                    }
                }
            }

            if (datosInvalidos == null)
            {
                return true;
            }
            else
            {
                throw datosInvalidos;
            }
        }
    }
}
