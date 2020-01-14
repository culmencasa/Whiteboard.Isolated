using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Whiteboard.Isolated.Interface;
using Whiteboard.Isolated.DataAccess;
using Whiteboard.Isolated.Model;
using Whiteboard.Isolated.Export.Compose;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;

namespace Whiteboard.Isolated.WPF
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        #region 静态成员

        internal static CanvasBoardSetting Setting { get; set; }

        static string SettingFileName = "CanvasBoardSetting.xml";

        static App()
        {
            // 如果文件不存在, 则创建一份
            if (!File.Exists(SettingFileName))
            {
                Setting = new CanvasBoardSetting();
                SerialzeSetting(Setting);
            }
        }

        /// <summary>
        /// 序列化参数
        /// </summary>
        /// <param name="entity"></param>
        static void SerialzeSetting(CanvasBoardSetting entity)
        {
            if (entity == null)
                return;

            XmlSerializer xs = new XmlSerializer(typeof(CanvasBoardSetting));
            using (Stream stream = new FileStream(SettingFileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                xs.Serialize(stream, entity);
            }
        }

        /// <summary>
        /// 反序列化参数
        /// </summary>
        /// <returns></returns>
        static CanvasBoardSetting DeserialzeSetting()
        {
            if (!File.Exists(SettingFileName))
            {
                return new CanvasBoardSetting();
            }
            CanvasBoardSetting entity = null;
            using (FileStream fs = new FileStream(SettingFileName, FileMode.Open, FileAccess.Read))
            {
                XmlSerializer xs = new XmlSerializer(typeof(CanvasBoardSetting));
                entity = (CanvasBoardSetting)xs.Deserialize(fs);
            }

            return entity;
        }

        /// <summary>
        /// 删除数据库
        /// </summary>
        static void DeleteDatabase()
        {
            // 教师ID
            string teacherBusinessId = Setting.UserBusinessId;
            if (string.IsNullOrEmpty(teacherBusinessId))
            {
                teacherBusinessId = "NOUSER";
            }
            // 教师用户文件夹
            string teacherDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserCache", teacherBusinessId);
            // 数据库文件名
            string userdb = Misc.GetUserDbName();
            // 数据库完整路径
            string dbFileFullPath = Path.Combine(teacherDir, userdb);

            if (File.Exists(dbFileFullPath))
            {
                File.Delete(dbFileFullPath);
            }
        }

        #endregion


        /// <summary>
        /// 程序启动命令行模式
        /// Whiteboard.exe 
        ///                -id=12345                老师Id
        ///                -showtool=false          隐藏工具栏
        ///                -showpager=false         隐藏分页栏
        ///                -cleandb=true            清理数据库
        ///                -goto=2                  跳到指定页(默认页从1开始)
        /// 
        /// 也可以xml里修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void App_Startup(object sender, StartupEventArgs e)
        {
            // 如果没有给定UserBusinessId, 使用默认配置
            BusinessDataParts.Singleton.UserBusinessId = "NOUSER";

            Setting = DeserialzeSetting();

            // 处理exe传入的参数
            if (e.Args != null && e.Args.Length > 0)
            {
                for (int i = 0; i < e.Args.Length; i++)
                {
                    string item = e.Args[i].ToLower();

                    // ID
                    if (item.StartsWith("-id="))
                    {
                        string value = item.Substring(item.IndexOf("=") + 1);
                        BusinessDataParts.Singleton.UserBusinessId = value;

                        Setting.UserBusinessId = value;
                        continue;
                    }

                    // 显示工具栏
                    if (item.StartsWith("-showtool="))
                    {
                        string value = item.Substring(item.IndexOf("=") + 1).ToLower();
                        if (value == "false")
                        {
                            Setting.Toolbar.Visible = false;
                        }
                        continue;
                    }

                    // 显示分页
                    if (item.StartsWith("-showpager="))
                    {
                        string value = item.Substring(item.IndexOf("=") + 1).ToLower();
                        if (value == "false")
                        {
                            Setting.Pager.Visible = false;
                        }
                        continue;
                    }

                    // 清空数据库
                    if (item.StartsWith("-cleandb="))
                    {
                        string value = item.Substring(item.IndexOf("=") + 1).ToLower();
                        if (value == "true")
                        {
                            Setting.CleanDatabase = true;

                            DeleteDatabase();
                        }
                        continue;
                    }

                    // 跳转页面
                    if (item.StartsWith("-goto="))
                    {
                        string value = item.Substring(item.IndexOf("=") + 1);

                        int pageIndex = 1;
                        if (!Int32.TryParse(value, out pageIndex))
                        {
                            pageIndex = 1;
                        }
                        Setting.Pager.PageIndex = pageIndex;
                        continue;
                    }
                }

                //注: 传入的参数不会保存配置到xml文件
            }
            else
            {
                Setting.Pager.PageIndex = -1;// 标记为-1, 让程序启动时从数据库记录的索引加载

                // 清空数据库
                if (Setting.CleanDatabase)
                {
                    DeleteDatabase();
                }

            }

              
            // 启动主窗体
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }


    }
}
