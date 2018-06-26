using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using ClassicCipher.Model;
using CommonServiceLocator;

namespace ClassicCipher.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;
        private RelayCommand _caesarNavigateCommand;
        private RelayCommand _playfairNavigateCommand;
        private RelayCommand _hillNavigateCommand;
        private RelayCommand _vigenereNavigateCommand;

        public RelayCommand CaesarNavigateCommand
        {
            get
            {
                return _caesarNavigateCommand ??
                       (_caesarNavigateCommand = new RelayCommand(
                           () => _navigationService.NavigateTo(ViewModelLocator.CaesarPageKey),
                           () => true));
            }
        }

        public RelayCommand PlayfairNavigateCommand
        {
            get
            {
                return _playfairNavigateCommand ??
                       (_playfairNavigateCommand = new RelayCommand(
                           () => _navigationService.NavigateTo(ViewModelLocator.PlayfairPageKey),
                           () => true));
            }
        }

        public RelayCommand HillNavigateCommand
        {
            get
            {
                return _hillNavigateCommand
                       ?? (_hillNavigateCommand = new RelayCommand(
                           () => _navigationService.NavigateTo(ViewModelLocator.HillPageKey),
                           () => true));
            }
        }

        public RelayCommand VigenereNavigateCommand
        {
            get
            {
                return _vigenereNavigateCommand
                       ?? (_vigenereNavigateCommand = new RelayCommand(
                           () => _navigationService.NavigateTo(ViewModelLocator.VigenerePageKey),
                           () => true));
            }
        }

        public MainViewModel(IDataService dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
        }
    }
}