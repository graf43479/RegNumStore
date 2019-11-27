using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class EFOrderRepository: IOrderRepository
    {
        private RegNumDBContext context;

        public EFOrderRepository(RegNumDBContext context)
        {
            this.context = context;
        }

        public IQueryable<Order> Orders
        {
            get { return context.Orders; }
        }

        public void SaveOrder(Order order)
        {
            if (order.OrderID== 0)
            {
                context.Orders.Add(order);
            }
            else
            {
                context.Entry(order).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteOrder(Order order)
        {
            context.Orders.Remove(order);
            context.SaveChanges();
        }

        public async Task SaveOrderAsync(Order order)
        {
            if (order.OrderID == 0)
            {
                context.Orders.Add(order);
            }
            else
            {
                context.Entry(order).State = EntityState.Modified;
            }
            await context.SaveChangesAsync();
        }
    }
}
