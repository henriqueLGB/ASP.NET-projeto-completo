using Microsoft.AspNetCore.Mvc;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using AutoMapper;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using DevIO.App.Extensions;

namespace DevIO.App.Controllers
{
    [Authorize]
    public class FornecedoresController : BaseController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;

        public FornecedoresController(  IFornecedorRepository fornecedorRepository, 
                                        IMapper mapper, 
                                        IFornecedorService fornecedorService,
                                        INotificador notificador) : base(notificador)
        {
            _fornecedorRepository = fornecedorRepository;
            _fornecedorService = fornecedorService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("lista-de-fornecedores")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos()));
        }

        [AllowAnonymous]
        [Route("dados-do-fornecedor/{id:guid}")]
        public async Task<IActionResult> Details(Guid  id)
        {

            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null)
            {
                return NotFound();
            }

            return View(fornecedorViewModel);
        }

        [ClaimsAuthorize("Fornecedor", "Adicionar")]
        [Route("novo-fornecedor")]
        public IActionResult Create()
        {
            return View();
        }

        [ClaimsAuthorize("Fornecedor", "Adicionar")]
        [Route("novo-fornecedor")]
        [HttpPost]
        public async Task<IActionResult> Create(FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid) return View(fornecedorViewModel);

            await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(fornecedorViewModel));

            //VERIFICANDO PARA NOTIFICAR CASO OCORRÁ ALGUM ERRO
            if (!OperacaoValida()) return View(fornecedorViewModel);

            return RedirectToAction("Index");
 
        }

        [ClaimsAuthorize("Fornecedor", "Editar")]
        [Route("editar-fornecedor/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorProdutosEndereco(id);

            if (fornecedorViewModel == null)
            {
                return NotFound();
            }
            return View(fornecedorViewModel);
        }

        [ClaimsAuthorize("Fornecedor", "Editar")]
        [Route("editar-fornecedor/{id:guid}")]
        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> Edit(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id) return NotFound(); 

            if (!ModelState.IsValid) return View(fornecedorViewModel);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorService.Atualizar(fornecedor);

            //VERIFICANDO PARA NOTIFICAR CASO OCORRÁ ALGUM ERRO
            if (!OperacaoValida()) return View(fornecedorViewModel);

            return RedirectToAction("Index");

        }

        [ClaimsAuthorize("Fornecedor", "Excluir")]
        [Route("excluir-fornecedores/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {

            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null)
            {
                return NotFound();
            }

            return View(fornecedorViewModel);
        }

        [ClaimsAuthorize("Fornecedor", "Excluir")]
        [Route("excluir-fornecedores/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null) return NotFound();

            await _fornecedorService.Remover(id);

            //VERIFICANDO PARA NOTIFICAR CASO OCORRÁ ALGUM ERRO
            if (!OperacaoValida()) return View(fornecedorViewModel);

            return RedirectToAction("Index");
        }

        [Route("obter-endereco-fornecedor/{id:guid}")]
        public async Task<IActionResult> ObterEndereco(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);

            if(fornecedor == null)
            {
                return NotFound();
            }

            return PartialView("_DetalhesEndereco",fornecedor);
        }

        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        public async Task<IActionResult> AtualizaEndereco(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);
            
            if (fornecedor == null)
            {
                return NotFound();  
            }

            return PartialView("_AtualizaEndereco", new FornecedorViewModel { Endereco = fornecedor.Endereco });
        }

        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> AtualizaEndereco(FornecedorViewModel fornecedorViewModel)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("Documento");

            if (!ModelState.IsValid) return PartialView("_AtualizaEndereco", fornecedorViewModel);

            await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(fornecedorViewModel.Endereco));

            //VERIFICANDO PARA NOTIFICAR CASO OCORRÁ ALGUM ERRO
            if (!OperacaoValida()) return PartialView("_AtualizaEndereco", fornecedorViewModel);

            var url = Url.Action("ObterEndereco","Fornecedores",new {id = fornecedorViewModel.Endereco.FornecedorId});

            return Json(new {success = true, url});
        }

        private async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
        }

        private async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
        }
    }
}
