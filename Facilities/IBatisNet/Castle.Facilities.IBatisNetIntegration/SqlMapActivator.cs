#region Licence
/// Copyright 2004-2005 Castle Project - http://www.castleproject.org/
///  
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///  
/// http://www.apache.org/licenses/LICENSE-2.0
///  
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// 
/// -- 
/// 
/// This facility was a contribution kindly 
/// donated by Gilles Bayon <gilles.bayon@gmail.com>
/// 
/// --
#endregion

#region Using
using System;

using Castle.Model;

using Castle.MicroKernel;
using Castle.MicroKernel.ComponentActivator;

using IBatisNet.DataMapper;
#endregion

namespace Castle.Facilities.IBatisNetIntegration
{
	/// <summary>
	/// Summary description for SqlMapFactoryActivator.
	/// </summary>
	public class SqlMapActivator : AbstractComponentActivator
	{
		public SqlMapActivator(ComponentModel model, 
			IKernel kernel, ComponentInstanceDelegate onCreation, 
			ComponentInstanceDelegate onDestruction) : 
			base(model, kernel, onCreation, onDestruction)
		{
		}

		protected override object InternalCreate()
		{
			string fileName = (string) Model.ExtendedProperties[ IBatisNetFacility.FILE_CONFIGURATION ];

			return SqlMapper.Configure(fileName);
		}

		protected override void InternalDestroy(object instance)
		{
		}
	}
}
