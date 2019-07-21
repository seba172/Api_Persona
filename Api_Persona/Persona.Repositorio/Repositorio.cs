using Microsoft.EntityFrameworkCore;
using Persona.Interfaces;
using Persona.Framework.Excepciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persona.Repositorio
{
    public class Repositorio<TEntity> : IRepositorio<TEntity> where TEntity : class
    {
        protected PersonaContext Context;

        public Repositorio(PersonaContext _context)
        {
            Context = _context;
        }

        #region Insertar
        public virtual async Task<TEntity> InsertarAsync(TEntity Entidad)
        {
            try
            {
                Context.Set<TEntity>().Add(Entidad);
                await Context.SaveChangesAsync();

                return Entidad;
            }
            catch (Exception ex)
            {
                throw new AccesoADatosException(ex);
            }
        }

        public virtual TEntity Insertar(TEntity Entidad)
        {
            try
            {
                Context.Set<TEntity>().Add(Entidad);
                Context.SaveChanges();

                return Entidad;
            }
            catch (Exception ex)
            {
                throw new AccesoADatosException(ex);
            }
        }
        #endregion

        #region Actualizar
        public virtual async Task ActualizarAsync(TEntity Entidad)
        {
            try
            {
                Context.Entry<TEntity>(Entidad).State = EntityState.Modified;
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new AccesoADatosException(ex);
            }
        }

        public virtual void Actualizar(TEntity Entidad)
        {
            try
            {
                Context.Entry<TEntity>(Entidad).State = EntityState.Modified;
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new AccesoADatosException(ex);
            }
        }
        #endregion

        #region Eliminar
        public virtual async Task EliminarAsync(TEntity Entidad)
        {
            try
            {
                Context.Entry<TEntity>(Entidad).State = EntityState.Deleted;
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new AccesoADatosException(ex);
            }
        }
        #endregion

        #region ObtenerByID
        public virtual async Task<TEntity> ObtenerPorIDAsync(Int32 ID, params Expression<Func<TEntity, Object>>[] Includes)
        {
            try
            {
                DbSet<TEntity> _dbSet = Context.Set<TEntity>();

                if (Includes != null)
                {
                    foreach (Expression<Func<TEntity, Object>> include in Includes)
                    {
                        _dbSet = _dbSet.Include(include) as DbSet<TEntity>;
                    }
                }

                return await _dbSet.FindAsync(ID);
            }
            catch (Exception ex)
            {
                throw new AccesoADatosException(ex);
            }
        }
        #endregion

        #region ObtenerUno
        public virtual async Task<TEntity> ObtenerUnoAsync(Expression<Func<TEntity, Boolean>> Filtro, params Expression<Func<TEntity, Object>>[] Includes)
        {
            try
            {
                IQueryable<TEntity> query = Context.Set<TEntity>();

                if (Includes != null)
                {
                    foreach (Expression<Func<TEntity, Object>> include in Includes)
                    {
                        query = query.Include(include) as IQueryable<TEntity>;
                    }
                }

                return await query.Where(Filtro).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new AccesoADatosException(ex);
            }
        }
        #endregion

        #region ObtenerListado
        public virtual async Task<List<TEntity>> ObtenerListadoAsync(params Expression<Func<TEntity, Object>>[] Includes)
        {
            return await ObtenerListadoAsync(null, Includes);
        }

        public virtual async Task<List<TEntity>> ObtenerListadoAsync(Expression<Func<TEntity, bool>> Filtro, params Expression<Func<TEntity, Object>>[] Includes)
        {
            try
            {
                IQueryable<TEntity> query = Context.Set<TEntity>();

                if (Includes != null)
                {
                    foreach (Expression<Func<TEntity, Object>> include in Includes)
                    {
                        query = query.Include(include) as IQueryable<TEntity>;
                    }
                }

                if (Filtro != null)
                {
                    return query.Where(Filtro).ToList();
                }
                else
                {
                    return await query.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new AccesoADatosException(ex);
            }
        }
        #endregion
    }
}
