using Loja.DAO;
using Loja.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LojaAPI.Controllers
{
    public class CarrinhoController : ApiController
    {
        public HttpResponseMessage Get(int id)
        {
            try
            {
                Carrinho carrinho = new CarrinhoDAO().Busca(id);

                return Request.CreateResponse(HttpStatusCode.OK, carrinho);
            }
            catch (Exception)
            {
                HttpError erro = new HttpError($"O carrinho {id} não foi encontrato");
                return Request.CreateResponse(HttpStatusCode.NotFound, erro);
            }
        }

        public HttpResponseMessage Post([FromBody] Carrinho carrinho)
        {
            new CarrinhoDAO().Adiciona(carrinho);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
            string location = Url.Link("DefaultApi", new { controller = "carrinho", id = carrinho.Id });
            response.Headers.Location = new Uri(location);

            return response;
        }

        [Route("api/carrinho/{idCarrinho}/produto/{idProduto}")]
        public HttpResponseMessage Delete([FromUri] int idCarrinho, [FromUri] int idProduto)
        {
            Carrinho carrino = new CarrinhoDAO().Busca(idCarrinho);
            carrino.Remove(idProduto);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/carrinho/{idCarrinho}/produto/{idProduto}/quantidade")]
        public HttpResponseMessage Put([FromBody] Produto produto, [FromUri] int idCarrinho, [FromUri] int idProduto)
        {
            Carrinho carrinho = new CarrinhoDAO().Busca(idCarrinho);

            carrinho.TrocaQuantidade(produto);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
