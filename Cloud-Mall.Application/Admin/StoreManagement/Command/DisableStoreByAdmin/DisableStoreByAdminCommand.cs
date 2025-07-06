using Cloud_Mall.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Mall.Application.Admin.StoreManagement.Command.DisableStoreByAdmin;

public class DisableStoreByAdminCommand : IRequest<ApiResponse<bool>>
{
    public int StoreId { get; set; }
    public DisableStoreByAdminCommand(int storeId)
    {
        StoreId = storeId;
    }
}
