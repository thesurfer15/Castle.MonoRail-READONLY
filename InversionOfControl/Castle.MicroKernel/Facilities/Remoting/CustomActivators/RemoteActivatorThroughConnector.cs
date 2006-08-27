// Copyright 2004-2006 Castle Project - http://www.castleproject.org/
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

namespace Castle.Facilities.Remoting
{
	using System.Runtime.Remoting;
	
	using Castle.Core;
	using Castle.MicroKernel;
	using Castle.MicroKernel.ComponentActivator;
	

	public class RemoteActivatorThroughConnector : DefaultComponentActivator
	{
		public RemoteActivatorThroughConnector(ComponentModel model, IKernel kernel, ComponentInstanceDelegate onCreation, ComponentInstanceDelegate onDestruction) : base(model, kernel, onCreation, onDestruction)
		{
		}

		protected override object Instantiate(CreationContext context)
		{
			string uri = (string) Model.ExtendedProperties["remoting.uri"];
			
			RemotingRegistry registry = (RemotingRegistry) 
				Model.ExtendedProperties["remoting.remoteregistry"];
			
			registry.Publish(Model.Name);
				
			return RemotingServices.Connect(Model.Service, uri); 
		}
	}
}
