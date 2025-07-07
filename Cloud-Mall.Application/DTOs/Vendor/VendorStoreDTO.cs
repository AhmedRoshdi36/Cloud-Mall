using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Mall.Application.DTOs.Vendor;

public class VendorStoreDTO
{
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string CategoryName { get; set; }
    public DateTime CreatedAt { get; set; }
}
