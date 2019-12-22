using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Data;

namespace Todo.Client.Data
{
    public class TaskService
    {

        private TodotaskContext _ctx;

        public TaskService(TodotaskContext ctx)
        {
            _ctx = ctx;
        }

        public Task<List<Todo.Data.Task>> GetAll(int? userid = null)
        {
            if (userid != null)
                return System.Threading.Tasks.Task.FromResult(_ctx.Tasks.Include("User").Where(m => m.UserId == userid).ToList());
            return System.Threading.Tasks.Task.FromResult(_ctx.Tasks.Include("User").ToList());
        }

        public async Task<Todo.Data.Task> Add(Todo.Data.Task task)
        {
            task.Status = "not started";
            _ctx.Entry(task).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            await _ctx.SaveChangesAsync();

            return task;
        }

        public async Task<Todo.Data.Task> UpdateStatus(int id, string status)
        {
            Todo.Data.Task task = _ctx.Tasks.FirstOrDefault(m => m.Id == id);
            task.Status = status;
            _ctx.Entry(task).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _ctx.SaveChangesAsync();

            return task;
        }

    }
}
