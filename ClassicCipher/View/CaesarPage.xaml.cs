using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ClassicCipher.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CaesarPage : Page
    {
        public CaesarPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 禁止页面自动获取焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelFocusonPageLoaded(object sender, RoutedEventArgs e)
        {
            getRootScrollViewer().Focus(FocusState.Programmatic);
        }

        private ScrollViewer getRootScrollViewer()
        {
            DependencyObject el = this;
            while (el != null && !(el is ScrollViewer))
            {
                el = VisualTreeHelper.GetParent(el);
            }

            return (ScrollViewer)el;
        }

        /// <summary>
        /// 过滤掉非法字符并将光标重新置为字符串末
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlphaFilter(object sender, TextChangedEventArgs e)
        {
            if (Regex.IsMatch(((TextBox)sender).Text, @"[^A-Za-z]+"))
            {
                ((TextBox)sender).Text = Regex.Replace(((TextBox)sender).Text, @"[^A-Za-z]+", "");
                ((TextBox)sender).Select(((TextBox)sender).Text.Length, 0);
            }
        }
    }
}
