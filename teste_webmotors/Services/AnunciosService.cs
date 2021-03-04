using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using teste_webmotors.Model;
using teste_webmotors.Repositories;
using teste_webmotors.ViewModel;

namespace teste_webmotors.Services
{
    public interface IAnunciosService
    {
        Task<RetornoBase> Adicionar(AnuncioVMIncluir model);
        Task<RetornoBase> Alterar(AnuncioVMAlterar model);
        Task<RetornoBase> Remover(int anuncioId);
        Task<RetornoBase> Listar(int quantidade, int pagina);
        Task<RetornoBase> BuscarPorId(int id);
    }
    public class AnunciosService : IAnunciosService
    {
        private readonly IAnunciosContext _anunciosContext;
        public AnunciosService(IAnunciosContext anunciosContext)
        {
            this._anunciosContext = anunciosContext;
        }
        public async Task<RetornoBase> Adicionar(AnuncioVMIncluir model)
        {
            var anuncio = new Anuncio()
            {
                Marca = model.Marca,
                Modelo = model.Modelo,
                Ano = model.Ano,
                Observacao = model.Observacao,
                Quilometragem = model.Quilometragem,
                Versao = model.Versao
            };

            // retorna id do ultimo adicionado
            var retorno = await _anunciosContext.Adicionar(anuncio);

            return new RetornoBase() { Objeto = retorno, StatusCode = (int)HttpStatusCode.Created };
        }

        public async Task<RetornoBase> Alterar(AnuncioVMAlterar model)
        {

            var anuncio = new Anuncio()
            {
                Id = model.Id,
                Marca = model.Marca,
                Modelo = model.Modelo,
                Ano = model.Ano,
                Observacao = model.Observacao,
                Quilometragem = model.Quilometragem,
                Versao = model.Versao
            };

            var retorno = await _anunciosContext.Alterar(anuncio);

            return new RetornoBase() { Objeto = retorno, StatusCode = (int)HttpStatusCode.OK };

        }

        public async Task<RetornoBase> BuscarPorId(int id)
        {
            var anuncio = await _anunciosContext.BuscarPorId(id);

            if (anuncio == null)
                return new RetornoBase() { StatusCode = (int)HttpStatusCode.NotFound };

            var retorno = new AnuncioVMRetornar()
            {
                Id = anuncio.Id,
                Marca = anuncio.Marca,
                Modelo = anuncio.Modelo,
                Ano = anuncio.Ano,
                Observacao = anuncio.Observacao,
                Quilometragem = anuncio.Quilometragem,
                Versao = anuncio.Versao
            };

            return new RetornoBase() { Objeto = retorno, StatusCode = (int)HttpStatusCode.Created };
        }

        public async Task<RetornoBase> Listar(int quantidade, int pagina)
        {
            var lista = await _anunciosContext.Listar(quantidade, pagina);

            var retorno = new List<AnuncioVMListar>();

            foreach (var item in lista)
            {
                retorno.Add(new AnuncioVMListar()
                {
                    Id = item.Id,
                    Marca = item.Marca,
                    Modelo = item.Modelo,
                    Ano = item.Ano,
                    Observacao = item.Observacao,
                    Quilometragem = item.Quilometragem,
                    Versao = item.Versao
                });
            }

            return new RetornoBase() { Objeto = retorno, StatusCode = (int)HttpStatusCode.OK };
        }

        public async Task<RetornoBase> Remover(int anuncioId)
        {
            var anuncio = await _anunciosContext.BuscarPorId(anuncioId);

            if (anuncio != null)
                await _anunciosContext.Remover(anuncio);
            else
                return new RetornoBase() { Objeto = false, StatusCode = (int)HttpStatusCode.NotFound }; 

            return new RetornoBase() { Objeto = true, StatusCode = (int)HttpStatusCode.NoContent }; 

        }
    }
}
