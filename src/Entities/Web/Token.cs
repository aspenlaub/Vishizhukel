using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Web {
    public class Token : INotifyPropertyChanged {
        public const int ExpireInMinutes = 10;

        [Key]
        public string Guid { get; set; }

        public string SingleUseGuid { get => PrivateSingleUseGuid; set { PrivateSingleUseGuid = value; OnPropertyChanged(nameof(SingleUseGuid)); } }
        private string PrivateSingleUseGuid;

        public bool SingleUseGuidUsed { get => PrivateSingleUseGuidUsed; set { PrivateSingleUseGuidUsed = value; OnPropertyChanged(nameof(SingleUseGuidUsed)); } }
        private bool PrivateSingleUseGuidUsed;

        public DateTime ExpireTime { get => PrivateExpireTime; set { PrivateExpireTime = value; OnPropertyChanged(nameof(ExpireTime)); } }
        private DateTime PrivateExpireTime;

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