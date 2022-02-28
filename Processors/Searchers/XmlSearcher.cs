using PathFinder.Models;
using PathFinder.Processors.Abstracts;
using PathFinder.Processors.Datas;
using PathFinder.Processors.Interfaces;
using PathFinder.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PathFinder.Processors.Searchers
{
	public class XmlSearcher : Searcher, IFormatterProcessor, IInfoProcessor
	{
		public override ProcessType ProcessType => ProcessType.SearchXml;
		public override int Priority => 1;
		public string InfoUrl => "https://www.w3.org/TR/1999/REC-xpath-19991116/";

		public string Format(string input) => Xml.Format(input);

		protected override IEnumerable<ProcessResult> Processing(IPageModel state)
		{
			try
			{
				var xml = Xml.Get(state.Input);
				try
				{
					return xml.SelectNodes(state.Parameter).Cast<XmlNode>().Select(x => GetOutput(x, state.FormData));
				}
				catch { throw new ProcessorException(ProcessPhase.Parameters); }
			}
			catch { throw new ProcessorException(ProcessPhase.Input); }
		}

		private ProcessResult GetOutput(XmlNode node, bool formData)
		{
			if(node is XmlAttribute)
				return new ProcessResult(node.InnerText, "");
			if(node.LocalName.ToLower().Contains("form") && formData)
				return new ProcessResult(Xml.Format(node), GetFormData(node));
			return new ProcessResult(Xml.Format(node), node.InnerText);
		}

		private string GetFormData(XmlNode form)
		{
			var fields = GetFormFields(form).ToList();
			int length = 0;
			int count = 0;
			foreach((string key, string value) in fields)
			{
				++count;
				if(key.Length > length)
					length = key.Length;
			}
			var builder = new StringBuilder();
			foreach((string key, string value) in fields)
			{
				builder.Append($"{key.PadLeft(length)} = {value}");
				if(--count != 0)
					builder.Append(Environment.NewLine);
			}
			return builder.ToString();
		}

		private IEnumerable<(string, string)> GetFormFields(XmlNode form)
		{
			foreach(var input in GetFields(form, "input").Select(i => GetFieldValues(i, i)))
				yield return input;
			foreach(var select in GetFields(form, "select").Select(i => GetFieldValues(i, i.SelectSingleNode(".//*[local-name()='opton' and @selected]"))))
				yield return select;
		}

		private IEnumerable<XmlNode> GetFields(XmlNode form, string name) => form.SelectNodes($".//*[local-name()='{name}']").Cast<XmlNode>();

		private (string, string) GetFieldValues(XmlNode key, XmlNode value)
		{
			return (key.Attributes["name"]?.InnerText ?? "", (value?.Attributes["value"] ?? value)?.InnerText ?? "");
		}
	}
}