using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Whiteboard.Isolated.UserCanvas
{
    /// <summary>
    /// AdornerToolbar.xaml 的交互逻辑
    /// </summary>
    public partial class AdornerToolbar : UserControl
    {
        public AdornerToolbar()
        {
            InitializeComponent();
        }

        public event Action PutOnTopHandler;
        public event Action CopyHandler;
        public event Action DeleteHandler;

        private void OnButtonItemClick(object sender, EventArgs e)
        {
            IconControl button = sender as IconControl;
            if (button.IconText == "置顶")
            {
                PutOnTopHandler?.Invoke();
            }
            else if (button.IconText == "复制")
            {
                CopyHandler?.Invoke();
            }
            else if (button.IconText == "删除")
            {
                DeleteHandler?.Invoke();
            }

        }
        
    }
}
