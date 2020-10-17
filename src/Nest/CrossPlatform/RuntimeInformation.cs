using System;
using System.Linq;
#if NET461
using System.Reflection;

namespace Nest6
{
	internal static class RuntimeInformation
	{
		private static string _frameworkDescription;
		private static string _osDescription;

		public static string FrameworkDescription
		{
			get
			{
				if (_frameworkDescription == null)
				{
					var assemblyFileVersionAttribute =
						((AssemblyFileVersionAttribute[])Attribute.GetCustomAttributes(
							typeof(object).GetTypeInfo().Assembly,
							typeof(AssemblyFileVersionAttribute)))
						.OrderByDescending(a => a.Version)
						.First();
					_frameworkDescription = $".NET Framework {assemblyFileVersionAttribute.Version}";
				}
				return _frameworkDescription;
			}
		}

		public static string OSDescription
		{
			get
			{
				if (_osDescription == null)
				{
					var platform = (int)Environment.OSVersion.Platform;
					var isWindows = platform != 4 && platform != 6 && platform != 128;
					if (isWindows)
						_osDescription = NativeMethods.Windows.RtlGetVersion() ?? "Microsoft Windows";
					else
						_osDescription = Environment.OSVersion.VersionString;
				}
				return _osDescription;
			}
		}
	}
}
#endif
