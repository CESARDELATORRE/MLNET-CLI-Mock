using System;
using System.Threading;
using System.Threading.Tasks;

using ShellProgressBar;

namespace MLNET.CLI.Mock
{
    public class CLIExperimentMock : IProgressBarExperiment
	{
		private bool RequestToQuit { get; set; }

		protected void Start()
        {
			//Assume one tick per second
			const int totalTicks = 50;
            var options = new ProgressBarOptions
            {
                ForegroundColor = ConsoleColor.Yellow,
	            ForegroundColorDone = ConsoleColor.Yellow,
	            BackgroundColor = ConsoleColor.DarkGray,
	            BackgroundCharacter = '\u2593'
            };
            using (var pbar = new ProgressBar(totalTicks, "-- PLACEHOLDER --", options))
            {
                TickToCompletion(pbar, totalTicks, sleep: 1000);
            }
        }

		protected void TickToCompletion(IProgressBar pbar, int ticks, int sleep = 1000, Action childAction = null)
		{
			var initialMessage = pbar.Message;
			double bestAccuracy1 = 0.61;
			string bestAlgorithm1 = "LogisticRegression";
			string nextAlgorithm1 = "SdcaBinary";

			double bestAccuracy2 = 0.79;
			string bestAlgorithm2 = "SdcaBinary";
			string nextAlgorithm2 = "AveragedPerceptronBinary";

			string msg0 = $"done – Best accuracy: <na>     – Best Algorithm: <na> – Next Algorithm: {bestAlgorithm1}";
			string msg1 = $"done – Best accuracy: {(bestAccuracy1 * 100).ToString()}% – Best Algorithm: {bestAlgorithm1} – Next Algorithm: {nextAlgorithm1}";
			string msg2 = $"done – Best accuracy: {(bestAccuracy2 * 100).ToString()}% – Best Algorithm: {bestAlgorithm2} – Next Algorithm: {nextAlgorithm2}";
			string msgFinal = $"done – Best accuracy: {(0.93 * 100).ToString()}% – Best Algorithm: {"AveragedPerceptronBinary"} ";

			string currentMsg = msg0;
			pbar.Message = currentMsg;

			int msgTurn = 1;
			for (var i = 0; i < ticks && !RequestToQuit; i++)
			{
				if(i > 4)
				{ 
					if ((i % 5) == 0) //If divisible by 5, update content
					{
						if (msgTurn == 1)
						{
							currentMsg = msg1;
							msgTurn = 0;
						}
						else
						{
							currentMsg = msg2;
							msgTurn = 1;
						}

						pbar.Message = currentMsg;
						childAction?.Invoke();
					}
				}

				Thread.Sleep(sleep);
				pbar.Tick(currentMsg);
			}
			//Final message
			pbar.Tick(msgFinal);
		}

		public Task Start(CancellationToken token)
		{
			RequestToQuit = false;
			token.Register(() => RequestToQuit = true);

			this.Start();
			return Task.FromResult(1);
		}
	}
}
