using ConStat.Abstractions;
using Hardware.Info;

namespace ConStat.Services;

public class RamService : IRamService
{
    private readonly HardwareInfo _hardwareInfo;

    public RamService()
    {
        _hardwareInfo = new HardwareInfo();
    }

    public string GetRamInfo()
    {
        try
        {
            _hardwareInfo.RefreshMemoryStatus();

            var memStatus = _hardwareInfo.MemoryStatus;

            if (memStatus == null)
            {
                return "N/A";
            }

            double totalBytes = memStatus.TotalPhysical;
            double availableBytes = memStatus.AvailablePhysical;

            double totalMb = totalBytes / (1024 * 1024);
            double availableMb = availableBytes / (1024 * 1024);
            double usedMb = totalMb - availableMb;

            double percent = totalMb > 0 ? (usedMb / totalMb) * 100.0 : 0.0;

            return $"{usedMb:0} MB / {totalMb:0} MB ({percent:0.0}%)";
        }
        catch (Exception ex)
        {
            throw new Exception($"RAM read failed: {ex.Message}");
        }
    }
}