﻿using System.Threading.Tasks;
using Elastic.Xunit.XunitPlumbing;
using Nest6;
using Tests.Framework;
using static Tests.Framework.UrlTester;

namespace Tests.XPack.License.GetLicense
{
	public class GetLicenseUrlTests : UrlTestsBase
	{
		[U] public override async Task Urls() => await GET("/_xpack/license")
			.Fluent(c => c.GetLicense())
			.Request(c => c.GetLicense(new GetLicenseRequest()))
			.FluentAsync(c => c.GetLicenseAsync())
			.RequestAsync(c => c.GetLicenseAsync(new GetLicenseRequest()));
	}
}
