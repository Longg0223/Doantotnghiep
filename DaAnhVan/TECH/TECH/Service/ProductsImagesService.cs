
using Microsoft.CodeAnalysis;
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
    public interface IProductsImagesService
    {           
        void Add(List<ProductImageModelView> view);
        bool Update(List<ProductImageModelView> view);
        List<int> GetImageProduct(int productid);
        bool Deleted(int id);
        void Remove(List<int> ids);
        void RemoveProductId(int productId);
        void Save();
    }

    public class ProductsImagesService : IProductsImagesService
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IProductsImageRepository _productsImageRepository;
        private IUnitOfWork _unitOfWork;
        public ProductsImagesService(IProductsImageRepository productsImageRepository,
            IUnitOfWork unitOfWork)
        {
            _productsImageRepository = productsImageRepository;
            _unitOfWork = unitOfWork;
        }
        public List<int> GetImageProduct(int productid)
        {
            int imageid = 0;
            if (productid > 0)
            {
                var productimage = _productsImageRepository.FindAll(p => p.product_id == productid).Select(p=>p.image_id.Value).ToList();
                if (productimage != null)
                    return productimage;
            }
            return null;
        }

        public void Add(List<ProductImageModelView> view)
        {
            try
            {
                if (view != null && view.Count > 0)
                {
                    foreach (var item in view)
                    {
                        var image = new ProductImages
                        {
                            product_id = item.product_id,
                            image_id = item.images_id
                        };
                        _productsImageRepository.Add(image);
                    }
                                
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
        public bool Update(List<ProductImageModelView> view)
        {
            try
            {
                if (view != null && view.Count > 0)
                {
                    foreach (var item in view)
                    {
                        var dataServer = _productsImageRepository.FindById(item.id);
                        if (dataServer != null)
                        {
                            dataServer.product_id = item.product_id;
                            dataServer.image_id = item.images_id;
                            _productsImageRepository.Update(dataServer);
                           
                        }
                    }
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
                var dataServer = _productsImageRepository.FindById(id);
                if (dataServer != null)
                {
                    _productsImageRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public void RemoveProductId(int productId)
        {
            try
            {
                var dataServer = _productsImageRepository.FindAll(p => p.product_id == productId).ToList();
                if (dataServer != null && dataServer.Count > 0)
                {
                    foreach (var item in dataServer)
                    {
                        _productsImageRepository.Remove(item.id);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void Remove(List<int> ids)
        {
            try
            {
                var dataServer = _productsImageRepository.FindAll(p=> ids.Contains(p.image_id.Value)).ToList();
                if (dataServer != null && dataServer.Count > 0)
                {
                    foreach (var item in dataServer)
                    {
                        _productsImageRepository.Remove(item.id);
                    }                   
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
