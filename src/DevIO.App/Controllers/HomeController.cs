using DevIO.App.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DevIO.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Errors(int id)
        {
            var modelErro = new ErrorViewModel();

            switch (id)
            {
                case 500:
                    modelErro.Mensagem = "Ocorreu um erro ! Tente novamente mais tarde ou contate o suporte.";
                    modelErro.Titulo = "Ocorreu um erro !";
                    modelErro.ErroCode = id;
                    break;
                case 404:
                    modelErro.Mensagem = "A página que está procurando não existe ! <br /> Em caso de dúvida entre em contato com o suporte.";
                    modelErro.Titulo = "Ops! página não encontrada.";
                    modelErro.ErroCode = id;
                    break;
                case 403:
                    modelErro.Mensagem = "Você não tem permissão para fazer isto.";
                    modelErro.Titulo = "Acesso Negado";
                    modelErro.ErroCode = id;
                    break;
                default:
                    StatusCode(404);
                    break;
            }

            return View("Error", modelErro);
        }
    }
}