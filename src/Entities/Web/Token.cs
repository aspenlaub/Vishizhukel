using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Web {
    public class Token : INotifyPropertyChanged {
        public const int ExpireInMinutes = 10;

        [Key]
        public string Guid { get; set; }

        public string SingleUseGuid { get => _SingleUseGuid; set { _SingleUseGuid = value; OnPropertyChanged(nameof(SingleUseGuid)); } }
        private string _SingleUseGuid;

        public bool SingleUseGuidUsed { get => _SingleUseGuidUsed; set { _SingleUseGuidUsed = value; OnPropertyChanged(nameof(SingleUseGuidUsed)); } }
        private bool _SingleUseGuidUsed;

        public DateTime ExpireTime { get => _ExpireTime; set { _ExpireTime = value; OnPropertyChanged(nameof(ExpireTime)); } }
        private DateTime _ExpireTime;

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
}