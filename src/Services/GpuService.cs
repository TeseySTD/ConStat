using Hardware.Info;
using ConStat.Abstractions;
using ConStat.Models;

namespace ConStat.Services;

public class GpuService : IGpuService
{
    private readonly HardwareInfo _hardwareInfo;

    public GpuService()
    {
        _hardwareInfo = new HardwareInfo();
    }

#pragma warning disable CA1416
    public List<GpuInfo> GetGpuInfos()
    {
        _hardwareInfo.RefreshVideoControllerList();
        var vidList = _hardwareInfo.VideoControllerList;

        List<GpuInfo> gpuNames = vidList
            .Select(vid => new GpuInfo(vid.Name, (int)(vid.AdapterRAM / (1024 * 1024))))
            .ToList();
        return gpuNames;
    }
#pragma warning restore CA1416
}