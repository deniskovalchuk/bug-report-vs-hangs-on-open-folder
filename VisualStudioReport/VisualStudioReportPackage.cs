using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace VisualStudioReport
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(VisualStudioReportPackage.PackageGuidString)]
    [ProvideService(typeof(STestService))]
    [ProvideSourceControlProvider("Test Source Control Provider", "#100",
                                  "6e100c73-1435-424e-9521-7516e1b59c8c",
                                  "eee1eeeb-e1f0-4643-830d-b7f9f914aaab",
                                  "af7b0a50-8811-4a61-9b96-dcce00828c8a")]
    public sealed class VisualStudioReportPackage : AsyncPackage
    {
        public const string PackageGuidString = "eee1eeeb-e1f0-4643-830d-b7f9f914aaab";
        private TestService testService = new TestService();

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            
            ((IServiceContainer) this).AddService(typeof(STestService), testService, true);

            // Set up notification of solution events on the 'testService' object.
            IVsSolution solution = (IVsSolution) await GetServiceAsync(typeof(SVsSolution));
            Assumes.Present(solution);
            uint cookie;
            solution.AdviseSolutionEvents(testService, out cookie);

            // Register with Visual Studio.
            IVsRegisterScciProvider rscp = (IVsRegisterScciProvider) await GetServiceAsync(typeof(IVsRegisterScciProvider));
            Assumes.Present(rscp);
            rscp.RegisterSourceControlProvider(new Guid("6e100c73-1435-424e-9521-7516e1b59c8c"));
        }
    }
}
