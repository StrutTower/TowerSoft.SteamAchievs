using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TowerSoft.SteamAchievs.Website.ViewModels {
    public class LoginViewModel {
        [Required, Display(Prompt = "Username")]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password), Display(Prompt = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remeber my login on this device")]
        public bool RememberLogin { get; set; }
    }
}
