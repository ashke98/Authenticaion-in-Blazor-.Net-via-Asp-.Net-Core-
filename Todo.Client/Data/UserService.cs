using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Todo.Data;

namespace Todo.Client.Data
{
    public class UserService
    {
        private TodotaskContext _ctx;

        public UserService(TodotaskContext ctx)
        {
            _ctx = ctx;
        }

        public Task<List<Todo.Data.User>> GetAll()
        {
            return System.Threading.Tasks.Task.FromResult(_ctx.Users.ToList());
        }

        public int GetUserId(ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            principal.FindFirstValue(ClaimTypes.NameIdentifier);
            return Int32.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        public Task<Todo.Data.User> Get(int id)
        {
            return System.Threading.Tasks.Task.FromResult(_ctx.Users.FirstOrDefault(m => m.Id == id));
        }


    }
}
