using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Web {
    public class Token : INotifyPropertyChanged {
        public const int ExpireInMinutes = 10;

        [Key]
        public string Guid { get; set; }

        public string SingleUseGuid { get => vSingleUseGuid; set { vSingleUseGuid = value; OnPropertyChanged(nameof(SingleUseGuid)); } }
        private string vSingleUseGuid;

        public bool SingleUseGuidUsed { get => vSingleUseGuidUsed; set { vSingleUseGuidUsed = value; OnPropertyChanged(nameof(SingleUseGuidUsed)); } }
        private bool vSingleUseGuidUsed;

        public DateTime ExpireTime { get => vExpireTime; set { vExpireTime = value; OnPropertyChanged(nameof(ExpireTime)); } }
        private DateTime vExpireTime;

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