
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using TECH.Utilities;

namespace TECH.Service
{
    public interface IPostsService
    {
        PagedResult<PostModelView> GetAllPaging(PostsViewModelSearch PostModelViewSearch);
        PostModelView GetByid(int id);
        bool Add(PostModelView view);
        bool Update(PostModelView view);
        bool Deleted(int id);
        void Save();
        bool IsNameExist(string name);
        bool UpdateStatus(int id, int status);
        int GetCount();
    }

    public class PostsService : IPostsService
    {
        private readonly IPostsRepository _postsRepository;
        private IUnitOfWork _unitOfWork;
        public PostsService(IPostsRepository postsRepository,
            IUnitOfWork unitOfWork)
        {
            _postsRepository = postsRepository;
            _unitOfWork = unitOfWork;
        }
        public bool IsNameExist(string title)
        {
            var data = _postsRepository.FindAll().Any(p=>p.title == title);
            return data;
        }
        public PostModelView GetByid(int id)
        {
            var data = _postsRepository.FindAll(p => p.id == id).FirstOrDefault();
            if (data != null)
            {                
                var model = new PostModelView()
                {
                    id = data.id,
                    title = data.title,
                    avatar = data.avatar,
                    slug = data.slug,
                    author = data.author,
                    status = data.status,
                    short_content = data.short_content,
                    content = data.content,
                    created_at = data.created_at,
                    updated_at = data.updated_at
                };
                return model;
            }
            return null;
        }
        public bool Add(PostModelView view)
        {
            try
            {
                if (view != null)
                {
                    var products = new Posts
                    {
                        title = view.title,
                        avatar = !string.IsNullOrEmpty(view.avatar) ? Regex.Replace(view.avatar.ToLower(), @"\s+", ""):"",
                        slug = Regex.Replace(view.title.ToLower(), @"\s+", "-"),
                        author = view.author,
                        status = 0,
                        short_content = view.short_content,
                        content = view.content,
                        created_at = DateTime.Now,                        
                    };
                    _postsRepository.Add(products);

                    return true;                    
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;

        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public bool Update(PostModelView view)
        {
            try
            {
                var dataServer = _postsRepository.FindById(view.id);
                if (dataServer != null)
                {
                    dataServer.id = view.id;
                    dataServer.title = view.title;
                    if (!string.IsNullOrEmpty(view.avatar))
                    {
                        dataServer.avatar =  Regex.Replace(view.avatar.ToLower(), @"\s+", "");
                    }
                    
                    dataServer.slug = Regex.Replace(view.title.ToLower(), @"\s+", "-");
                    //dataServer.author = view.author;
                    dataServer.short_content = view.short_content;
                    dataServer.content = view.content;
                    dataServer.updated_at = DateTime.Now;     
                    _postsRepository.Update(dataServer);                                        
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

      public  bool UpdateStatus(int id, int status)
        {
            try
            {
                var dataServer = _postsRepository.FindById(id);
                if (dataServer != null)
                {
                    dataServer.status = status;
                    _postsRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        public bool Deleted(int id)
        {
            try
            {
                var dataServer = _postsRepository.FindById(id);
                if (dataServer != null)
                {
                    _postsRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public int GetCount()
        {
            int count = 0;
            count = _postsRepository.FindAll().Count();
            return count;
        }
        public PagedResult<PostModelView> GetAllPaging(PostsViewModelSearch PostModelViewSearch)
        {
            try
            {
                var query = _postsRepository.FindAll();

                if (PostModelViewSearch.author_ids != null && PostModelViewSearch.author_ids.Count > 0)
                {
                    query = query.Where(c => PostModelViewSearch.author_ids.Contains(c.author.Value));
                }
                else
                {
                    if (!string.IsNullOrEmpty(PostModelViewSearch.name))
                    {
                        query = query.Where(c => c.title.ToLower().Trim().Contains(PostModelViewSearch.name.ToLower().Trim()));
                    }
                }

               

                int totalRow = query.Count();
                query = query.Skip((PostModelViewSearch.PageIndex - 1) * PostModelViewSearch.PageSize).Take(PostModelViewSearch.PageSize);
                var data = query.OrderByDescending(p => p.id).Select(p => new PostModelView()
                {
                    id = p.id,
                    title = p.title,
                    avatar = p.avatar,
                    slug = p.slug,
                    author = p.author,
                    status = p.status,
                    short_content = p.short_content,
                    content = p.content,
                    created_at = p.created_at,
                    create_atstr = p.created_at.HasValue ? p.created_at.Value.ToString("hh:mm") + " - " + p.created_at.Value.ToString("dd/MM/yyyy") : "",
                    updated_at = p.updated_at
                }).ToList();
              
                var pagingData = new PagedResult<PostModelView>
                {
                    Results = data,
                    CurrentPage = PostModelViewSearch.PageIndex,
                    PageSize = PostModelViewSearch.PageSize,
                    RowCount = totalRow,
                };
                return pagingData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }       
    }
}
