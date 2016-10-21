using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using UAParser;

namespace FilterIisLog.Test
{
	[TestFixture()]
	public class FilterIisTest
	{
		string _filePath = "gehs-2016-10-17-12.log";


		[Test]
		public void Read_GivenIisLogFile_ShouldReturnListOfHeaders()
		{
			// Arrange
			var fileLines = File.ReadAllLines(_filePath).ToList();
			var softwareTitleLine = fileLines[0];
			var headersLine = fileLines[1];

			// Act 
			var columnHeaders = GetColumnHeaders(headersLine);

			// Assert
			Assert.AreEqual(19, columnHeaders.Count);

			foreach (var column in columnHeaders)
			{
				Console.WriteLine(column);	
			}
		}

		[Test]
		public void ReadLine_GivenIisLogFile_ShouldReadFirstLineAsLogItem()
		{
			// Arrange
			var iisLogParser = CreateIisLogParser();

			// Act
			var firstLogItem = iisLogParser.Logs[0];

			// Assert
			Assert.AreEqual("2016-10-17", firstLogItem["date"]);
		}

		[Test]
		public void GetTop10SortedBrowsers_GivenIisLogFile_ShouldReturnTop10OrderedCounts()
		{
			// Arrange
			var iisLogParser = CreateIisLogParser();

			// Act
			var sortedBrowsers = iisLogParser.GetSortedBrowsers();

			var top10Browsers = sortedBrowsers.Take(10).ToList();

			// Assert
			var parser = Parser.GetDefault();
			foreach (var browser in top10Browsers)
			{
				var clientInfo = parser.Parse(browser.Key);
				Console.WriteLine("Browser: " + clientInfo.UserAgent.Family + " - Version: " + clientInfo.UserAgent.Major
								  + " Count: " + browser.Value + " Agent: " +browser.Key);
			}

			Assert.AreEqual(4803, top10Browsers[0].Value);
			Assert.AreEqual(1338, top10Browsers[1].Value);
			Assert.AreEqual(1244, top10Browsers[2].Value);
			Assert.AreEqual(844, top10Browsers[3].Value);
			Assert.AreEqual(684, top10Browsers[4].Value);
			Assert.AreEqual(673, top10Browsers[5].Value);
			Assert.AreEqual(655, top10Browsers[6].Value);
			Assert.AreEqual(398, top10Browsers[7].Value);
			Assert.AreEqual(378, top10Browsers[8].Value);
			Assert.AreEqual(302, top10Browsers[9].Value);


		}

		[Test]
		public void GetAllBrowsers_GivenIisLogFile_ShouldReturnAllBrowsers()
		{
			// Arrange
			var iisLogParser = CreateIisLogParser();

			// Act
			var allBrowsers = iisLogParser.GetAllBrowsers();

			// Assert
			Assert.AreEqual(85, allBrowsers.Count);
			Console.WriteLine("Count: " + allBrowsers.Count);
		}


		IisLogParser CreateIisLogParser()
		{
			var fileLines = File.ReadAllLines(_filePath).ToList();
			var dataLines = fileLines.Skip(2).ToList();
			var columnHeaders = GetColumnHeaders(fileLines[1]);
			return new IisLogParser(columnHeaders, dataLines);
		}

		List<string> GetColumnHeaders(string headersLine)
		{
			var headers = headersLine.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();

			headers = headers.Skip(1).ToList();

			return headers;
		}
}
}
