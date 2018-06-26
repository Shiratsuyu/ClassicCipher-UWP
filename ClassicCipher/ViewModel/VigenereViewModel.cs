using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassicCipher.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace ClassicCipher.ViewModel
{
    class VigenereViewModel : ViewModelBase
    {
        private string _vigenereKey;

        private string _inputKey;

        private string _plainText;

        private string _cipherText;

        private RelayCommand _changeKeyCommand;

        private RelayCommand _encryptCommand;

        private RelayCommand _decryptCommand;

        public string VigenereKey
        {
            get
            {
                return _vigenereKey;
            }
            set
            {
                _vigenereKey = value;
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


    }
}
