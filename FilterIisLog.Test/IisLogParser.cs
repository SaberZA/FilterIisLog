using System;
using System.Collections.Generic;
using System.Linq;

namespace FilterIisLog.Test
{
	class IisLogParser
	{
		List<string> columnHeaders;
		List<string> dataLines;

		public List<LogItem> Logs { get; private set;}

		public IisLogParser(List<string> columnHeaders, List<string> dataLines)
		{
			this.columnHeaders = columnHeaders;
			this.dataLines = dataLines;


			ParseLogs();
		}

		void ParseLogs()
		{
			Logs = new List<LogItem>();
			foreach (var line in dataLines)
			{
				var logItem = new LogItem(columnHeaders, line);
				Logs.Add(logItem);
			}
		}

		public List<KeyValuePair<string,int>> GetSortedBrowsers()
		{
			var browserInstances = new Dictionary<string, int>();

			foreach (var log in Logs)
			{
				var userAgent = log["cs(User-Agent)"];
				if (browserInstances.ContainsKey(userAgent))
				{
					browserInstances[userAgent] = browserInstances[userAgent] + 1;
				}
				else
				{
					browserInstances.Add(userAgent, 1);
				}

			}

			var dictionaryList = browserInstances.ToList();

			return dictionaryList.OrderByDescending(p=>p.Value).ToList();
		}

		public List<string> GetAllBrowsers()
		{
			var browserList = new List<string>();

			foreach (var log in Logs)
			{
				var userAgent = log["cs(User-Agent)"];
				browserList.Add(userAgent);
			}

			browserList = browserList.Distinct(new DistinctComparer()).ToList();

			return browserList;
		}
	}
}