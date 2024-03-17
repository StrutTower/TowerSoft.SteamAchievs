using System.Net.Http.Json;
using TowerSoft.SteamAchievs.Blazor.Shared.Models;
using TowerSoft.SteamAchievs.Blazor.Shared.ViewModels;

namespace TowerSoft.SteamAchievs.Blazor.Client.Services {
    public class TableListDataService {
        private readonly HttpClient http;

        public TableListDataService(HttpClient http) {
            this.http = http;
        }

        public async Task<TableListManageModel> GetTableListManageModel(long steamGameID) {
            TableListManageModel model = new() {
                SteamGame = await http.GetFromJsonAsync<SteamGameModel>("api/SteamGame/" + steamGameID),
                TableLists = await http.GetFromJsonAsync<List<TableListModel>>("api/TableList/SteamGameID/" + steamGameID)
            };

            return model;
        }

        public async Task<ManageTableListModel> GetManageTableListModel(long tableListID) {
            ManageTableListModel model = new() {
                TableList = await http.GetFromJsonAsync<TableListModel>("api/TableList/" + tableListID),
                Columns = [],
                Rows = []
            };
            model.SteamGame = await http.GetFromJsonAsync<SteamGameModel>("api/SteamGame/" + model.TableList.SteamGameID);

            List<TableColumnModel> cols = await http.GetFromJsonAsync<List<TableColumnModel>>("api/TableColumn/TableListID/" + model.TableList.ID);
            List<TableColumnChoiceModel> choices = await http.GetFromJsonAsync<List<TableColumnChoiceModel>>("api/TableColumnChoice/TableListID/" + model.TableList.ID);
            List<TableRowModel> rows = await http.GetFromJsonAsync<List<TableRowModel>>("api/TableRow/TableListID/" + model.TableList.ID);
            List<TableDataModel> datas = await http.GetFromJsonAsync<List<TableDataModel>>("api/TableData/TableListID/" + model.TableList.ID);

            foreach (TableColumnModel col in cols) {
                ManageTableColumnViewModel columnModel = new() {
                    Column = col,
                    Choices = choices.Where(x => x.TableColumnID == col.ID).ToList()
                };
                model.Columns.Add(columnModel);
            }

            foreach (TableRowModel row in rows) {
                TableRowViewModel rowModel = new() {
                    Row = row,
                    Columns = []
                };
                foreach (TableColumnModel col in cols) {
                    TableRowColumnViewModel rowColModel = new() {
                        Column = col,
                        Value = datas.SingleOrDefault(x => x.TableRowID == row.ID && x.TableColumnID == col.ID)
                    };

                    if (rowColModel.Value == null)
                        rowColModel.Value = new() { TableRowID = row.ID, TableColumnID = col.ID };

                    if (rowColModel.Value.ChoiceID.HasValue) {
                        rowColModel.SelectedChoice = choices.SingleOrDefault(x => x.ID == rowColModel.Value.ChoiceID.Value);
                    }
                    rowModel.Columns.Add(rowColModel);
                }
                model.Rows.Add(rowModel);
            }

            return model;
        }

        public async Task<EditTableListViewModel> GetEditTableListViewModel(long? id = null, long? steamGameID = null) {
            EditTableListViewModel model = new();
            if (id.HasValue) {
                model.TableList = await http.GetFromJsonAsync<TableListModel>("api/TableList/" + id.Value);
                model.SteamGame = await http.GetFromJsonAsync<SteamGameModel>("api/SteamGame/" + model.TableList.SteamGameID);
            } else if (steamGameID.HasValue) {
                model.SteamGame = await http.GetFromJsonAsync<SteamGameModel>("api/SteamGame/" + steamGameID.Value);
                model.TableList = new TableListModel { SteamGameID = steamGameID.Value, IsActive = true };
            }

            return model;
        }

        public async Task AddOrUpdateTableList(TableListModel tableList) {
            if (tableList.ID == 0) {
                await http.PostAsJsonAsync("api/TableList", tableList);
            } else {
                await http.PutAsJsonAsync("api/TableList", tableList);
            }
        }

