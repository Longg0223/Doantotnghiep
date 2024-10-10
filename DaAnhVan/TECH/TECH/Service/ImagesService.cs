
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
    public interface IImagesService
    {           
        List<int> Add(List<string> view);
        bool Update(ImageModelView view);
        List<ImageModelView> GetImageName(List<int> imgaeId);
        bool Deleted(int id);
        void Remove(int id);
        void Save();
    }

    public class ImagesService : IImagesService
    {
        private readonly IImageRepository _imagesRepository;
        private IUnitOfWork _unitOfWork;
        public ImagesService(IImageRepository imagesRepository,
            IUnitOfWork unitOfWork)
        {
            _imagesRepository = imagesRepository;
            _unitOfWork = unitOfWork;
        }
        public List<ImageModelView> GetImageName(List<int> imgaeId)
        {
            if (imgaeId != null && imgaeId.Count > 0)
            {
                var lstImages = _imagesRepository.FindAll(p => imgaeId.Contains(p.id)).Select(p=>new ImageModelView()
                {
                    id=p.id,
                    name =p.name
                }).ToList();
                if (lstImages != null && lstImages.Count > 0)
                {
                    return lstImages;
                }
            }
            return null;
        }
        public List<int> Add(List<string> view)
        {
            try
            {
               
                if (view != null && view.Count > 0)
                {
                    var listItem = view.Select(p=> new Images
                    {
                        name = p
                    }).ToList();
                    foreach (var item in listItem)
                    {
                        _imagesRepository.Add(item);
                    }
                    Save();
                    return listItem.Select(p => p.id).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public bool Update(ImageModelView view)
        {
            try
            {
                var dataServer = _imagesRepository.FindById(view.id);
                if (dataServer != null)
                {
                    dataServer.name = view.name;
                    _imagesRepository.Update(dataServer);                                        
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
                var dataServer = _imagesRepository.FindById(id);
                if (dataServer != null)
                {
                    _imagesRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public void Remove(int id)
        {
            try
            {
                var dataServer = _imagesRepository.FindById(id);
                if (dataServer != null)
                {
                    _imagesRepository.Remove(dataServer);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
