using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Posme.Maui.Services.Repository;
using Unity;

namespace Posme.Maui.Services.Helpers;

public class BluetoothPrinterService
{
    private readonly IAdapter _adapter;
    private readonly IBluetoothLE _bluetoothLE;
    private readonly IRepositoryTbParameterSystem _repositoryTbParameterSystem;
    public string PrinterName = String.Empty;
    public BluetoothPrinterService()
    {
        _bluetoothLE = CrossBluetoothLE.Current;
        _adapter = CrossBluetoothLE.Current.Adapter;
        _repositoryTbParameterSystem = VariablesGlobales.UnityContainer.Resolve<IRepositoryTbParameterSystem>();
    }

    public async Task<bool> IsPrinterConnectedAsync()
    {
        if (_bluetoothLE.State != BluetoothState.On)
        {
            return false;
        }

        var connectedDevices = _adapter.GetSystemConnectedOrPairedDevices();

        var printerParameter = await _repositoryTbParameterSystem.PosMeFindPrinter();
        PrinterName = printerParameter.Value!;
        return connectedDevices.Any(device => device.Name.Contains("Printer") || device.Name.Contains(printerParameter.Value!));
    }
    
    public async Task<bool> CheckAndRequestBluetoothPermissionsAsync()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }

        if (status != PermissionStatus.Granted)
        {
            return false;
        }

        return true;
    }
}