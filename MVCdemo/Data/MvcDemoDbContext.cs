using Microsoft.EntityFrameworkCore;
using MVCdemo.Models.Domain;

namespace MVCdemo.Data
{
	public class MvcDemoDbContext : DbContext
	{
		public MvcDemoDbContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Employee> Employees { get; set; }
	}
}
