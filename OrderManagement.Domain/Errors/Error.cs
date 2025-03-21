using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Errors
{
    public class Error
    {
        public string Code { get; }
        public string Message { get; }
        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }
        public static implicit operator string(Error error) { return error.Code; }
        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error CommitError = new("Error.CommitError", "Error when save data to server.");


        #region Order error
        public static readonly Error OrderNotFound = new("Error.OrderNotFound", "Order not found.");
        //update error message
        public static readonly Error OrderUpdateError = new("Error.OrderUpdateError", "Error when update order.");
        public static readonly Error OrderDeleteError = new("Error.OrderDeleteError", "Error when delete order.");
        public static readonly Error OrderDeleteAllOrderDetailError = new("Error.OrderDeleteAllOrderDetailError", "This order was deleted when there was no OrderDetail left.");
        #endregion



        #region OrderDetail error
        public static readonly Error OrderDetailNotFound = new("Error.OrderDetailNotFound", "Order detail not found.");
        public static readonly Error OrderDetailNull = new("Error.OrderDetailNull", "Order detail null");
        public static readonly Error LastOrderDetail = new("Error.LastOrderDetail", "Cannot delete the last product in the order.");
        #endregion



    }
    public class Error<T> : Error
    {
        public Error(string code, string message, T data) : base(code, message)
        {
            Data = data;
        }

        public T Data { get; }
    }
}
