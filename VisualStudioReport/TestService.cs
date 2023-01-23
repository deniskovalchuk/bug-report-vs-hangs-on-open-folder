using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.SccIntegration;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace VisualStudioReport
{
    [Guid("af7b0a50-8811-4a61-9b96-dcce00828c8a")]
    public interface STestService
    {
    }

    public interface ITestService
    {
    }

    public class TestService : STestService,
                               ITestService,
                               IVsSolutionEvents7,
                               IVsSolutionEvents,
                               IVsSccProvider,
                               IVsSccFolderProviderBinder
    {
        // IVsSolutionEvents7
        public void OnAfterOpenFolder(string folderPath)
        {
            if (SccBindingsChanged != null)
            {
                // This call causes Visual Studio to hang.
                SccBindingsChanged(this, new SccBindingChangedEventArgs());
            }
        }

        public void OnBeforeCloseFolder(string folderPath) {}
        public void OnQueryCloseFolder(string folderPath, ref int pfCancel) {}
        public void OnAfterCloseFolder(string folderPath) {}
        public void OnAfterLoadAllDeferredProjects() {}

        // IVsSolutionEvents
        public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeCloseSolution(object pUnkReserved)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterCloseSolution(object pUnkReserved)
        {
            return VSConstants.S_OK;
        }

        // IVsSccProvider
        public int SetActive()
        {
            return VSConstants.S_OK;
        }

        public int SetInactive()
        {
            return VSConstants.S_OK;
        }

        public int AnyItemsUnderSourceControl(out int pfResult)
        {
            pfResult = 1;
            return VSConstants.S_OK;
        }

        // IVsSccFolderProviderBinder
        public event EventHandler<SccBindingChangedEventArgs> SccBindingsChanged;
        public event EventHandler<SccStatusChangedEventArgs> SccStatusChanged;

        Task<IEnumerable<IVsSccFolderProvider>> IVsSccFolderProviderBinder.BindAsync(string rootFolder, CancellationToken cancellationToken)
        {
            IVsSccFolderProvider provider = new FolderProvider(rootFolder);

            return Task.FromResult<IEnumerable<IVsSccFolderProvider>>(new[] { provider });
        }
    }
}
