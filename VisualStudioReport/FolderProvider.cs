using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.SccIntegration;
using System.Threading;
using System.Threading.Tasks;

namespace VisualStudioReport
{
    public class FolderProvider : IVsSccFolderProvider
    {
        public string RootFolder { get; private set; }

        public FolderProvider(string rootFolder)
        {
            RootFolder = rootFolder;
        }

        public void Dispose()
        {
            if (RootFolder != null)
            {
                RootFolder = null;
            }
        }

        public Task<SccItemStatus> GetSccStatusAsync(string absolutePath, CancellationToken cancellationToken)
        {
            return Task.FromResult(new SccItemStatus(__SccStatus.SCC_STATUS_CHECKEDOUT, VsStateIcon.STATEICON_CHECKEDOUT));
        }
    }
}
