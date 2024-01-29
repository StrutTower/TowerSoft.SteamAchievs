using System.Net.Http.Json;
using TowerSoft.SteamAchievs.Blazor.Client.Utilities;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Blazor.Shared.ViewModels;

namespace TowerSoft.SteamAchievs.Blazor.Client.Services {
    public class CompanyDataService {
        private readonly HttpClient http;

        public CompanyDataService(HttpClient http) {
            this.http = http;
        }

        public async Task<CompanyModel[]> GetAll() {
            return await http.GetFromJsonAsync<CompanyModel[]>("api/company");
        }

        public async Task<CompanyModel> GetByID(long id) {
            return await http.GetFromJsonAsync<CompanyModel>("api/company/" + id);
        }

        public async Task UpdateCompany(CompanyModel company) {
            await http.PutAsJsonAsync("api/company", company);
        }

        public async Task<List<GameCompanyViewModel>> GetGamesByCompanyID(long companyID) {
            SteamGameModel[] games = await http.GetFromJsonAsync<SteamGameModel[]>("api/SteamGame/GetByCompanyID/" + companyID);

            Dictionary<long, CompanyModel> companies = (await GetAll()).ToDictionary(x => x.ID);
            List<GameCompanyModel> gameCompanies = await http.PostGetFromJson<List<GameCompanyModel>>("api/GameCompany/GetByIDs", games.Select(x => x.ID));

            List<GameCompanyViewModel> models = new();
            foreach (SteamGameModel game in games) {
                List<GameCompanyModel> gcs = gameCompanies.Where(x => x.SteamGameID == game.ID).ToList();

                GameCompanyViewModel model = new() {
                    SteamGame = game,
                    Developers = new(),
                    Publishers = new()
                };

                foreach(GameCompanyModel gc in gcs) {
                    CompanyModel company = companies[gc.CompanyID];
                    if (gc.IsDeveloper) {
                        model.Developers.Add(company);
                    } else {
                        model.Publishers.Add(company);
                    }
                }


                models.Add(model);
            }

            return models;
        }
    }
}
