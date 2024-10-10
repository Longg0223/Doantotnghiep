using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;

namespace TECH.Controllers.Components

{
    [ViewComponent(Name = "PostsComponent")]
    public class PostsComponent : ViewComponent
    {
        private readonly IPostsService _postsService;
        private readonly IAppUserService _appUserService;
        public PostsComponent(IPostsService postsService, IAppUserService appUserService)
        {
            _postsService = postsService;
            _appUserService = appUserService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var postsViewModelSearch = new PostsViewModelSearch();
            postsViewModelSearch.PageIndex = 1;
            postsViewModelSearch.PageSize = 4;
            var data = _postsService.GetAllPaging(postsViewModelSearch);
            data.Results = data.Results.Where(p => p.status != 1).ToList();
            foreach (var item in data.Results)
            {
                if (item.author.HasValue)
                {
                    var appUser = _appUserService.GetByid(item.author.Value);
                    if (appUser != null && !string.IsNullOrEmpty(appUser.full_name))
                    {
                        item.author_name = appUser.full_name;
                    }
                }
            }
            if (data != null && data.Results != null && data.Results.Count > 0)
            {
                data.Results = data.Results.OrderByDescending(p => p.id).ToList();
            }
            return View(data);
        }
    }
}