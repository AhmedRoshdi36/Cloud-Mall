using Cloud_Mall.Application.DTOs.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Mall.Application.Admin.Users.Query.GetAllAdminsBySuperAdminQuery;

public class GetAllAdminsBySuperAdminQuery:  IRequest<ApiResponse<List<UserDTO>>>
{
}
