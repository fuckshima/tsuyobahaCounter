using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace TsuyobahaCounter.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
            DropItems = new ObservableCollection<DropItem>()
            {
                new DropItem(0, 99),
                new DropItem(100, 199),
                new DropItem(200, 299),
                new DropItem(300, 800),
            };

            foreach (var item in DropItems)
            {
                item.PropertyChanged += Item_PropertyChanged;
            }
        }

        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "BlueDrop":
                    TotalCount++;
                    BlueTotalCount++;
                    break;
                case "NoDrop":
                    TotalCount++;
                    break;
            }
        }



        // ドロップアイテムのリスト
        private ObservableCollection<DropItem> _dropItems = new ObservableCollection<DropItem>();
        // ドロップ数初期化コマンド
        private RelayCommand? _clearDropCountCommand;

        /// <summary>
        /// 討伐数
        /// </summary>
        public int _totalCount = 0;
        public int TotalCount
        {
            get => _totalCount;
            set => SetProperty(ref _totalCount, value);
        }

        /// <summary>
        /// 青箱数
        /// </summary>
        public int _blueTotalCount = 0;
        public int BlueTotalCount
        {
            get => _blueTotalCount;
            set => SetProperty(ref _blueTotalCount, value);
        }

        /// <summary>
        /// ドロップアイテムのリスト
        /// </summary>
        public ObservableCollection<DropItem> DropItems
        {
            get => _dropItems;
            set => SetProperty(ref _dropItems, value);
        }

        /// <summary>
        /// ドロップ数初期化コマンド
        /// </summary>
        public RelayCommand ClearDropCountCommand
        {
            get
            {
                return _clearDropCountCommand ??= new RelayCommand(
                    () =>
                    {
                        foreach(var item in DropItems)
                        {
                            item.InitCounts();
                            TotalCount = 0;
                            BlueTotalCount = 0;
                        }
                    });
            }
        }
    }

    /// <summary>
    /// 貢献度別ドロップアイテム
    /// </summary>
    internal class DropItem : ViewModelBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="minContribution">貢献度最小値</param>
        /// <param name="maxContribution">貢献度最大値</param>
        public DropItem(int minContribution, int maxContribution)
        {
            MinContribution = minContribution;
            MaxContribution = maxContribution;

            EikanThumbnail.PropertyChanged += DropItem_PropertyChanged;
            HagyoThumbnail.PropertyChanged += DropItem_PropertyChanged;
            ShigokuThumbnail.PropertyChanged += DropItem_PropertyChanged;
            HihiiroThumbnail.PropertyChanged += DropItem_PropertyChanged;
            NoDropThumbnail.PropertyChanged += NoDropThumbnail_PropertyChanged;
        }

        private void DropItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Count":
                    //@todo (暫定) 最終的には、各要素のすべてを足すようにしたい。こんな実装だといつかずれそう
                    NotifyPropertyChanged("BlueDrop");
                    break;
            }
        }

        private void NoDropThumbnail_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Count":
                    //@todo (暫定) 最終的には、各要素のすべてを足すようにしたい。こんな実装だといつかずれそう
                    NotifyPropertyChanged("NoDrop");
                    break;
            }
        }

        // 貢献度 表示文字列
        public string Contribution => string.Format("{0} ～ {1}", MinContribution, MaxContribution);

        /// <summary>
        /// 貢献度最小値
        /// </summary>
        public int MinContribution { get; }
        /// <summary>
        /// 貢献度最大値
        /// </summary>
        public int MaxContribution { get; }
        /// <summary>
        /// [栄冠]のアイテム情報
        /// </summary>
        public DropItemInfomation EikanThumbnail { get; } = new DropItemInfomation("栄冠", Properties.Resources.eikan);
        /// <summary>
        /// [覇業]のアイテム情報
        /// </summary>
        public DropItemInfomation HagyoThumbnail { get; } = new DropItemInfomation("覇業", Properties.Resources.hagyou);
        /// <summary>
        /// [至極]のアイテム情報
        /// </summary>
        public DropItemInfomation ShigokuThumbnail { get; } = new DropItemInfomation("至極", Properties.Resources.sigoku);
        /// <summary>
        /// [ﾋﾋｲﾛ]のアイテム情報
        /// </summary>
        public DropItemInfomation HihiiroThumbnail { get; } = new DropItemInfomation("ﾋﾋｲﾛ", Properties.Resources.hihiiro);
        /// <summary>
        /// [外れ]のアイテム情報
        /// </summary>
        public DropItemInfomation NoDropThumbnail { get; } = new DropItemInfomation("外れ", Properties.Resources.blue);

        /// <summary>
        /// 各ドロップ数の初期化
        /// </summary>
        internal void InitCounts()
        {
            EikanThumbnail.InitCount();
            HagyoThumbnail.InitCount();
            ShigokuThumbnail.InitCount();
            HihiiroThumbnail.InitCount();
            NoDropThumbnail.InitCount();
        }
    }

    /// <summary>
    /// ドロップ情報
    /// </summary>
    internal class DropItemInfomation : ViewModelBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">アイテム名</param>
        /// <param name="icon">アイテムアイコン</param>
        public DropItemInfomation(string name, Bitmap icon)
        {
            Name = name;

            // Bitmapのハンドルを取得し、
            var hBitmap = icon.GetHbitmap();
            // CreateBitmapSourceFromHBitmap()で System.Windows.Media.Imaging.BitmapSource に変換する
            BitmapSource bitmapsource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                        hBitmap,
                                        IntPtr.Zero,
                                        Int32Rect.Empty,
                                        BitmapSizeOptions.FromEmptyOptions());
            Icon = bitmapsource;
        }

        // ドロップ数
        private int _count = 0;

        /// <summary>
        /// アイテム名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// アイコン
        /// </summary>
        public BitmapSource Icon { get; set; }
        /// <summary>
        /// ドロップ数
        /// </summary>
        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }

        internal void InitCount()
        {
            Count = 0;
        }

        /// <summary>
        /// ドロップ数初期化コマンド
        /// </summary>
        public RelayCommand _incrementCountCommand = null;
        public RelayCommand IncrementCountCommand
        {
            get
            {
                return _incrementCountCommand ??= new RelayCommand(
                    () =>
                    {
                        Count++;
                    });
            }
        }
    }
}
