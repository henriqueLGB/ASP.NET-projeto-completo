using Microsoft.AspNetCore.Mvc;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using AutoMapper;
using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DevIO.App.Extensions;

namespace DevIO.App.Controllers
{
    [Authorize]
    public class ProdutosController : BaseController
    {
        private readonly IProdutosRepository _produtoRespository;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;

        public ProdutosController(  IProdutosRepository produtosRepository, 
                                    IMapper mapper, 
                                    IFornecedorRepository fornecedorRepository, 
                                    IProdutoService produtoService,
                                    INotificador notificador) : base(notificador)
        {
            _produtoRespository = produtosRepository;
            _mapper = mapper;
            _fornecedorRepository = fornecedorRepository;
            _produtoService = produtoService;
        }

        [AllowAnonymous]
        [Route("lista-de-produtos")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRespository.ObterProdutosFornecedores()));
        }

        [AllowAnonymous]    
        [Route("dados-produto/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {

            var produtoViewModel = await ObterProduto(id);
            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Adicionar")]
        [Route("novo-produto")]
        public async Task<IActionResult> Create()
        {
            var produtoViewModel = await PopularFornecedores(new ProdutoViewModel());
            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Adicionar")]
        [Route("novo-produto")]
        [HttpPost]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {

            produtoViewModel = await PopularFornecedores(produtoViewModel);

            if (!ModelState.IsValid) return View(produtoViewModel);

            //CONFIGURANDO PARA SALVAR A IMAGEM
            //CRIANDO O NOME DO ARQUIVO UTILIZANDO O GUID E CONCATENANDO COM O NOME DO ARQUIVO  
            var imgPrefixo = Guid.NewGuid() + "_";

            if (!await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo)) return View(produtoViewModel);

            produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));
            
            //VERIFICANDO PARA NOTIFICAR CASO OCORRÁ ALGUM ERRO
            if(!OperacaoValida()) return View(produtoViewModel);

            return RedirectToAction("Index");

        }

        [ClaimsAuthorize("Produto", "Editar")]
        [Route("editar-produto/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {

            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Editar")]
        [Route("editar-produto/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(produtoViewModel);

            if (produtoViewModel.ImagemUpload != null)
            {
                //CONFIGURANDO PARA SALVAR A IMAGEM
                //CRIANDO O NOME DO ARQUIVO UTILIZANDO O GUID E CONCATENANDO COM O NOME DO ARQUIVO  
                var imgPrefixo = Guid.NewGuid() + "_";

                if (!await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo)) return View(produtoViewModel);

                produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
            }

            await _produtoService.Atualizar(_mapper.Map<Produto>(produtoViewModel));

            //VERIFICANDO PARA NOTIFICAR CASO OCORRÁ ALGUM ERRO
            if (!OperacaoValida()) return View(produtoViewModel);

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Produto", "Excluir")]
        [Route("excluir-produto/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {

            var produto = await ObterProduto(id);

            if (produto == null) return NotFound();

            return View(produto);
        }

        [ClaimsAuthorize("Produto", "Excluir")]
        [Route("excluir-produto/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produto = await ObterProduto(id);

            if (produto == null) return NotFound();

            await _produtoService.Remover(id);

            //VERIFICANDO PARA NOTIFICAR CASO OCORRÁ ALGUM ERRO
            if (!OperacaoValida()) return View(produto);

            return RedirectToAction("Index");
        }

        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            var produto = _mapper.Map<ProdutoViewModel>(await _produtoRespository.ObterProdutoFornecedor(id));
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());

            return produto;
        }

        private async Task<ProdutoViewModel> PopularFornecedores(ProdutoViewModel produto)
        {
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            return produto;
        }

        private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
        {
            //VERIFICA SE O TAMANHO DO ARQUIVO É MENOR OU IGUAL A ZERO 
            if (arquivo.Length <= 0) return false;

            //CAMINHO ONDE SERÁ SALVO A IMAGEM
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefixo + arquivo.FileName);

            //VERIFICA SE O ARQUIVO JÁ EXISTE
            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome !");
                return false;
            }

            //UTILIZA O FILESTREAM PARA SALVAR O ARQUIVO
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;

        }
    }
}
