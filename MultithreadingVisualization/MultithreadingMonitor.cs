using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MultithreadingVisualization {
	public class MultithreadingTestRunner {
		private const int ObservationTimePerThreadMicroseconds = 1000;
		private const int GranularityMicroseconds = 5;
		private const int MaxMeaturementsPerThreadCount = ObservationTimePerThreadMicroseconds / GranularityMicroseconds;
		private const int ThreadsCount = 9;

		public Task<(long totalMicroseconds, (long initialMicroseconds, int[] segments)[] results)> Run() {
			Stopwatch sw = new Stopwatch();

			var tasks = Enumerable.Range(0, ThreadsCount).Select(x => new Task<(long initialMicroseconds, int[] segments)>(() => {
				var segments = new List<int>(MaxMeaturementsPerThreadCount);				

				//should be recorded right before meaturement loop
				var initialMicroseconds = sw.ElapsedMicroseconds();
				var prevMicroseconds = initialMicroseconds;
				while (sw.ElapsedMicroseconds() - initialMicroseconds < ObservationTimePerThreadMicroseconds) {
					//delay
					while (sw.ElapsedMicroseconds() - prevMicroseconds < GranularityMicroseconds) { }

					var currentMicroseconds = sw.ElapsedMicroseconds();
					segments.Add((int)(currentMicroseconds - prevMicroseconds));
					prevMicroseconds = currentMicroseconds;
				}

				return (initialMicroseconds, segments.ToArray());
			})).ToArray();

			//should be launched right before start of the tasks
			sw.Start();
			
			foreach (var task in tasks) {
				task.Start();
			}

			return Task.WhenAll(tasks).ContinueWith(x => {
				sw.Stop();
				return (sw.ElapsedMicroseconds(), x.Result);
			});
		}
	}
}