using System;

namespace MessageSocket.TestAppClient
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Client");
			new ClientTest().TestAsync().Wait();

			Console.WriteLine("Hello World!");
		}
	}
}
