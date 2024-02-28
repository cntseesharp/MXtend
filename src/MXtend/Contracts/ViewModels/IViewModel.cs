using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXtend.Contracts.ViewModels
{
    public interface IViewModel
    {
        bool IsModal { get; set; }

        Task OnLoad();
        Task OnUnload();

        void OnAppearing();
        void OnDisappearing();

        /// <summary>
        /// Called when object is passed in the navigation request
        /// </summary>
        Task Prepare(object parameter);
    }
}
