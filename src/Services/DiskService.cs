using ConStat.Abstractions;
using ConStat.Models;

namespace ConStat.Services;

public class DiskService : IDiskService
{
    public List<DiskInfo> GetDisksInfo()
    {
        var list = new List<DiskInfo>();
        try
        {
            foreach (var di in DriveInfo.GetDrives())
            {
                try
                {
                    if (!di.IsReady) continue;
                    var total = (double)di.TotalSize;
                    var free = (double)di.AvailableFreeSpace;
                    var freePercent = total > 0 ? (free / total) * 100.0 : 0.0;
                    var used = Math.Max(0.0, total - free);
                    var usedPercent = total > 0 ? (used / total) * 100.0 : 0.0;
                    var description =
                        $"{FormatBytes(used)} / {FormatBytes(total)} ({usedPercent:0.0}%); Free: {FormatBytes(free)} ({freePercent:0.0}%)";
                    list.Add(new DiskInfo(di.Name, description));
                }
                catch
                {
                    // ignore per-drive errors
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Disk read failed: {ex.Message}");
        }

        return list;
    }

    private string FormatBytes(double bytes)
    {
        const double KB = 1024;
        const double MB = KB * 1024;
        const double GB = MB * 1024;
        if (bytes >= GB) return $"{bytes / GB:0.##} GB";
        if (bytes >= MB) return $"{bytes / MB:0.#} MB";
        if (bytes >= KB) return $"{bytes / KB:0.#} KB";
        return $"{bytes:0} B";
    }
}