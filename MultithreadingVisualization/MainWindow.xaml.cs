using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MultithreadingVisualization {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow {
		private readonly SolidColorBrush[] _brushes = new[]
		{
			new SolidColorBrush(Colors.Red),
			new SolidColorBrush(Colors.Blue),
			new SolidColorBrush(Colors.Yellow),
			new SolidColorBrush(Colors.Magenta),
			new SolidColorBrush(Colors.Green)
		};

		private const int RectangleHeight = 50;

		public MainWindow() {
			InitializeComponent();
		}

		private async void RefreshButton_Click(object sender, RoutedEventArgs e) {
			var multithreadingMonitor = new MultithreadingTestRunner();
			var testResult = await multithreadingMonitor.Run()/*.ConfigureAwait(false)*/;
			var totalMicroseconds = testResult.totalMicroseconds;
			var results = testResult.results;
			var totalWidth = Window.ActualWidth;

			ThreadCanvas.Children.Clear();
			for (int i = 0; i < results.Length; i++) {
				var brush = _brushes[i % _brushes.Length];
				var initialRect = new Rectangle {
					Fill = brush,
					Height = RectangleHeight
				};
				var canvasTop = i * RectangleHeight;
				Canvas.SetLeft(initialRect, totalWidth * results[i].initialMicroseconds / totalMicroseconds);
				Canvas.SetTop(initialRect, canvasTop);
				ThreadCanvas.Children.Add(initialRect);

				var prevMicroseconds = results[i].initialMicroseconds;
				foreach (var segment in results[i].segments) {
					var rect = new Rectangle {
						Fill = brush,
						Height = RectangleHeight
					};
					var currentMicroseconds = prevMicroseconds + segment;
					Canvas.SetLeft(rect, totalWidth * currentMicroseconds / totalMicroseconds);
					Canvas.SetTop(rect, canvasTop);
					ThreadCanvas.Children.Add(rect);
					prevMicroseconds = currentMicroseconds;
				}
			}
		}

		private void MagnifyButton_Click(object sender, RoutedEventArgs e)
		{
			ThreadCanvasTransform.ScaleX *= 2;			
		}

		private void RefuceButton_Click(object sender, RoutedEventArgs e) {
			ThreadCanvasTransform.ScaleX *= 0.5;
		}
	}
}