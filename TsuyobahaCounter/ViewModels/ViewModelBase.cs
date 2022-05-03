using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TsuyobahaCounter.ViewModels
{
    /// <summary>
    /// ViewModel.ベースクラス
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {

        /// <summary>
        /// プロパティ変更イベント
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// PropertyChanged()呼び出し
        /// </summary>
        /// <param name="propertyName">イベントを発生させたいプロパティ名</param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            // イベント発生
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (object.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            // イベント発生
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            return true;
        }
    }
}
