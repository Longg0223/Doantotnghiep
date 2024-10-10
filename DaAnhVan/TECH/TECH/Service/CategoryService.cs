
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using TECH.Utilities;

namespace TECH.Service
{
    public interface ICategoryService
    {
        PagedResult<CategoryModelView> GetAllPaging(CategoryViewModelSearch CategoryModelViewSearch);
        CategoryModelView GetByid(int id);
        void Add(CategoryModelView view);
        bool Update(CategoryModelView view);
        bool Deleted(int id);
        void Save();
        bool UpdateStatus(int id, int status);
        bool IsCategoryNameExist(string name);
        List<CategoryModelView> GetAll();
        int GetCount();
        List<CategoryModelView> GetAllMenu();
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private IUnitOfWork _unitOfWork;
        public CategoryService(ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }
        public List<CategoryModelView> GetAll()
        {
            var data = _categoryRepository.FindAll(c => c.status == 0).Select(c=>new CategoryModelView()
            {
                id = c.id,
                name = c.name,
                icon = c.icon
            }).ToList();

            return data;
        }

        public List<CategoryModelView> GetAllMenu()
        {
            var data = _categoryRepository.FindAll(c=>c.status == 0).Select(c => new CategoryModelView()
            {
                id = c.id,
                name = c.name,
                icon = c.icon
            }).ToList();

            return data;
        }

        public bool IsCategoryNameExist(string name)
        {
            var data = _categoryRepository.FindAll().Any(p => p.name == name);
            return data;
        }

        public CategoryModelView GetByid(int id)
        {
            var data = _categoryRepository.FindAll(p => p.id == id).FirstOrDefault();
            if (data != null)
            {                
                var model = new CategoryModelView()
                {
                    id = data.id,
                    name = data.name,
                    slug = data.slug,
                    status = data.status,
                    icon = data.icon,
                    created_at = data.created_at,
                    updated_at = data.updated_at
                };
                return model;
            }
            return null;
        }
        public int GetCount()
        {
            int count = 0;
            count = _categoryRepository.FindAll().Count();
            return count;
        }
        public void Add(CategoryModelView view)
        {
            try
            {
                if (view != null)
                {
                    var category = new Category
                    {                      
                        name = view.name,
                        icon = view.icon,
                        status = 0,
                        created_at = DateTime.Now,
                    };
                    _categoryRepository.Add(category);                  
                }
            }
            catch (Exception ex)
            {
            }

        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public bool Update(CategoryModelView view)
        {
            try
            {
                var dataServer = _categoryRepository.FindById(view.id);
                if (dataServer != null)
                {
                    dataServer.name = view.name;
                    dataServer.slug = view.name.ToLower();                    
                    dataServer.icon = view.icon;
                    dataServer.updated_at = DateTime.Now;
                    _categoryRepository.Update(dataServer);                                        
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
                var dataServer = _categoryRepository.FindById(id);
                if (dataServer != null)
                {
                    dataServer.status = status;
                    _categoryRepository.Update(dataServer);
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
                var dataServer = _categoryRepository.FindById(id);
                if (dataServer != null)
                {
                    //dataServer.isdetele = 1;
                    //_categoryRepository.Update(dataServer);
                    _categoryRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public PagedResult<CategoryModelView> GetAllPaging(CategoryViewModelSearch CategoryModelViewSearch)
        {
            try
            {
                var query = _categoryRepository.FindAll();               
                
                if (!string.IsNullOrEmpty(CategoryModelViewSearch.name))
                {
                    query = query.Where(c => c.name.ToLower().Trim().Contains(CategoryModelViewSearch.name.ToLower().Trim()));
                }

                int totalRow = query.Count();
                query = query.Skip((CategoryModelViewSearch.PageIndex - 1) * CategoryModelViewSearch.PageSize).Take(CategoryModelViewSearch.PageSize);
                var data = query.OrderByDescending(p => p.id).Select(c => new CategoryModelView()
                {
                    name = (!string.IsNullOrEmpty(c.name) ? c.name : ""),
                    id = c.id,
                    slug = !string.IsNullOrEmpty(c.slug) ? c.slug : "",                   
                    status = c.status,
                    status_str = c.status.HasValue && c.status.Value == 0 ? "<span class='badge' style='background-color: #5cb85c;'>Hoạt động</span>" : "<span class='badge' style='background-color: #d9534f;'>Ẩn</span>",
                    created_at = c.created_at,
                    created_str = c.created_at.ToString("dd/MM/yyyy"),
                    updated_str = c.updated_at.HasValue ? c.updated_at.Value.ToString("dd/MM/yyyy"):"",
                    icon = c.icon,
                    updated_at = c.updated_at,
                }).ToList();
              
                var pagingData = new PagedResult<CategoryModelView>
                {
                    Results = data,
                    CurrentPage = CategoryModelViewSearch.PageIndex,
                    PageSize = CategoryModelViewSearch.PageSize,
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
