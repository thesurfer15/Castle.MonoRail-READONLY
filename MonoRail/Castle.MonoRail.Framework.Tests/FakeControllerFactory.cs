// Copyright 2004-2005 Castle Project - http://www.castleproject.org/
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

namespace Castle.MonoRail.Framework.Tests
{
	using System;

	using Castle.MonoRail.Engine;

	using Castle.MonoRail.Framework.Internal;
	using Castle.MonoRail.Framework.Tests.Controllers;

	public class FakeControllerFactory : IControllerFactory
	{
		public FakeControllerFactory()
		{
		}

		public Controller CreateController(UrlInfo info)
		{
			switch(info.Controller)
			{
				case "home":
				{
					return new HomeController();
				}
				case "filtered":
				{
					return new FilteredController();
				}
				case "filtered2":
				{
					return new Filtered2Controller();
				}
				case "exception":
				{
					return new ExceptionController();
				}
				case "layout":
				{
					return new LayoutController();
				}
				case "smart":
				{
					return new SmartController();
				}
				case "databind":
				{
					return new DataBindController();
				}
				case "resource":
				{
					return new ResourceController();
				}
				default:
				{
					throw new RailsException("Unknown controller");
				}
			}
		}

		public void Release(Controller controller)
		{

		}
	}
}
