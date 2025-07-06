using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Mall.Application.Admin.StoreManagement.Command.EnableStoreByAdmin;

public class EnableStoreByAdminCommand: IRequest<ApiResponse<bool>>
{
    public int StoreId { get; set; }
    public EnableStoreByAdminCommand(int storeId)
    {
        StoreId = storeId;
    }

}