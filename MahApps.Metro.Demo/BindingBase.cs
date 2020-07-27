using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class BindingBase : ICloneable, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public bool SetProperty<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

        storage = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }

    public void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public System.Windows.Application Current { get { return System.Windows.Application.Current; } }


    public object Clone()
    {
        return this as object;
    }

    #region Extension
    private bool isLoading;

    [XmlIgnore]
    public bool IsLoading
    {
        get { return isLoading; }
        set { SetProperty(ref isLoading, value); }
    }


    private string logicStatus;

    [XmlIgnore]
    public string LogicStatus
    {
        get { return logicStatus; }
        set { SetProperty(ref logicStatus, value); }
    }

    private int logicStatusFlag;
    /// <summary>
    /// 狀態等級0正常, 1成功，2警告，3危險
    /// </summary>
    [XmlIgnore]
    public int LogicStatusFlag
    {
        get { return logicStatusFlag; }
        set { SetProperty(ref logicStatusFlag, value); }
    }

    public void General(string status)
    {
        LogicStatus = status;
        LogicStatusFlag = 0;
    }

    public void Success(string status)
    {
        LogicStatus = status;
        LogicStatusFlag = 1;
    }
    public void Warnning(string status)
    {
        LogicStatus = status;
        LogicStatusFlag = 2;
    }
    public void Dagerous(string status)
    {
        LogicStatus = status;
        LogicStatusFlag = 3;
    }
    #endregion
}

/// <summary>
/// 擴充類
/// </summary>
public static class BindingExtenssion
{
    public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> range)
    {
        foreach (var item in range)
            collection.Add(item);
    }
}
