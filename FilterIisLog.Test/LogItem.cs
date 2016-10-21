using System.Collections.Generic;

namespace FilterIisLog.Test
{
	public class LogItem
	{
		List<string> columnHeaders;
		string line;

		public Dictionary<string, string> LogDictionary { get; private set; }

		public LogItem(List<string> columnHeaders, string line)
		{
			this.columnHeaders = columnHeaders;
			this.line = line;
			FillLogDictionary(columnHeaders, line);
		}


		public string this[string column]
		{
			get
			{
				return LogDictionary[column];
			}
		}

		void FillLogDictionary(List<string> columnHeaders, string line)
		{
			var elementDictionary = new Dictionary<string, string>();
			var dataElements = line.Split(new[] { " " }, System.StringSplitOptions.RemoveEmptyEntries);

			for (int i = 0; i < columnHeaders.Count; i++)
			{
				var column = columnHeaders[i];
				var dataElement = dataElements[i];
				elementDictionary.Add(column, dataElement);
			}

			LogDictionary = elementDictionary;
		}

	}
}