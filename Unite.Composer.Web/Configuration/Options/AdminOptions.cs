using System;

namespace Unite.Composer.Web.Configuration.Options
{
	public class AdminOptions
	{
		public string User => Environment.GetEnvironmentVariable("UNITE_ADMIN_USER");

		public string Password => Environment.GetEnvironmentVariable("UNITE_ADMIN_PASSWORD");
	}
}
