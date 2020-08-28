using DATA.Domain.Models;
using DATA.Domain.Supervisor;
using DATA.Domain.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private UserSupervisor _usr;

        public UserController(AppDbContext context)
        {
            _context = context;
            _usr = new UserSupervisor(_context);
        }

        // GET: api/User
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers([FromQuery] int? page = 0, [FromQuery] int? offset = 10)
        {
            try
            { 
                if (page < 0)
                    page = 0;
                ViewPagination _pag = new ViewPagination();
                _pag.Page = (int)page;
                _pag.OffSet = (int)offset;

                var _ret = _usr.toList (ref _pag);
                _pag.Page += 1;
                Response.Headers.Add("X-Total", _pag.Count.ToString());
                Response.Headers.Add("X-Per-Page", _pag.OffSet.ToString());
                Response.Headers.Add("X-Page", _pag.Page.ToString());
                Response.Headers.Add("X-Total-Pages", _pag.TotalPages().ToString());
                return Ok(_ret);
            }
            catch (Exception e)
            {
                return BadRequest(string.Format("Error during load list{0}{1}", Environment.NewLine, e.Message));
            }
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserByid(int id)
        {
            var user = _usr.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            } 

            try
            {
                _usr.Update(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/User
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _usr.Add(user);
            return Ok();
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            } 
            _usr.Remove(user.Id); 
            return user;
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
