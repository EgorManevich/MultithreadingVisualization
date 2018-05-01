using System.Diagnostics;

namespace MultithreadingVisualization {
	public static class StopwatchUtils {
		public static long TicksToMicroseconds(long elapsedTicks) {
			return 1000000 * elapsedTicks / Stopwatch.Frequency;
		}
	}
}
