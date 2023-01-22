using Microsoft.VisualStudio.Shell.SccIntegration;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Microsoft;

namespace VisualStudioReport
{
    [Export(typeof(IVsSccFolderProviderBinder))]
    public class FolderProviderBinder : IVsSccFolderProviderBinder
    {
        private IVsSccFolderProviderBinder inner;

        [ImportingConstructor]
        public FolderProviderBinder([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            inner = (IVsSccFolderProviderBinder) serviceProvider.GetService(typeof(STestService));
            Assumes.Present(inner);
        }

        public event EventHandler<SccBindingChangedEventArgs> SccBindingsChanged
        {
            add
            {
                inner.SccBindingsChanged += value;
            }
            remove
            {
                inner.SccBindingsChanged -= value;
            }
        }

        public event EventHandler<SccStatusChangedEventArgs> SccStatusChanged
        {
            add
            {
                inner.SccStatusChanged += value;
            }
            remove
            {
                inner.SccStatusChanged -= value;
            }
        }

        public Task<IEnumerable<IVsSccFolderProvider>> BindAsync(string rootFolder, CancellationToken cancellationToken)
        {
            return inner.BindAsync(rootFolder, cancellationToken);
        }
    }
}
