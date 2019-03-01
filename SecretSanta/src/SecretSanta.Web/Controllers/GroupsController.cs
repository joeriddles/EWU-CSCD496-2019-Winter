using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SecretSanta.Web.ViewModels;

namespace SecretSanta.Web.Controllers
{
    public class GroupsController : Controller
    {
        private IHttpClientFactory ClientFactory { get; }
        private IMapper Mapper { get; }
        private ILogger Logger { get; }
        public GroupsController(IHttpClientFactory clientFactory, IMapper mapper, ILoggerFactory loggerFactory)
        {
            ClientFactory = clientFactory;
            Mapper = mapper;
            Logger = loggerFactory.CreateLogger("GroupsController");
        }

        public async Task<IActionResult> Index()
        {
            using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            {
                var apiClient = new ApiClient(httpClient.BaseAddress.ToString(), httpClient);
                ViewBag.Groups = await apiClient.GetGroupsAsync();
            }
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(GroupInputViewModel viewModel)
        {
            IActionResult result = View();

            if (ModelState.IsValid)
            {
                using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
                {
                    try
                    {
                        var apiClient = new ApiClient(httpClient.BaseAddress.ToString(), httpClient);
                        await apiClient.CreateGroupAsync(viewModel);

                        result = RedirectToAction(nameof(Index));
                    }
                    catch (SwaggerException se)
                    {
                        ViewBag.ErrorMessage = se.Message;
                    }
                }
            }

            return result;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            GroupViewModel fetchedGroup = null;
            using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            {
                try
                {
                    var apiClient = new ApiClient(httpClient.BaseAddress.ToString(), httpClient);
                    fetchedGroup = await apiClient.GetGroupAsync(id);
                }
                catch (SwaggerException se)
                {
                    ModelState.AddModelError("", se.Message);
                }

                return View(fetchedGroup);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GroupViewModel viewModel)
        {
            IActionResult result = View();

            if (ModelState.IsValid)
            {
                using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
                {
                    try
                    {
                        var apiClient = new ApiClient(httpClient.BaseAddress.ToString(), httpClient);
                        await apiClient.UpdateGroupAsync(viewModel.Id, Mapper.Map<GroupInputViewModel>(viewModel));

                        result = RedirectToAction(nameof(Index));
                    }
                    catch (SwaggerException se)
                    {
                        ModelState.AddModelError("", se.Message);
                    }
                }
            }

            return result;
        }

        public async Task<IActionResult> Delete(int id)
        {
            IActionResult result = View("Index");
            using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            {
                try
                {
                    var apiClient = new ApiClient(httpClient.BaseAddress.ToString(), httpClient);
                    await apiClient.DeleteGroupAsync(id);

                    result = RedirectToAction(nameof(Index));
                }
                catch (SwaggerException se)
                {
                    ModelState.AddModelError("", se.Message);
                }
            }

            return result;
        }
    }
}