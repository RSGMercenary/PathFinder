using PathFinder.Processors.Abstracts;
using PathFinder.Processors.Datas;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace PathFinder.Processors.Converters
{
	public class SamlConverter : Converter
	{
		public override ProcessType ProcessType => ProcessType.ConvertSaml;

		protected override string ConvertTo(string value)
		{
			var bytes = Convert.FromBase64String(value);
			using(var output = new MemoryStream())
			{
				using(var stream = new DeflateStream(new MemoryStream(bytes), CompressionMode.Decompress))
					stream.CopyTo(output, bytes.Length);
				return Encoding.UTF8.GetString(output.ToArray());
			}
		}

		protected override string ConvertFrom(string value)
		{
			var bytes = Encoding.UTF8.GetBytes(value);
			using(var output = new MemoryStream())
			{
				using(var stream = new DeflateStream(output, CompressionMode.Compress))
					stream.Write(bytes, 0, bytes.Length);
				return Convert.ToBase64String(output.ToArray());
			}
		}
	}
}