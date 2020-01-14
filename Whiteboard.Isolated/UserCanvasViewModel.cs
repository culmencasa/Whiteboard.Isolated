using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Whiteboard.ViewModels
{
    public class UserCanvasViewModel // : ViewModelBase
    {
        public UserCanvasViewModel()
        {

        }


        public ICommand RunDialogCommand { get; set; }

        public ICommand NextPageCommand { get; private set; }

        public ICommand PreviousPageCommand { get; private set; }

        public ICommand AddNewPageCommand { get; private set; }

        public ICommand RemoveCurrentPageCommand { get; private set; }
         

    }
}
