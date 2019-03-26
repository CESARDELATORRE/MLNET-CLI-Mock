using System.Threading;
using System.Threading.Tasks;

namespace MLNET.CLI.Mock
{
	public interface IProgressBarExperiment
	{
		Task Start(CancellationToken token);
	}
}
