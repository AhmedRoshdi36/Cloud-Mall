using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Mall.Application.Admin.StoreManagement.Command.DeleteStoreByAdmin
{
    public class DeleteStoreByAdminCommand : IRequest<ApiResponse<bool>>
    {
        public int StoreId { get; set; }
        public DeleteStoreByAdminCommand(int storeId)
        {
            StoreId = storeId;
        }
    }
}
