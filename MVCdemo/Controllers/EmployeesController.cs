using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MVCdemo.Data;
using MVCdemo.Models;
using MVCdemo.Models.Domain;

namespace MVCdemo.Controllers
{
	public class EmployeesController : Controller
	{
		private readonly MvcDemoDbContext _mvcDemoDbConext;

		public EmployeesController(MvcDemoDbContext mvcDemoDbConext)
		{
			_mvcDemoDbConext = mvcDemoDbConext;
		}

		[HttpGet]
		public IActionResult Add()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var employees = await _mvcDemoDbConext.Employees.ToListAsync();

			return View(employees);
		}


		[HttpPost]
		public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
		{
			var employee = new Employee()
			{
				Id = Guid.NewGuid(),
				Name = addEmployeeRequest.Name,
				Email= addEmployeeRequest.Email,
				Salary= addEmployeeRequest.Salary,
				DateOfBirth= addEmployeeRequest.DateOfBirth,
				Department= addEmployeeRequest.Department
			};

			await _mvcDemoDbConext.Employees.AddAsync(employee);
			await _mvcDemoDbConext.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> View(Guid id)
		{

			var employee = await _mvcDemoDbConext.Employees.FirstOrDefaultAsync(x => x.Id == id);

			if (employee != null)
			{
				var viewModel = new UpdateEmployeeViewModel()
				{
					Id = employee.Id,
					Name = employee.Name,
					Email = employee.Email,
					Salary = employee.Salary,
					DateOfBirth = employee.DateOfBirth,
					Department = employee.Department

				};

				return await Task.Run(() => View("View", viewModel));
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> View(UpdateEmployeeViewModel updatedEmployee)
		{
			var employee = await _mvcDemoDbConext.Employees.FindAsync(updatedEmployee.Id);

			if (employee != null)
			{
				employee.Name= updatedEmployee.Name;
				employee.Email= updatedEmployee.Email;
				employee.Salary= updatedEmployee.Salary;
				employee.DateOfBirth= updatedEmployee.DateOfBirth;
				employee.Department= updatedEmployee.Department;

				await _mvcDemoDbConext.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			return await Task.Run(() => View("View", updatedEmployee));
		}

		[HttpPost]
		public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
		{
			var employee = await _mvcDemoDbConext.Employees.FindAsync(model.Id);

			if (employee != null)
			{
				_mvcDemoDbConext.Employees.Remove(employee);
				await _mvcDemoDbConext.SaveChangesAsync();
			}

			return RedirectToAction("Index");
		}
	}
}
