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

namespace Castle.MonoRail.Views.AspView.Compiler.PreCompilationSteps
{
	public class ChainingStep : IPreCompilationStep
	{
		private readonly IPreCompilationStep[] internalSteps;

		public ChainingStep(IPreCompilationStep[] internalSteps)
		{
			this.internalSteps = internalSteps;
		}

		public virtual void Process(SourceFile file)
		{
			foreach (var step in internalSteps)
			{
				step.Process(file);
			}
		}
	}
}
