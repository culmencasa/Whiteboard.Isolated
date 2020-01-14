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
    /// PageBoard.xaml 的交互逻辑
    /// </summary>
    public partial class PageBoard : UserControl
    {
        public PageBoard()
        {
            InitializeComponent();

            PageCount = 1;
            PageIndex = 1;
        }

        private int _pageIndex = 1;
        private int _pageCount = 1;

        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }
            set
            {
                _pageIndex = value;
                lblPageIndex.Content = _pageIndex;
            }
        }

        public int PageCount
        {
            get
            {
                return _pageCount;
            }
            set
            {
                _pageCount = value;
                lblPageCount.Content = _pageCount;
            }
        }

        public int MaxPageCount { get; set; } = 50;
        
    }
}
