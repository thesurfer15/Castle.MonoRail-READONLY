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

namespace Castle.MonoRail.Views.AspView.Compiler
{
	using System;
	using System.CodeDom.Compiler;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using Adapters;
	using Factories;

	/// <summary>
	/// Base class for aspview compilers. Two obvious derivations would be an OnlineCompiler, and an Offline compiler
	/// </summary>
	public abstract class AbstractCompiler : IDisposable
	{
		public readonly static string[] TemplateExtensions = new[] { ".aspx", ".master" };
		protected AspViewCompilerOptions options;
		protected readonly CompilerParameters parameters = new CompilerParameters();
		protected ICompilationContext context;
		protected IPreProcessor preProcessor;
		protected ICodeProviderAdapter codeProvider;
		const string AssemblyAttributeAllowPartiallyTrustedCallers = "[assembly: System.Security.AllowPartiallyTrustedCallers]";
		internal const string ViewSourceFileExtension = ".aspview.cs";

		protected AbstractCompiler(ICodeProviderAdapterFactory codeProviderAdapterFactory, IPreProcessor preProcessor, ICompilationContext context, AspViewCompilerOptions options)
		{
			this.preProcessor = preProcessor;
			this.context = context;
			this.options = options;
			codeProvider = codeProviderAdapterFactory.GetAdapter();
			InitialiseCompilerParameters(options.Debug);
		}

		/// <summary>
		/// Entry point for logic to apply after the pre-compilation, and before the 
		/// actual compilation
		/// </summary>
		protected virtual void AfterPreCompilation(List<SourceFile> files)
		{
		}

		/// <summary>
		/// Execute the compilation process using the precompilation steps and code provider
		/// </summary>
		/// <returns>CompilerResults with the compilation results</returns>
		/// <exception cref="Exception">Should compilation errors occur</exception>
		protected CompilerResults InternalExecute()
		{
			List<SourceFile> files = GetSourceFiles();
			if (files.Count == 0)
				return null;

			preProcessor.ApplyPreCompilationStepsOn(files);

			AfterPreCompilation(files);

			foreach (ReferencedAssembly reference in options.References)
			{
				string assemblyName = reference.Name;
				if (reference.Source == ReferencedAssembly.AssemblySource.BinDirectory)
					assemblyName = Path.Combine(context.BinFolder.FullName, assemblyName);
				parameters.ReferencedAssemblies.Add(assemblyName);
			}

			CompilerResults results = GetResultsFrom(files);

			ThrowIfErrorsIn(results);
			return results;
		}

		protected abstract CompilerResults GetResultsFrom(List<SourceFile> files);

		protected static string GetAllowPartiallyTrustedCallersFileContent()
		{
			return
@"// This file was generated by a tool
// Casle.MonoRail.Views.AspView compiler, version
" + AssemblyAttributeAllowPartiallyTrustedCallers;
		}


		void InitialiseCompilerParameters(bool debug)
		{
			parameters.GenerateExecutable = false;
			parameters.IncludeDebugInformation = debug;
		}

		List<SourceFile> GetSourceFiles()
		{
			var files = new List<SourceFile>();
			if (context.ViewRootDir.Exists == false)
				throw new Exception(string.Format("Could not find views folder [{0}]", context.ViewRootDir));

			foreach (string templateExtension in TemplateExtensions)
			{
				FileInfo[] templateFilenames = context.ViewRootDir.GetFiles("*" + templateExtension, SearchOption.AllDirectories);
				foreach (FileInfo fileInfo in templateFilenames)
				{
					string viewName = fileInfo.FullName.Replace(context.ViewRootDir.FullName, "");
					var file = new SourceFile
					{
						TemplateFullPath = fileInfo.FullName,
						ViewName = viewName,
						ClassName = AspViewEngine.GetClassName(viewName),
						ViewSource = File.ReadAllText(fileInfo.FullName)
					};
					file.RenderBody = file.ViewSource;
					files.Add(file);
				}
			}
			return files;

		}

		static void ThrowIfErrorsIn(CompilerResults results)
		{
			if (results.Errors.Count == 0) return;
			var shouldThrow = false;
			var message = new StringBuilder();
			message.AppendLine("AspView Compilation error:");
			foreach (CompilerError err in results.Errors)
			{
				if (err.IsWarning == false)
					shouldThrow = true;

				message.AppendFormat(@"
On [{0}] line#{1}, Column {2}, {3} {4}:
{5}
========================================",
												 err.FileName,
												 err.Line,
												 err.Column,
												 err.IsWarning ? "Warning" : "Error",
												 err.ErrorNumber,
												 err.ErrorText);
				message.AppendLine();
			}

			if (shouldThrow)
				throw new AspViewCompilationException(message.ToString());

			Console.WriteLine(message.ToString());
		}


		protected string SourceFileToFileName(SourceFile file)
		{
			return Path.Combine(context.TemporarySourceFilesDirectory.FullName, file.FileName);
		}

		protected static string SourceFileToSource(SourceFile file)
		{
			return file.ConcreteClass;
		}

		///<summary>
		///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		///</summary>
		public void Dispose()
		{
			if (codeProvider != null && codeProvider is IDisposable)
				((IDisposable)codeProvider).Dispose();
		}
	}

	/// <summary>
	/// Indicates a compilation error of the view templates
	/// </summary>
	public class AspViewCompilationException : Exception
	{
		///<summary>
		/// a new <see cref="AspViewCompilationException"/>
		///</summary>
		///<param name="messages">The compilation error messages</param>
		public AspViewCompilationException(string messages)
			: base(messages)
		{
		}
	}
}