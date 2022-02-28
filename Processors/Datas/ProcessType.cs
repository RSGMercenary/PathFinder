using System.ComponentModel;

namespace PathFinder.Processors.Datas
{
	public enum ProcessType
	{
		None,
		[Category("SEARCH"), Description("AUTO")]
		Search,
		[Category("SEARCH"), Description("XML")]
		SearchXml,
		[Category("SEARCH"), Description("JSON")]
		SearchJson,
		[Category("SEARCH"), Description("REGEX")]
		SearchRegex,
		[Category("SEARCH"), Description("TEXT")]
		SearchText,
		[Category("SEARCH"), Description("DELIMIT")]
		SearchDelimiter,
		[Category("CONVERT"), Description("HTML")]
		ConvertHtml,
		[Category("CONVERT"), Description("URL")]
		ConvertUrl,
		[Category("CONVERT"), Description("HEX")]
		ConvertHex,
		[Category("CONVERT"), Description("JWT")]
		ConvertJwt,
		[Category("CONVERT"), Description("ESCAPE")]
		ConvertEscape,
		[Category("CONVERT"), Description("BASE 64")]
		ConvertBase64,
		[Category("CONVERT"), Description("SAML")]
		ConvertSaml,
		[Category("CONVERT"), Description("BINARY")]
		ConvertBinary
	}
}