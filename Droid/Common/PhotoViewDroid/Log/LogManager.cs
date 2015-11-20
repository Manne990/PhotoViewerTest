using System;

namespace PhotoViewerTest.Droid
{
	public sealed class LogManager
	{
		private static ILogger logger = new LoggerDefault();

		public static void SetLogger(ILogger newLogger) {
			logger = newLogger;
		}

		public static ILogger GetLogger() {
			return logger;
		}
	}
}

