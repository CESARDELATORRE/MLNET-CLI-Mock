using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MLNET.CLI.Mock
{
	class Program
	{
		static void Main(string[] args)
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			var cts = new CancellationTokenSource();
			Console.CancelKeyPress += (s, e) =>
			{
				e.Cancel = true;
				cts.Cancel();
			};

			MainAsync(args, cts.Token).GetAwaiter().GetResult();
		}

		static async Task MainAsync(string[] args, CancellationToken token)
		{
			string introMsg = @"Exploring multiple combinations of ML algorithms and settings to find you the best model for ML task: binary-classification";
			Console.WriteLine(introMsg);

			await Run(token);
			return;
		}

		private static async Task Run(CancellationToken token)
		{
			var experiment = new CLIExperimentMock();
			var requestToQuit = false;
			token.Register(() => requestToQuit = true);

			while (!requestToQuit)
			{
				Console.WriteLine();
				await experiment.Start(token);

				PrintExperimentResults();

				var c = Console.Read();
				if (c == 'q') break;
				//Console.Clear();
			}
		}

		public static void BusyWait(int milliseconds)
		{
			Thread.Sleep(milliseconds);
		}

		public static void PrintExperimentResults()
		{
			var defaultColor = Console.ForegroundColor;

			Console.WriteLine();
			Console.WriteLine("======================================================  EXPERIMENT RESULTS ==================================================");
			Console.WriteLine();
			Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------");
			Console.WriteLine("| ML Task: Binary classification    | Dataset: customer-reviews.tsv    | Label: Sentiment   | Time of experiment: 50 secs   |");
			Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------");
			Console.WriteLine("| Summary                                                                                                                   |");
			Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------");
			Console.WriteLine("| Total models explored: 28                                                                                                 |");
			Console.WriteLine("| Best model’s algorithm: AveragedPerceptronBinary                                                                          |");
			Console.WriteLine("| Accuracy of best model from validation data: 93%                                                                          |");
			Console.WriteLine("| Accuracy of best model on test data: 77%                                                                                  |");
			Console.WriteLine("| --------------------------------------------------------------------------------------------------------------------------|");
			Console.WriteLine("| Best five models explored                                                                                                 |");
			Console.WriteLine("| --------------------------------------------------------------------------------------------------------------------------|");
			Console.WriteLine("| Algorithm/Trainer                      | Accuracy    | AUC      | AUPRC    | F1-score  | Duration                         |");

			Console.Write("|");
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write(" 1  AveragedPerceptronBinary            | 93%         | 0.81     | 0.85     | 0.76      | 0.6                              ");
			Console.ForegroundColor = defaultColor;
			Console.Write("|\n");

			Console.WriteLine("| 2  SdcaBinary                          | 68%         | 0.77     | 0.79     | 0.72      | 0.3                              |");
			Console.WriteLine("| 3  LightGbmBinary                      | 63%         | 0.65     | 0.72     | 0.69      | 0.4                              |");
			Console.WriteLine("| 4  LightGbmBinary (*)                  | 68%         | 0.70     | 0.81     | 0.76      | 0.1                              |");
			Console.WriteLine("| 5  LinearSvmBinary                     | 68%         | 0.71     | 0.82     | 0.75      | 0.1                              |");
			Console.WriteLine("| --------------------------------------------------------------------------------------------------------------------------|");

			Console.Write("|");
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write(" Generated .zip model file for consumption: C:\\Customer-Reviews-Binary-Classification\\ModelLibrary\\model.zip               ");
			Console.ForegroundColor = defaultColor;
			Console.Write("|\n");

			Console.Write("|");
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write(" Generated C# code for model consumption: C:\\Customer-Reviews-Binary-Classification\\App.Predict                            ");
			Console.ForegroundColor = defaultColor;
			Console.Write("|\n");

			Console.Write("|");
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write(" Generated C# code for model training: C:\\Customer-Reviews-Binary-Classification\\App.Train                                 ");
			Console.ForegroundColor = defaultColor;
			Console.Write("|\n");

			Console.WriteLine("| --------------------------------------------------------------------------------------------------------------------------|");
			Console.WriteLine("| Log file for further details: C:\\Customer-Reviews-Binary-Classification\\mlnet-cli.log                                     |");
			Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------");


		}

	}
}
