// Copyright 2004-2010 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.MonoRail.Framework.Tests.JSON
{
	using Castle.MonoRail.Framework.Helpers;
	using Castle.MonoRail.Framework.Services;
	using Castle.MonoRail.Framework.Test;
	using NUnit.Framework;

	[TestFixture]
	public class JsonHelperTestCase
	{
		private JSONHelper helper;

		[SetUp]
		public void Init()
		{
			helper = new JSONHelper();

			var services = new StubMonoRailServices();
			services.JSONSerializer = new NewtonsoftJSONSerializer();
			helper.SetContext(new StubEngineContext(null, null, services, null));
		}

		[Test]
		public void Serialize()
		{
			var p = new Person("Json", 23);
			Assert.AreEqual("{\"Name\":\"Json\",\"Age\":23}", helper.ToJSON(p));
		}
	}
}
