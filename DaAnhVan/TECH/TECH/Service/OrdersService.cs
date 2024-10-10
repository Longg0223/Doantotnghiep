
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public interface IOrdersService
    {
        PagedResult<OrdersModelView> GetAllPaging(OrdersViewModelSearch OrdersModelViewSearch);
        OrdersModelView GetByid(int id);
        bool Add(OrdersModelView view);
        bool Update(OrdersModelView view);
        bool UpdateReview(int orderId, int review);
        bool Deleted(int id);
        void Save();
        bool UpdateStatus(int id, int status);
        int AddOrder(OrdersModelView view);
        bool AddOrderDetail(OrdersDetailModelView view);

        List<OrdersDetailModelView> GetOrderDetails(int orderId);

        List<OrdersModelView> GetOrderForUserId(int user_id);
        //OrderStatistical GetOrderStatistical();
        Dictionary<string, OrderStatistical> GetOrderStatistical();
    }

    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private IUnitOfWork _unitOfWork;
        public OrdersService(IOrdersRepository ordersRepository,
            IOrderDetailsRepository orderDetailsRepository,
            IUnitOfWork unitOfWork)
        {
            _ordersRepository = ordersRepository;
            _orderDetailsRepository = orderDetailsRepository;
            _unitOfWork = unitOfWork;
        }
        public List<OrdersDetailModelView> GetOrderDetails(int orderId)
        {
            var data = _orderDetailsRepository.FindAll().Where(p => p.order_id == orderId).Select(p => new OrdersDetailModelView()
            {
                id = p.id,
                order_id = p.order_id,
                product_id = p.product_id,
                //color = p.color,
                price = p.price.Value,
                quantity = p.quantity,              
            }).ToList();
            return data;
        }

        public Dictionary<string, OrderStatistical> GetOrderStatistical()
        {
            
            var data = _ordersRepository.FindAll().Select(o=> new OrdersModelView()
            {
                code = o.code,
                created_at = o.created_at,
                status = o.status
            }).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    item.created_atstr = item.created_at.Value.ToString("dd/MM/yyyy");
                }
            }
            var dataConvert = data.GroupBy(p => p.created_atstr);
            var OrderStatistical = new Dictionary<string, OrderStatistical>();
            if (dataConvert != null)
            {
                foreach (var item in dataConvert)
                {
                    var orderStatistical = new OrderStatistical();
                    orderStatistical.TotalAccomplished = item.Where(p => p.status == 1).Count(); // đã hoàn thành
                    orderStatistical.TotalWaitingProgressing = item.Where(p => p.status == 0).Count(); // chờ xử lý
                    orderStatistical.Totalcancel = item.Where(p => p.status == 2).Count(); // đã hủy
                    OrderStatistical.Add(item.Key, orderStatistical);
                }
            }
            return OrderStatistical;
        }

        public OrdersModelView GetByid(int id)
        {
            var data = _ordersRepository.FindAll(p => p.id == id).FirstOrDefault();
            if (data != null)
            {                
                var model = new OrdersModelView()
                {
                    id = data.id,
                    user_id = data.user_id,
                    note = data.note,
                    review = data.review,
                    payment = data.payment,
                    status = data.status,
                    total = data.total,
                    full_name = data.full_name,
                    phone_number = data.phone_number,
                    fee_ship = data.fee_ship,
                    created_at = data.created_at,
                    code=data.code
                };
                return model;
            }
            return null;
        }
        public bool Add(OrdersModelView view)
        {
            try
            {
                if (view != null)
                {
                    var products = new Orders
                    {
                        user_id = view.user_id,
                        note = view.note,
                        review = view.review,
                        payment = view.payment,
                        status = view.status,
                        total = view.total,
                        fee_ship = view.fee_ship,
                        created_at = DateTime.Now,
                        //code = view.code
                    };
                    _ordersRepository.Add(products);

                    return true;                    
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;

        }

        public int AddOrder(OrdersModelView view)
        {
            try
            {
                if (view != null)
                {
                    var products = new Orders
                    {
                        user_id = view.user_id,
                        note = view.note,
                        payment = view.payment,
                        status = 0,
                        total = view.total,
                        full_name = view.full_name,
                        phone_number = view.phone_number,
                        fee_ship = view.fee_ship,
                        created_at = DateTime.Now,
                        code = view.code
                    };
                    _ordersRepository.Add(products);
                    Save();
                    return products.id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 0;

        }

        public bool AddOrderDetail(OrdersDetailModelView view)
        {
            try
            {
                if (view != null)
                {
                    var ordersDetails = new OrdersDetails
                    {
                        order_id = view.order_id,
                        product_id = view.product_id,
                        //color = view.color,
                        //sizeId = view.sizeId,
                        price = view.price.Value,
                        quantity = view.quantity
                        //code = view.code
                    };
                    _orderDetailsRepository.Add(ordersDetails);
                    Save();
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
        public bool Update(OrdersModelView view)
        {
            try
            {
                var dataServer = _ordersRepository.FindById(view.id);
                if (dataServer != null)
                {
                    //dataServer.code = view.code;
                    dataServer.note = view.note;
                    dataServer.review = view.review;
                    dataServer.payment = view.payment;
                    dataServer.status = view.status;
                    dataServer.total = view.total;
                    dataServer.fee_ship = view.fee_ship;
                    dataServer.created_at = DateTime.Now;
                    _ordersRepository.Update(dataServer);                                        
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

       public bool UpdateReview(int orderId, int review)
        {
            try
            {
                var dataServer = _ordersRepository.FindById(orderId);
                if (dataServer != null)
                {
                    dataServer.review = review;
                    _ordersRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        public bool UpdateStatus(int id, int status)
        {
            try
            {
                var dataServer = _ordersRepository.FindById(id);
                if (dataServer != null)
                {
                    dataServer.status = status;
                    _ordersRepository.Update(dataServer);
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
                var dataServer = _ordersRepository.FindById(id);
                if (dataServer != null)
                {
                    _ordersRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }

        public List<OrdersModelView> GetOrderForUserId(int user_id)
        {
            var data = _ordersRepository.FindAll().Where(o=>o.user_id == user_id).Select(p => new OrdersModelView()
            {
                id = p.id,
                code = p.code,
                user_id = p.user_id,
                note = p.note,
                review = p.review,
                payment = p.payment,
                status = p.status,
                total = p.total,
                totalstr = p.total.HasValue && p.total.Value > 0 ? p.total.Value.ToString("#,###") : "",
                fee_ship = p.fee_ship,
                created_atstr = p.created_at.HasValue ? p.created_at.Value.ToString("hh:mm") + " - " + p.created_at.Value.ToString("dd/MM/yyyy") : "",
            }).ToList();
            return data;
        }

        public PagedResult<OrdersModelView> GetAllPaging(OrdersViewModelSearch OrdersModelViewSearch)
        {
            try
            {
                var query = _ordersRepository.FindAll();

                //if (OrdersModelViewSearch.user_ids != null && OrdersModelViewSearch.user_ids.Count > 0)
                //{
                //    query = query.Where(c => OrdersModelViewSearch.user_ids.Contains(c.user_id.Value));
                //}
                //else
                //{
                //    if (!string.IsNullOrEmpty(OrdersModelViewSearch.name))
                //    {
                //        query = query.Where(c => c.code.ToLower().Contains(OrdersModelViewSearch.name.ToLower().Trim()));
                //    }
                //}
                if (!string.IsNullOrEmpty(OrdersModelViewSearch.name))
                {
                    query = query.Where(c => c.code.ToLower().Contains(OrdersModelViewSearch.name.ToLower().Trim()) 
                    || c.full_name.ToLower().Trim().Contains(OrdersModelViewSearch.name.ToLower().Trim())
                    || c.phone_number.ToLower().Trim().Contains(OrdersModelViewSearch.name.ToLower().Trim()));
                }

                if (OrdersModelViewSearch.payment.HasValue)
                {
                    query = query.Where(c => c.payment == OrdersModelViewSearch.payment.Value);
                }
                
                if (OrdersModelViewSearch.status.HasValue)
                {
                    query = query.Where(c => c.status == OrdersModelViewSearch.status.Value);
                }


                int totalRow = query.Count();
                query = query.OrderByDescending(p => p.id);
                query = query.Skip((OrdersModelViewSearch.PageIndex - 1) * OrdersModelViewSearch.PageSize).Take(OrdersModelViewSearch.PageSize);
               
                var data = query.Select(p => new OrdersModelView()
                {
                    id = p.id,
                    code=p.code,
                    user_id = p.user_id,
                    note = p.note,
                    review = p.review,
                    payment = p.payment,
                    full_name = p.full_name,
                    phone_number = p.phone_number,
                    status = p.status,
                    total = p.total,
                    totalstr = p.total.HasValue && p.total.Value > 0? p.total.Value.ToString("#,###"):"",
                    fee_ship = p.fee_ship,
                    created_atstr = p.created_at.HasValue ? p.created_at.Value.ToString("hh:mm") + " - " + p.created_at.Value.ToString("dd/MM/yyyy") : "",
                }).ToList();
              
                var pagingData = new PagedResult<OrdersModelView>
                {
                    Results = data,
                    CurrentPage = OrdersModelViewSearch.PageIndex,
                    PageSize = OrdersModelViewSearch.PageSize,
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
