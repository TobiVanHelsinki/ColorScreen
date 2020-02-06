using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace ColorScreen
{
    public sealed partial class MainPage : Page
    {
        IEnumerable<Color> AllColors;
        IEnumerator<Color> AllColorsEnum;
        IEnumerable<(Color, string)> AllColorsName;
        public bool FullScreenMode;

        public MainPage()
        {
            InitializeComponent();
            var propertyInfo = typeof(Colors).GetRuntimeProperties();
            var enumerable = propertyInfo.Where(x => x.PropertyType == typeof(Color)).ToList();
            AllColorsName = from x in enumerable let colorval = x.GetValue(null, null) let iscolor = colorval is Color where iscolor select((Color)colorval, x.Name);
            AllColors = enumerable.Select(x => x.GetValue(null, null)).OfType<Color>().ToList();
            AllColorsEnum = AllColors.GetEnumerator();
            Background = new SolidColorBrush(Colors.Black);

            if (this is Page p && p.ContextFlyout is MenuFlyout fl)
            {
                foreach (var item in AllColorsName)
                {
                    var item1 = new MenuFlyoutItem() { Text = item.Item2, Tag = item.Item1 };
                    item1.Click += SetColor_Click;
                    fl.Items.Add(item1);
                }
            }

            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            SetColor(Colors.Black);
        }

        private void SetColor(Color col)
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            // Set active window colors
            titleBar.ForegroundColor = Complementary(Windows.UI.Colors.White, col);
            titleBar.BackgroundColor = col;
            titleBar.ButtonForegroundColor = Complementary(Windows.UI.Colors.White, col);
            titleBar.ButtonBackgroundColor = col;
            titleBar.ButtonHoverForegroundColor = Complementary(Windows.UI.Colors.White, col);
            titleBar.ButtonHoverBackgroundColor = col;
            titleBar.ButtonPressedForegroundColor = Complementary(Windows.UI.Colors.Gray, col);
            titleBar.ButtonPressedBackgroundColor = col;
            Background = new SolidColorBrush(col);
        }

        private Color? Complementary(Color col1, Color col2)
        {
            return new Color() {
                A = (byte)Math.Abs(col1.A - col2.A),
                R = (byte)Math.Abs(col1.R - col2.R),
                G = (byte)Math.Abs(col1.G - col2.G),
                B = (byte)Math.Abs(col1.B - col2.B),
            };
        }

        private void Page_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (AllColorsEnum.MoveNext())
            {
                SetColor(AllColorsEnum.Current);
            }
            else
            {
                AllColorsEnum = AllColors.GetEnumerator();
            }
        }

        private void Page_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (sender is Page p && p.ContextFlyout is MenuFlyout fl)
            {
                fl.ShowAt(p, new FlyoutShowOptions() { Placement = FlyoutPlacementMode.Auto, ShowMode = FlyoutShowMode.Auto });
            }
        }

        private void SetColor_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem item && item.Tag is Color c)
            {
                SetColor(c);
            }
        }

        private void Reset_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            SetColor(Colors.Black);
        }

        private void Page_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (!FullScreenMode)
            {
                FullScreenMode = ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
            }
            else
            {
                ApplicationView.GetForCurrentView().ExitFullScreenMode();
                FullScreenMode = false;
            }
        }
    }
}
