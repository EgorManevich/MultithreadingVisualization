using System;
using System.Diagnostics;

namespace MultithreadingVisualization {
	public static class StopwatchExtemsions {
		public static long ElapsedMilliseconds(this Stopwatch stopwatch) {
			return 1000 * stopwatch.ElapsedTicks / Stopwatch.Frequency;
		}

		public static long ElapsedMicroseconds(this Stopwatch stopwatch) {			
			return 1000000 * stopwatch.ElapsedTicks / Stopwatch.Frequency;
		}
	}
}