
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
    public interface ICartsService
    {
        void Add(CartsModelView view);
        bool Update(CartsModelView view);
        bool Deleted(int id);
        bool IsExist(int user_id, int product_id);
        void Save();
        CartsModelView GetProductCart(int user_id, int product_id);
        List<CartsModelView> GetAllCart(int user_id);
        CartsModelView GetById(int id);
    }

    public class CartsService : ICartsService
    {
        private readonly ICartsRepository _cartsRepository;
        private IUnitOfWork _unitOfWork;
        public CartsService(ICartsRepository cartsRepository,
            IUnitOfWork unitOfWork)
        {
            _cartsRepository = cartsRepository;
            _unitOfWork = unitOfWork;
        }
        public CartsModelView GetProductCart(int user_id, int product_id)
        {
            var data = _cartsRepository.FindAll().Where(p => p.user_id == user_id && p.product_id == product_id).Select(c => new CartsModelView()
            {
                id = c.id,
                user_id = c.user_id,
                product_id = c.product_id,
                //color = c.color,
                price = c.price,
                //sizeId = c.sizeId,
                quantity = c.quantity
            }).FirstOrDefault();
            return data;
        }
        public bool IsExist(int user_id, int product_id)
        {
            var data = _cartsRepository.FindAll().Where(p => p.user_id == user_id && p.product_id == product_id).FirstOrDefault();
            if (data != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<CartsModelView> GetAllCart(int user_id)
        {
            var data = _cartsRepository.FindAll().Where(p=>p.user_id == user_id).OrderByDescending(p => p.id).Select(c=>new CartsModelView()
            {
                id = c.id,
                user_id = c.user_id,
                product_id = c.product_id,
                //color = c.color,
                price = c.price,
                //sizeId = c.sizeId,
                quantity = c.quantity
            }).ToList();

            return data;
        }       
        public CartsModelView GetById(int id)
        {
            if (id > 0)
            {
                var data = _cartsRepository.FindAll().Where(p => p.id == id).Select(c => new CartsModelView()
                {
                    id = c.id,
                    user_id = c.user_id,
                    product_id = c.product_id,
                    //color = c.color,
                    price = c.price,
                    //sizeId = c.sizeId,
                    quantity = c.quantity
                }).FirstOrDefault();

                return data;
            }
            return null;
           
        }
        public void Add(CartsModelView view)
        {
            try
            {
                if (view != null)
                {
                    var carts = new Carts
                    {
                        user_id = view.user_id,
                        product_id = view.product_id,
                        //color = view.color,
                        price = view.price,
                        //sizeId = view.sizeId,
                        quantity = view.quantity,
                    };
                    _cartsRepository.Add(carts);
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
        public bool Update(CartsModelView view)
        {
            try
            {
                var dataServer = _cartsRepository.FindById(view.id);
                if (dataServer != null)
                {                    
                    dataServer.quantity = view.quantity;
                    dataServer.price = view.price.Value;
                    _cartsRepository.Update(dataServer);                                        
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
                var dataServer = _cartsRepository.FindById(id);
                if (dataServer != null)
                {
                    _cartsRepository.Remove(dataServer);
                    Save();
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }    
    }
}
