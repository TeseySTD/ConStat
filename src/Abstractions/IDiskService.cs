using ConStat.Models;

namespace ConStat.Abstractions;

public interface IDiskService
{
    List<DiskInfo> GetDisksInfo();
}