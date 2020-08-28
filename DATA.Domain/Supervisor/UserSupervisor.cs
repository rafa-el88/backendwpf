using DATA.Domain.Models;
using DATA.Domain.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DATA.Domain.Supervisor
{
    public class UserSupervisor
    {
        private AppDbContext _ctx;
        public UserSupervisor(AppDbContext appDBContext)
        {
            _ctx = appDBContext;
        }

        public List<User> toList(ref ViewPagination pag)
        {
            pag.Count = _ctx.Users.Count();
            return _ctx.Users.Skip(pag.StartAt()).Take(pag.OffSet).AsNoTracking().ToList();
        }

        public User Find(int Id)
        {
            return _ctx.Users.Find(Id);
        }

        public bool Remove(int Id)
        {
            _ctx.Remove(_ctx.Users.Find(Id));
            return _ctx.SaveChanges() != 0;
        }

        public int Add(User usr)
        {
            _ctx.Add(usr);
            return _ctx.SaveChanges();
        }

        public int Update(User usr)
        {
            User tmpUser = _ctx.Users.Where(w => w.Id == usr.Id).AsNoTracking().FirstOrDefault();
            _ctx.Entry(usr).State = EntityState.Modified;
            _ctx.Update(usr);
            return _ctx.SaveChanges();
        }
    }
}
