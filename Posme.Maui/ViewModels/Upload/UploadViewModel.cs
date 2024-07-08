using Posme.Maui.Services.Api;
using Posme.Maui.Services.Repository;
using Posme.Maui.Services.SystemNames;
using Unity;

namespace Posme.Maui.ViewModels.Upload;

public class UploadViewModel : BaseViewModel
{
    private readonly IRepositoryItems _repositoryItems;
    private readonly IRepositoryTbCustomer _repositoryTbCustomer;
    private readonly IRepositoryTbTransactionMaster _repositoryTbTransactionMaster;
    private readonly IRepositoryTbTransactionMasterDetail _repositoryTbTransactionMasterDetail;

    public UploadViewModel()
    {
        Title = "Suibr Datos";
        _repositoryItems = VariablesGlobales.UnityContainer.Resolve<IRepositoryItems>();
        _repositoryTbCustomer = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbCustomer>();
        _repositoryTbTransactionMaster = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbTransactionMaster>();
        _repositoryTbTransactionMasterDetail = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbTransactionMasterDetail>();
        UploadCommand = new Command(OnUploadCommand, ValidateUpload);
        PropertyChanged += (_, _) => UploadCommand.ChangeCanExecute();
    }

    private async void OnUploadCommand()
    {
        var api = new RestApiAppMobileApi();
        await api.SendDataAsync();
    }

    private bool ValidateUpload()
    {
        return Connectivity.Current.NetworkAccess != NetworkAccess.None && Switch;
    }

    private async Task LoadData()
    {
        var findCustomers = await _repositoryTbCustomer.PosMeTakeModificados();
        var findItems = await _repositoryItems.PosMeTakeModificado();
        var findTransactionMaster = await _repositoryTbTransactionMaster.PosMeFindAll();
        var findTransactionMasterDetail = await _repositoryTbTransactionMasterDetail.PosMeFindAll();
    }

    private bool _switch;

    public bool Switch
    {
        get => _switch;
        set => SetProperty(ref _switch, value);
    }

    public Command UploadCommand { get; }

    public void OnAppearing(INavigation navigation)
    {
        Navigation = navigation;
        IsBusy = false;
    }
}