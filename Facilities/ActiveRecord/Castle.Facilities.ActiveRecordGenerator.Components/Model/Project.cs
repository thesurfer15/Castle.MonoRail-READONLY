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

namespace Castle.Facilities.ActiveRecordGenerator.Model
{
	using System;
	using System.Collections;


	public class Project
	{
		private String _name;
		private String _location;
		private String _driver;
		private String _connectionString;
		private String _namespace;
		private String _codeProvider;		
		private IList _activeRecordDescriptors = new ArrayList();
		private DatabaseDefinition _dbDefinition;

		public Project()
		{
		}
	}
}
