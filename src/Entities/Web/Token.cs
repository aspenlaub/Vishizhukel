using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Web;

public class Token : INotifyPropertyChanged {
    public const int ExpireInMinutes = 10;

    [Key]
    public string Guid { get; set; }

    public string SingleUseGuid {
        get;
        set { field = value; OnPropertyChanged(nameof(SingleUseGuid)); }
    }

    public bool SingleUseGuidUsed {
        get;
        set { field = value; OnPropertyChanged(nameof(SingleUseGuidUsed)); }
    }

    public DateTime ExpireTime {
        get;
        set { field = value; OnPropertyChanged(nameof(ExpireTime)); }
    }

    public Token() {
        Guid = System.Guid.NewGuid().ToString();
        SingleUseGuid = System.Guid.NewGuid().ToString();
        SingleUseGuidUsed = false;
        ExpireTime = DateTime.Now.AddMinutes(ExpireInMinutes);
    }

    protected void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler PropertyChanged;
}