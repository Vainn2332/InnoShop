using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.ApplicationLayer.Interfaces
{
    public interface IAdminService
    {
        public Task DeactivateUserAsync(int id);
        public Task ActivateUserAsync(int id);
    }
}
