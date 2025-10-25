using ConStat.Models;

namespace ConStat.Abstractions;

public interface IGpuService
{
    List<GpuInfo> GetGpuInfos();
}