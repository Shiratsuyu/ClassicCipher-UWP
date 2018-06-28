using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassicCipher.Model;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;

namespace ClassicCipher.ViewModel
{
    public class VigenereKeyViewController
    {
        public int KeyLength;

        public bool this[int i]
        {
            get
            {
                if (i <= KeyLength)
                {
                    return true;
                }
                return false;
            }
        }

        public VigenereKeyViewController(int len)
        {
            KeyLength = len;
        }
    }

    public class VigenereKeyViewArgument
    {
        public string KeyValue;

        public struct Argument
        {
            public int index;

            public char keychar;
        }

        public Argument this[int i]
        {
            get
            {
                if (i <= KeyValue.Length && i > 0 && !string.IsNullOrEmpty(KeyValue))
                {
                    return new Argument()
                    {
                        index = i,
                        keychar = KeyValue[i - 1]
                    };
                }

                return new Argument();
            }
        }
    }

    public class VigenereViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;
        private VigenereKeyViewController _viewController;
        private VigenereKeyViewArgument _viewArgument;
        private string _vigenereKey;
        private string _inputKey;
        private string _plainText;
        private string _cipherText;
        private RelayCommand<string> _visitKeyRound;
        private RelayCommand _changeKeyCommand;
        private RelayCommand _encryptCommand;
        private RelayCommand _decryptCommand;

        public VigenereKeyViewController ViewController
        {
            get
            {
                return _viewController;
            }
            set
            {
                _viewController = value;
                RaisePropertyChanged<VigenereKeyViewController>(nameof(ViewController));
            }
        }

        public VigenereKeyViewArgument ViewArgument
        {
            get
            {
                return _viewArgument;
            }
            set
            {
                _viewArgument = value;
                RaisePropertyChanged<VigenereKeyViewArgument>(nameof(ViewArgument));
            }
        }

        public string VigenereKey
        {
            get
            {
                return _vigenereKey;
            }
            set
            {
                _vigenereKey = value;
                ViewController.KeyLength = VigenereKey.Length;
                ViewArgument.KeyValue = VigenereKey;
                VisitKeyRound.RaiseCanExecuteChanged();
                RaisePropertyChanged<string>(nameof(VigenereKey));
                ChangeKeyCommand.RaiseCanExecuteChanged();
            }
        }

        public string InputKey
        {
            get
            {
                return _inputKey;
            }
            set
            {
                _inputKey = value;
                RaisePropertyChanged<string>(nameof(InputKey));
                ChangeKeyCommand.RaiseCanExecuteChanged();
            }
        }

        public string PlainText
        {
            get
            {
                return _plainText;
            }
            set
            {
                _plainText = value;
                RaisePropertyChanged<string>(nameof(PlainText));
                EncryptCommand.RaiseCanExecuteChanged();
            }
        }

        public string CipherText
        {
            get
            {
                return _cipherText;
            }
            set
            {
                _cipherText = value;
                RaisePropertyChanged<string>(nameof(CipherText));
                DecryptCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand ChangeKeyCommand
        {
            get
            {
                return _changeKeyCommand ??
                       (_changeKeyCommand = new RelayCommand(
                           () => VigenereKey = InputKey.ToUpper(),
                           () => (VigenereKey != InputKey?.ToUpper())));
            }
        }

        public RelayCommand EncryptCommand
        {
            get
            {
                return _encryptCommand ??
                       (_encryptCommand = new RelayCommand(
                           () => CipherText = VigenereModel.Encrypt(PlainText.ToUpper(), VigenereKey),
                           () => !string.IsNullOrEmpty(PlainText)));
            }
        }

        public RelayCommand DecryptCommand
        {
            get
            {
                return _decryptCommand ??
                       (_decryptCommand = new RelayCommand(
                           () => PlainText = VigenereModel.Decrypt(CipherText, VigenereKey).ToLower(),
                           () => !string.IsNullOrEmpty(CipherText)));
            }
        }

        public RelayCommand<string> VisitKeyRound
        {
            get
            {
                return _visitKeyRound ??
                       (_visitKeyRound = new RelayCommand<string>(
                           str =>
                           {
                               _navigationService.NavigateTo(ViewModelLocator.VigenereKeyRoundPageKey);
                               Messenger.Default.Send(ViewArgument[int.Parse(str)]);
                           },
                           str => ViewController[int.Parse(str)]));
            }
        }

        public VigenereViewModel(IDataService dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            _viewController = new VigenereKeyViewController(0);
            _viewArgument = new VigenereKeyViewArgument();
        }
    }
}
