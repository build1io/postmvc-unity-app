using System;

namespace Build1.PostMVC.Unity.App.Modules.InternetReachability
{
	public sealed class NoInternetException : Exception
	{
		public NoInternetException() : base("No internet connection")
		{
		}
	}
}