using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassicCipher.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace ClassicCipher.ViewModel
{
    class CaesarAlphabet
    {
        private const char _source = 'A';

        public int Offset = 0;

        public char this[int i]
        {
            get => (char)(_source + (Offset + 26 + i) % 26);
        }
    }

    class CaesarViewModel : ViewModelBase
    {
        private CaesarModel _caesarInstance;

        private CaesarAlphabet _offSetAlphabet;

        private int _offset;

        private string _plainText;

        private string _cipherText;

        private RelayCommand _encryptCommand;

        private RelayCommand _decryptCommand;

        public CaesarAlphabet OffSetAlphabet
        {
            get
            {
                return _offSetAlphabet;
            }
            set
            {
                _offSetAlphabet = value;
                RaisePropertyChanged<CaesarAlphabet>(nameof(OffSetAlphabet));
            }
        }

        public int OffSet
        {
            get
            {
                return _offset; 
            }
            set
            {
                if (value >= -25 && value <= 25)
                {
                    _offset = value;
                }
                RaisePropertyChanged<int>(nameof(OffSet));
                _offSetAlphabet.Offset = value;
                RaisePropertyChanged<CaesarAlphabet>(nameof(OffSetAlphabet));
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
                if (System.Text.RegularExpressions.Regex.IsMatch(value, @"^[a-zA-Z]*$") || string.IsNullOrEmpty(value))
                {
                    value = value.ToLower();
                    _plainText = value;
                }
                value = _plainText;
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
                value = value.ToUpper();
                _cipherText = value;
                RaisePropertyChanged<string>(nameof(CipherText));
                DecryptCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand EncryptCommand
        {
            get
            {
                return _encryptCommand ??
                       (_encryptCommand = new RelayCommand(
                           () => CipherText = _caesarInstance.CaesarEncrypt(PlainText, OffSet),
                           () => !string.IsNullOrEmpty(PlainText)));
            }
        }

        public RelayCommand DecryptCommand
        {
            get
            {
                return _decryptCommand ??
                       (_decryptCommand = new RelayCommand(
                           () => PlainText = _caesarInstance.CaesarDecrypt(CipherText, OffSet),
                           () => !string.IsNullOrEmpty(CipherText)));
            }
        }

        public CaesarViewModel()
        {
            _caesarInstance = new CaesarModel();
            _offSetAlphabet = new CaesarAlphabet();
            OffSet = 0;
        }
    }
}
