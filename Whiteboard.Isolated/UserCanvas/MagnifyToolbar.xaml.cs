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
    /// MagnifyToolbar.xaml 的交互逻辑
    /// </summary>
    public partial class MagnifyToolbar : UserControl
    {
        public MagnifyToolbar()
        {
            InitializeComponent();
        }

        public event Action<IconControl> DimTheLightHandler;
        public event Action<IconControl> CloseHandler;

        private void OnButtonItemClick(object sender, EventArgs e)
        {
            IconControl button = sender as IconControl;
            if (button.IconText == "关灯")
            {
                DimTheLightHandler?.Invoke(button);
            }
            else if (button.IconText == "开灯")
            {
                DimTheLightHandler?.Invoke(button);
            }
            else if (button.IconText == "关闭")
            {
                CloseHandler?.Invoke(button);
            }

        }
        
    }
}
