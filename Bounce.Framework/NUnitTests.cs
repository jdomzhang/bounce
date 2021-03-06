﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Bounce.Framework
{
    public class NUnitTests : Task
    {
        /// <summary>
        /// List of paths to test DLLs
        /// </summary>
        [Dependency] public IEnumerable<Task<string>> DllPaths;

        /// <summary>
        /// Full path to nunit-console.exe
        /// </summary>
        [Dependency] public Task<string> NUnitConsolePath;

        /// <summary>
        /// Categories to include in the test run.
        /// </summary>
        [Dependency] public Task<IEnumerable<string>> IncludeCategories;

        /// <summary>
        /// Categories to exclude in the test run.
        /// </summary>
        [Dependency] public Task<IEnumerable<string>> ExcludeCategories;

        public NUnitTests()
        {
            NUnitConsolePath = @"c:\Program Files (x86)\NUnit\nunit-console.exe";
            ExcludeCategories = new string[0];
            IncludeCategories = new string[0];
        }

        public override void Build(IBounce bounce)
        {
            IEnumerable<string> testDlls = DllPaths.Select(dll => dll.Value);
            string joinedTestDlls = "\"" + String.Join("\" \"", testDlls.ToArray()) + "\"";

            bounce.Log.Info("running unit tests for: " + joinedTestDlls);

            var args = new[]
            {
                Excludes,
                Includes,
                joinedTestDlls,
            };

            bounce.ShellCommand.ExecuteAndExpectSuccess(NUnitConsolePath.Value, String.Join(" ", args));
        }

        string Excludes
        {
            get
            {
                return GetIncludeExcludeArgument("exclude", ExcludeCategories);
            }
        }

        string Includes
        {
            get {
                return GetIncludeExcludeArgument("include", IncludeCategories);
            }
        }

        private string GetIncludeExcludeArgument(string argumentName, Task<IEnumerable<string>> categories)
        {
            if (categories.Value.Count() > 0)
            {
                return "/" + argumentName + "=" + String.Join(",", categories.Value.ToArray());
            } else
            {
                return "";
            }
        }
    }
}