        public async Task<List<TableListViewModel>> GetTableListModels(long steamGameID) {
            List<TableListViewModel> models = [];

            List<TableListModel> tableLists = await http.GetFromJsonAsync<List<TableListModel>>("api/TableList/SteamGameID/" + steamGameID);
            List<TableColumnModel> tableColumns = await http.GetFromJsonAsync<List<TableColumnModel>>("api/TableColumn/SteamGameID/" + steamGameID);
            List<TableColumnChoiceModel> tableColumnChoices = await http.GetFromJsonAsync<List<TableColumnChoiceModel>>("api/TableColumnChoice/SteamGameID/" + steamGameID);
            List<TableRowModel> tableRows = await http.GetFromJsonAsync<List<TableRowModel>>("api/TableRow/SteamGameID/" + steamGameID);
            List<TableDataModel> tableValues = await http.GetFromJsonAsync<List<TableDataModel>>("api/TableValue/SteamGameID/" + steamGameID);

            foreach (TableListModel tableList in tableLists) {
                TableListViewModel model = new() {
                    TableList = tableList,
                    Rows = []
                };

                List<TableColumnModel> columns = tableColumns.Where(x => x.TableListID == tableList.ID).ToList();
                List<TableRowModel> rows = tableRows.Where(x => x.TableListID == tableList.ID).ToList();

                foreach (TableRowModel row in rows) {
                    TableRowViewModel rowModel = new() {
                        Row = row,
                        Columns = []
                    };
                    foreach (TableColumnModel col in columns.OrderBy(x => x.SortValue).ThenBy(x => x.Title)) {
                        TableDataModel value = tableValues.SingleOrDefault(x => x.TableRowID == row.ID && x.TableColumnID == col.ID);
                        TableRowColumnViewModel rowColModel = new() {
                            Column = col,
                            Value = value,
                        };
                        if (value.ChoiceID.HasValue &&
                            (col.DataType == Blazor.Shared.Enums.ColumnDataType.RadioButtons ||
                             col.DataType == Blazor.Shared.Enums.ColumnDataType.SelectList)) {
                            rowColModel.SelectedChoice = tableColumnChoices.SingleOrDefault(x => x.ID == value.ChoiceID.Value);
                        }

                        rowModel.Columns.Add(rowColModel);
                    }
                    model.Rows.Add(rowModel);
                }

                models.Add(model);
            }
            return models;
        }

        public async Task<EditTableColumnModel> GetEditTableColumnModel(long? tableColumnID = null, long? tableListID = null) {
            EditTableColumnModel model = new();
            if (tableColumnID.HasValue) {
                model.TableColumn = await http.GetFromJsonAsync<TableColumnModel>("api/TableColumn/" + tableColumnID.Value);
                model.TableList = await http.GetFromJsonAsync<TableListModel>("api/TableList/" + model.TableColumn.TableListID);
            } else if (tableListID.HasValue) {
                model.TableList = await http.GetFromJsonAsync<TableListModel>("api/TableList/" + tableListID.Value);
                model.TableColumn = new() {
                    TableListID = model.TableList.ID,
                    IsActive = true
                };
            }

            model.SteamGame = await http.GetFromJsonAsync<SteamGameModel>("api/SteamGame/" + model.TableList.SteamGameID);

            return model;
        }

        public async Task AddOrUpdateTableColumn(TableColumnModel tableColumn) {
            if (tableColumn.ID == 0) {
                await http.PostAsJsonAsync("api/TableColumn", tableColumn);
            } else {
                await http.PutAsJsonAsync("api/TableColumn", tableColumn);
            }
        }

