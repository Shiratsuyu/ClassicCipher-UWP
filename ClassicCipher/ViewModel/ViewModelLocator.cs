using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using ClassicCipher.Model;
using ClassicCipher.View;
using CommonServiceLocator;

namespace ClassicCipher.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        public const string CaesarPageKey = "CaesarPage";
        public const string PlayfairPageKey = "PlayfairPage";
        public const string HillPageKey = "HillPageKey";
        public const string VigenerePageKey = "VigenerePage";
        public const string VigenereKeyRoundPageKey = "VigenereKeyRoundPage";
        /// <summary>
        /// This property can be used to force the application to run with design time data.
        /// </summary>
        public static bool UseDesignTimeData
        {
            get
            {
                return false;
            }
        }

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var nav = new NavigationService();
            nav.Configure(CaesarPageKey, typeof(CaesarPage));
            nav.Configure(PlayfairPageKey, typeof(PlayfairPage));
            nav.Configure(HillPageKey, typeof(HillPage));
            nav.Configure(VigenerePageKey, typeof(VigenerePage));
            nav.Configure(VigenereKeyRoundPageKey, typeof(VigenereKeyRoundPage));
            SimpleIoc.Default.Register<INavigationService>(() => nav);

            SimpleIoc.Default.Register<IDialogService, DialogService>();

            if (ViewModelBase.IsInDesignModeStatic
                    || UseDesignTimeData)
            {
                SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<CaesarViewModel>();
            SimpleIoc.Default.Register<PlayfairViewModel>();
            SimpleIoc.Default.Register<HillViewModel>();
            SimpleIoc.Default.Register<VigenereViewModel>();
            SimpleIoc.Default.Register<VigenereKeyRoundViewModel>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public CaesarViewModel Caesar => ServiceLocator.Current.GetInstance<CaesarViewModel>();
        public PlayfairViewModel Playfair => ServiceLocator.Current.GetInstance<PlayfairViewModel>();
        public HillViewModel Hill => ServiceLocator.Current.GetInstance<HillViewModel>();
        public VigenereViewModel Vigenere => ServiceLocator.Current.GetInstance<VigenereViewModel>();
        public VigenereKeyRoundViewModel VigenereKeyRound => ServiceLocator.Current.GetInstance<VigenereKeyRoundViewModel>();
    }
}
