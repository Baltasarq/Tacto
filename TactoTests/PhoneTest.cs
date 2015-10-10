using NUnit.Framework;
using System;
using Tacto.Core;

namespace TactoTests {
	[TestFixture]
	public class PhoneTest {
		[Test]
		public void TestFormatPhone()
		{
			var phoneInputs = new string[] {
				"",
				"988 23 45 66",
				"+34 (988) 34/56/78",
				"+34 (91) 45-67-78",
			};

			var phoneOutputs = new string[] {
				"",
				"988 234 566",
				"+34 988 345 678",
				"+3 491 456 778",
			};

			for (int i = 0; i < phoneInputs.Length; ++i) {
				Assert.AreEqual( phoneOutputs[ i ],
				                 Person.FormatPhoneNumber( phoneInputs[ i ] ) );
			}
		}
	}
}