        public async Task<EditTableRowModel> GetEditTableRowModel(long? id = null, long? tableListID = null) {
            EditTableRowModel model = new() { EditModel = new() };
            if (id.HasValue) {
                model.EditModel.TableRow = await http.GetFromJsonAsync<TableRowModel>("api/TableRow/" + id.Value);
                model.TableList = await http.GetFromJsonAsync<TableListModel>("api/TableList/" + model.EditModel.TableRow.TableListID);
            } else if (tableListID.HasValue) {
                model.TableList = await http.GetFromJsonAsync<TableListModel>("api/TableList/" + tableListID.Value);
                model.EditModel.TableRow = new() { TableListID = model.TableList.ID, IsActive = true };
            }

            model.SteamGame = await http.GetFromJsonAsync<SteamGameModel>("api/SteamGame/" + model.TableList.SteamGameID);

            var columns = await http.GetFromJsonAsync<List<TableColumnModel>>("api/TableColumn/TableListID/" + model.TableList.ID);
            var datas = await http.GetFromJsonAsync<List<TableDataModel>>("api/TableData/TableListID/" + model.TableList.ID);
            var choices = await http.GetFromJsonAsync<List<TableColumnChoiceModel>>("api/TableColumnChoice/TableListID/" + model.TableList.ID);

            foreach (var col in columns.Where(x => x.IsActive).OrderBy(x => x.SortValue).ThenBy(x => x.Title)) {
                TableDataModel data = datas.SingleOrDefault(x => x.TableColumnID == col.ID && x.TableRowID == model.EditModel.TableRow.ID);
                if (data == null) {
                    data = new() { TableColumnID = col.ID, TableRowID = model.EditModel.TableRow.ID };
                }
                List<TableColumnChoiceModel> colChoices = choices.Where(x => x.TableColumnID == col.ID).ToList();

                model.EditModel.ColumnValues.Add(new TableRowColEditModel {
                    ColumnID = col.ID,
                    Title = col.Title,
                    DataType = col.DataType,
                    SortValue = col.SortValue,
                    Data = data,
                    Choices = colChoices
                });
            }
            return model;
        }

        public async Task AddOrUpdateTableRow(TableRowEditModel model) {
            if (model.TableRow.ID == 0) {
                await http.PostAsJsonAsync("api/TableRow", model);
            } else {
                await http.PutAsJsonAsync("api/TableRow", model);
            }
        }

        public async Task<ManageChoicesModel> GetManageChoicesModel(long tableColumnID) {
            ManageChoicesModel model = new() {
                TableColumn = await http.GetFromJsonAsync<TableColumnModel>("api/TableColumn/" + tableColumnID)
            };

            model.TableList = await http.GetFromJsonAsync<TableListModel>("api/TableList/" + model.TableColumn.TableListID);
            model.Choices = await http.GetFromJsonAsync<List<TableColumnChoiceModel>>("api/TableColumnChoice/TableColumnID/" + tableColumnID);
            model.SteamGame = await http.GetFromJsonAsync<SteamGameModel>("api/SteamGame/" + model.TableList.SteamGameID);
            return model;
        }

        public async Task<EditChoiceModel> GetEditChoiceModel(long? id = null, long? tableColumnID = null) {
            EditChoiceModel model = new();
            if (id.HasValue) {
                model.TableColumnChoice = await http.GetFromJsonAsync<TableColumnChoiceModel>("api/TableColumnChoice/" + id.Value);
                model.TableColumn = await http.GetFromJsonAsync<TableColumnModel>("api/TableColumn/" + model.TableColumnChoice.TableColumnID);
            } else if (tableColumnID.HasValue) {
                model.TableColumn = await http.GetFromJsonAsync<TableColumnModel>("api/TableColumn/" + tableColumnID.Value);
                model.TableColumnChoice = new() { TableColumnID = tableColumnID.Value, IsActive = true };
            }

            model.TableList = await http.GetFromJsonAsync<TableListModel>("api/TableList/" + model.TableColumn.TableListID);
            model.SteamGame = await http.GetFromJsonAsync<SteamGameModel>("api/SteamGame/" + model.TableList.SteamGameID);
            return model;
        }

        public async Task AddOrUpdateTableColumnChoice(TableColumnChoiceModel tableColumnChoice) {
            if (tableColumnChoice.ID == 0) {
                await http.PostAsJsonAsync("api/TableColumnChoice", tableColumnChoice);
            } else {
                await http.PutAsJsonAsync("api/TableColumnChoice", tableColumnChoice);
            }
        }

        public async Task UpdateRowCompletion(long rowID, bool isCompleted) {
            TableRowModel model = new TableRowModel { ID = rowID, IsComplete = isCompleted };
            await http.PostAsJsonAsync("api/TableRow/Completion", model);
        }
    }
}
