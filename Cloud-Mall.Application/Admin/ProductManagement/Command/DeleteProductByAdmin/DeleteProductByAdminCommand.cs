using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Mall.Application.Admin.ProductManagement.Command.DeleteProductByAdmin
{
    public class DeleteProductByAdminCommand : IRequest<ApiResponse<bool>>
    {
        public int ProductId { get; set; }
        public DeleteProductByAdminCommand(int productId)
        {
            ProductId = productId;
        }

    }
}
