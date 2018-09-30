using System;

namespace Teste
{
	class MainClass
	{
		public static void Main (string[] args)
		{



		}
	}
}
namespace Teste3
{
	public struct um
	{
		int a;
		string b;

		public um (int a, string b)
		{
			this.a = a;
			this.b = b;
		}

		public override string ToString ()
		{
			return base.ToString () + a + b;
		}
	}
}
