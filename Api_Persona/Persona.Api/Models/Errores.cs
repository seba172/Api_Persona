using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persona.Api.Models
{
    public class Errores
    {
        public static ModelStateDictionary GetModelStateErrores(IDictionary LstErrores)
        {
            ModelStateDictionary _modelState = new ModelStateDictionary();
            foreach (DictionaryEntry error in LstErrores)
            {
                _modelState.AddModelError(error.Key.ToString(), error.Value.ToString());
            }

            return _modelState;
        }

        public static ModelStateDictionary AddErrorToModelState(string code, string description, ModelStateDictionary modelState)
        {
            modelState.TryAddModelError(code, description);
            return modelState;
        }
    }
}
