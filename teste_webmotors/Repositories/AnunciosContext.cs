using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using teste_webmotors.Model;

namespace teste_webmotors.Repositories
{
    public interface IAnunciosContext
    {
        Task<int> Adicionar(Anuncio model);
        Task<int> Alterar(Anuncio model);
        Task Remover(Anuncio model);
        Task<IEnumerable<Anuncio>> Listar(int quantidade, int pagina);
        Task<Anuncio> BuscarPorId(int id);
    }
    public class AnunciosContext : IAnunciosContext
    {
        private readonly Context _context;
        public AnunciosContext(Context context)
        {
            this._context = context;
        }
        public async Task<int> Adicionar(Anuncio model)
        {
            var resultado = await _context.Anuncios.AddAsync(model);

            _context.SaveChanges();

            return resultado.Entity.Id;
        }

        public async Task<int> Alterar(Anuncio model)
        {
            var resultado = _context.Anuncios.Update(model);
            _context.SaveChanges();

            return resultado.Entity.Id;
        }

        public async Task<Anuncio> BuscarPorId(int id)
        {
            var model = await _context.Anuncios.Where(x => x.Id == id).FirstOrDefaultAsync();

            return model;
        }

        public async Task<IEnumerable<Anuncio>> Listar(int quantidade, int pagina)
        {
            var lista = await _context.Anuncios.Skip((pagina - 1) * quantidade).Take(quantidade).ToListAsync();
            return lista;
        }

        public async Task Remover(Anuncio model)
        {
            _context.Anuncios.Remove(model);
            _context.SaveChanges();
        }
    }
}
