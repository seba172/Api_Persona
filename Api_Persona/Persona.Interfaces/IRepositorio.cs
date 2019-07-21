using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persona.Interfaces
{
    public interface IRepositorio<TEntity> where TEntity : class
    {
        Task<TEntity> InsertarAsync(TEntity Entidad);
        TEntity Insertar(TEntity Entidad);
        Task ActualizarAsync(TEntity Entidad);
        void Actualizar(TEntity Entidad);
        Task EliminarAsync(TEntity Entidad);
        Task<TEntity> ObtenerPorIDAsync(Int32 ID, params Expression<Func<TEntity, Object>>[] Includes);
        Task<TEntity> ObtenerUnoAsync(Expression<Func<TEntity, Boolean>> Filtro, params Expression<Func<TEntity, Object>>[] Includes);
        Task<List<TEntity>> ObtenerListadoAsync(params Expression<Func<TEntity, Object>>[] Includes);
        Task<List<TEntity>> ObtenerListadoAsync(Expression<Func<TEntity, bool>> Filtro, params Expression<Func<TEntity, Object>>[] Includes);  
    }
}
