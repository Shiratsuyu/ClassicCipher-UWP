using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using ClassicCipher.Model;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace ClassicCipher.ViewModel
{
    public class HillViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;
        private HillModel _hillInstance;
        private List<int> _keyList;
        private List<int> _inverseKeyList;
        private string _hillKey;
        private string _plainText;
        private string _cipherText;
        private RelayCommand _randomKeyCommand;
        private RelayCommand _importKeyCommand;
        private RelayCommand _encryptCommand;
        private RelayCommand _decryptCommand;

        public List<int> KeyList
        {
            get
            {
                return _keyList;
            }
            set
            {
                _keyList = value;
                RaisePropertyChanged<List<int>>(nameof(KeyList));
                EncryptCommand.RaiseCanExecuteChanged();
            }
        }

        public List<int> InverseKeyList
        {
            get
            {
                return _inverseKeyList;
            }
            set
            {
                _inverseKeyList = value;
                RaisePropertyChanged<List<int>>(nameof(InverseKeyList));
                DecryptCommand.RaiseCanExecuteChanged();
            }
        }

        public string HillKey
        {
            get
            {
                return _hillKey;
            }
            set
            {
                _hillKey = value;
                RaisePropertyChanged<string>(nameof(HillKey));
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

        public RelayCommand RandomKeyCommand
        {
            get
            {
                return _randomKeyCommand ??
                       (_randomKeyCommand = new RelayCommand(
                           GetKeyFromRamdom,
                           () => true));
            }
        }

        public RelayCommand ImportKeyCommand
        {
            get
            {
                return _importKeyCommand ??
                       (_importKeyCommand = new RelayCommand(
                           GetKeyFromImport,
                           () => true));
            }
        }

        public RelayCommand EncryptCommand
        {
            get
            {
                return _encryptCommand ??
                       (_encryptCommand = new RelayCommand(
                           () => CipherText = _hillInstance.Encrypt(PlainText),
                           () => (!string.IsNullOrEmpty(PlainText) && (KeyList != null))));
            }
        }

        public RelayCommand DecryptCommand
        {
            get
            {
                return _decryptCommand ??
                       (_decryptCommand = new RelayCommand(
                           () => PlainText = _hillInstance.Decrypt(CipherText).ToLower(),
                           () => (!string.IsNullOrEmpty(CipherText) && (InverseKeyList != null))));
            }
        }

        private List<int> _stringToList(string str)
        {
            List<int> container = new List<int>();
            char[] array = str.ToUpper().ToCharArray();

            foreach (char ch in array)
            {
                container.Add((int) (ch - 65));
            }

            return container;
        }

        private List<int> _3By3ArrayToList(int[][] array)
        {
            List<int> container = new List<int>();

            container.Add(array[0][0]);
            container.Add(array[0][1]);
            container.Add(array[0][2]);
            container.Add(array[1][0]);
            container.Add(array[1][1]);
            container.Add(array[1][2]);
            container.Add(array[2][0]);
            container.Add(array[2][1]);
            container.Add(array[2][2]);

            return container;
        }

        private List<int> _3By3ArrayToList(long[][] array)
        {
            List<int> container = new List<int>();

            container.Add((int) array[0][0]);
            container.Add((int) array[0][1]);
            container.Add((int) array[0][2]);
            container.Add((int) array[1][0]);
            container.Add((int) array[1][1]);
            container.Add((int) array[1][2]);
            container.Add((int) array[2][0]);
            container.Add((int) array[2][1]);
            container.Add((int) array[2][2]);

            return container;
        }

        private string _listToString(List<int> list)
        {
            List<char> chars = new List<char>();

            foreach (int itor in list)
            {
                chars.Add((char) (itor + 65));
            }

            return new string(chars.ToArray());
        }

        private int[][] _listTo3By3Array(List<int> list)
        {
            if (list.LongCount() != 9)
            {
                throw new Exception("输入密钥的长度必须为9位");
            }

            int[][] container = new int[3][];
            container[0] = new int[3];
            container[1] = new int[3];
            container[2] = new int[3];

            container[0][0] = list[0];
            container[0][1] = list[1];
            container[0][2] = list[2];
            container[1][0] = list[3];
            container[1][1] = list[4];
            container[1][2] = list[5];
            container[2][0] = list[6];
            container[2][1] = list[7];
            container[2][2] = list[8];

            return container;
        }

        private long[][] _listTo3By3LongArray(List<int> list)
        {
            if (list.LongCount() != 9)
            {
                throw new Exception("输入密钥的长度必须为9位");
            }

            long[][] container = new long[3][];
            container[0] = new long[3];
            container[1] = new long[3];
            container[2] = new long[3];

            container[0][0] = list[0];
            container[0][1] = list[1];
            container[0][2] = list[2];
            container[1][0] = list[3];
            container[1][1] = list[4];
            container[1][2] = list[5];
            container[2][0] = list[6];
            container[2][1] = list[7];
            container[2][2] = list[8];

            return container;
        }

        private void GetKeyFromRamdom()
        {
            _hillInstance.GetRandomKeyMatrix();
            KeyList = _3By3ArrayToList(_hillInstance.Matrix);
            InverseKeyList = _3By3ArrayToList(_hillInstance.InverseMatrix);
            HillKey = _listToString(KeyList);
        }

        private void GetKeyFromImport()
        {
            try
            {
                _hillInstance.ImportKeyMatrix(_listTo3By3LongArray(_stringToList(HillKey)));
                KeyList = _3By3ArrayToList(_hillInstance.Matrix);
                InverseKeyList = _3By3ArrayToList(_hillInstance.InverseMatrix);
            }
            catch (Exception e)
            {
                KeyList = null;
                InverseKeyList = null;
                var dialog = ServiceLocator.Current.GetInstance<IDialogService>();
                dialog.ShowMessage("导入密钥出错", e.Message);
            }
        }

        public HillViewModel(IDataService dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            _hillInstance = new HillModel(3);
        }
    }
}
