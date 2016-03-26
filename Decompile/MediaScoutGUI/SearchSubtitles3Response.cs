using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;
using System.Xml.Serialization;

[GeneratedCode("System.ServiceModel", "4.0.0.0"), EditorBrowsable(EditorBrowsableState.Advanced), DebuggerStepThrough, MessageContract(WrapperName = "SearchSubtitles3Response", WrapperNamespace = "http://www.sublight.si/", IsWrapped = true)]
public class SearchSubtitles3Response
{
	[MessageBodyMember(Namespace = "http://www.sublight.si/", Order = 0)]
	public bool SearchSubtitles3Result;

	[MessageBodyMember(Namespace = "http://www.sublight.si/", Order = 1), XmlArrayItem(IsNullable = false)]
	public Subtitle[] subtitles;

	[MessageBodyMember(Namespace = "http://www.sublight.si/", Order = 2), XmlArrayItem(IsNullable = false)]
	public Release[] releases;

	[MessageBodyMember(Namespace = "http://www.sublight.si/", Order = 3)]
	public bool isLimited;

	[MessageBodyMember(Namespace = "http://www.sublight.si/", Order = 4)]
	public string error;

	public SearchSubtitles3Response()
	{
	}

	public SearchSubtitles3Response(bool SearchSubtitles3Result, Subtitle[] subtitles, Release[] releases, bool isLimited, string error)
	{
		this.SearchSubtitles3Result = SearchSubtitles3Result;
		this.subtitles = subtitles;
		this.releases = releases;
		this.isLimited = isLimited;
		this.error = error;
	}
}
