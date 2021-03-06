﻿using System;
using System.IO;

namespace Bounce.Framework {
    public class TeamCityNUnitLogger : ICommandLog {
        private readonly TextWriter Output;
        private readonly ICommandLog Log;
        private TeamCityFormatter TeamCityFormatter;

        public TeamCityNUnitLogger(string args, TextWriter output, ICommandLog log) {
            Output = output;
            Log = log;
            CommandArgumentsForLogging = args;
            TeamCityFormatter = new TeamCityFormatter();
        }

        public void CommandOutput(string output) {
            Log.CommandOutput(output);
        }

        public void CommandError(string error) {
            Log.CommandError(error);
        }

        public void CommandComplete(int exitCode) {
            var resultsPath = Path.GetFullPath("TestResult.xml");
            Output.WriteLine(TeamCityFormatter.FormatTeamCityMessage("importData", "type", "nunit", "path", resultsPath));
            Log.CommandComplete(exitCode);
        }

        public string CommandArgumentsForLogging { get; private set; }
    }
}