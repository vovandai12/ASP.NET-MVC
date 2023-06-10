using ASPNETMVCCRUD.Data;
using ASPNETMVCCRUD.Models;
using ASPNETMVCCRUD.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNETMVCCRUD.Controllers
{
    public class EmpolyeeController : Controller
    {
        private readonly MVCDbContext mvcContext;

        public EmpolyeeController(MVCDbContext mvcContext)
        {
            this.mvcContext = mvcContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var empolyees = await mvcContext.Employees.ToListAsync();
            return View(empolyees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmpolyeeViewModel model)
        {
            var empolyee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Email = model.Email,
                Salary = model.Salary,
                DateOfBirth = model.DateOfBirth,
                Department = model.Department
            };

            await mvcContext.Employees.AddAsync(empolyee);
            await mvcContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var empolyee = await mvcContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if(empolyee != null) { 
                var viewModel = new UpdateEmpolyeeViewModel()
                {
                    Id = empolyee.Id,
                    Name = empolyee.Name,
                    Email = empolyee.Email,
                    Salary = empolyee.Salary,
                    DateOfBirth = empolyee.DateOfBirth,
                    Department = empolyee.Department
                };
                return await Task.Run(() => View("View", viewModel));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmpolyeeViewModel model)
        {
            var empolyee = await mvcContext.Employees.FindAsync(model.Id);
            if(empolyee != null)
            {
                empolyee.Name = model.Name;
                empolyee.Email = model.Email;
                empolyee.Salary = model.Salary;
                empolyee.DateOfBirth = model.DateOfBirth;
                empolyee.Department = model.Department;

                await mvcContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmpolyeeViewModel model)
        {
            var empolyee = await mvcContext.Employees.FindAsync(model.Id);
            if (empolyee != null)
            {
                mvcContext.Employees.Remove(empolyee);
                await mvcContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

    }
}
