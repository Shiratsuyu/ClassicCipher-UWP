using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using ClassicCipher.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace ClassicCipher.ViewModel
{
    public class PlayfairMatrix
    {
        private static readonly char[] _alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        private string _key;

        private List<char> _charList;

        private void ArrangeList()
        {
            string _pretreatKey = _key.ToUpper();
            _pretreatKey = _pretreatKey.Replace('J', 'I');
            char[] _keyArray = _pretreatKey.ToCharArray();

            _charList = new List<char>();

            foreach (char itor in _keyArray)
            {
                if (_charList.FindIndex(ch => ch == itor) == -1)
                {
                    _charList.Add(itor);
                }
            }

            foreach (char itor in _alphabet)
            {
                if (_charList.FindIndex(ch => ch == itor) == -1)
                {
                    _charList.Add(itor);
                }
            }
        }

        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
                ArrangeList();
            }
        }

        public string this[int i]
        {
            get
            {
                if (_charList[i] == 'I')
                {
                    return "I/J";
                }
                return _charList[i].ToString();
            }
        }
    }

    public class PlayfairViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;
        private PlayfairModel _playfairInstance;
        private PlayfairMatrix _cuttentMatrix;
        private string _playfairKey;
        private string _plainText;
        private string _cipherText;
        private RelayCommand _resetCommand;
        private RelayCommand _createCommand;
        private RelayCommand _encryptCommand;
        private RelayCommand _decryptCommand;

        public PlayfairMatrix CurrentMatrix
        {
            get
            {
                return _cuttentMatrix;
            }
            set
            {
                _cuttentMatrix = value;
                RaisePropertyChanged<PlayfairMatrix>(nameof(CurrentMatrix));
            }
        }

        public string PlayfairKey
        {
            get
            {
                return _playfairKey;
            }
            set
            {
                _playfairKey = value;
                RaisePropertyChanged<string>(nameof(PlayfairKey));
                CreateCommand.RaiseCanExecuteChanged();
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

        private void ResetMatrix()
        {
            PlayfairKey = "";
            _playfairInstance.SetPkayfairKey("ABCDE");
            CurrentMatrix.Key = "ABCDE";
            RaisePropertyChanged<PlayfairMatrix>(nameof(CurrentMatrix));
            ResetCommand.RaiseCanExecuteChanged();
            CreateCommand.RaiseCanExecuteChanged();
        }

        private void CreateMatrix()
        {
            _playfairInstance.SetPkayfairKey(PlayfairKey);
            CurrentMatrix.Key = PlayfairKey;
            RaisePropertyChanged<PlayfairMatrix>(nameof(CurrentMatrix));
            ResetCommand.RaiseCanExecuteChanged();
            CreateCommand.RaiseCanExecuteChanged();
        }

        public RelayCommand ResetCommand
        {
            get
            {
                return _resetCommand ??
                       (_resetCommand = new RelayCommand(
                           ResetMatrix,
                           () => !(string.IsNullOrEmpty(PlayfairKey) && CurrentMatrix.Key == "ABCDE")));
            }
        }

        public RelayCommand CreateCommand
        {
            get
            {
                return _createCommand ??
                       (_createCommand = new RelayCommand(
                           CreateMatrix,
                           () => (!string.IsNullOrEmpty(PlayfairKey) && CurrentMatrix.Key != PlayfairKey)));
            }
        }

        public RelayCommand EncryptCommand
        {
            get
            {
                return _encryptCommand ??
                       (_encryptCommand = new RelayCommand(
                           () => CipherText = _playfairInstance.Encrypt(PlainText).ToUpper(),
                           () => !string.IsNullOrEmpty(PlainText)));
            }
        }

        public RelayCommand DecryptCommand
        {
            get
            {
                return _decryptCommand ??
                       (_decryptCommand = new RelayCommand(
                           () => PlainText = _playfairInstance.Decrypt(CipherText).ToLower(),
                           () => !string.IsNullOrEmpty(CipherText)));
            }
        }

        public PlayfairViewModel(IDataService dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            _playfairInstance = new PlayfairModel("ABCDE");
            _cuttentMatrix = new PlayfairMatrix();
            CurrentMatrix.Key = "ABCDE";
            RaisePropertyChanged<PlayfairMatrix>(nameof(CurrentMatrix));
        }
    }
}
