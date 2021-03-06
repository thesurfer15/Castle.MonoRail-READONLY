// Copyright 2004-2011 Castle Project - http://www.castleproject.org/
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

namespace Castle.MonoRail.Framework.Tests.Async
{
	using System;
	using System.Threading;

	public class ControllerWithAsyncAction : Controller
	{
		private readonly Output output;

		public ControllerWithAsyncAction()
		{
			output = LongOp;
		}

		public delegate string Output();

		public IAsyncResult BeginIndex()
		{
			return output.BeginInvoke(delegate { }, null);
		}

		private string LongOp()
		{
			Thread.Sleep(250);
			return "foo";
		}

		public void EndIndex()
		{
			var s = output.EndInvoke(ControllerContext.Async.Result);
			RenderText(s);
		}

		public IAsyncResult BeginCurrentCulture()
		{
			return output.BeginInvoke(delegate { }, null);
		}

		public void EndCurrentCulture()
		{
			output.EndInvoke(ControllerContext.Async.Result);
			RenderText(Thread.CurrentThread.CurrentCulture.Name);
		}
	}
}