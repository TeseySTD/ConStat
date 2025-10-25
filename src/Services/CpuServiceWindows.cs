using System.Runtime.InteropServices;
using ConStat.Abstractions;

namespace ConStat.Services;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public class CpuServiceWindows : ICpuService
{
    private double _lastCpuPercent;
    private ulong _prevIdle;
    private ulong _prevTotal;
    private const double SmoothingAlpha = 0.3;

    public CpuServiceWindows()
    {
        TrySample(out _prevIdle, out _prevTotal);
    }

    public double GetCpuUsage()
    {
        if (!TrySample(out var idle, out var total))
        {
            throw new InvalidOperationException("Failed to retrieve system times.");
        }

        var totalDelta = total - _prevTotal;
        var idleDelta = idle - _prevIdle;

        double usage = 0.0;

        if (totalDelta > 0)
        {
            usage = (double)(totalDelta - idleDelta) / totalDelta * 100.0;
            usage = Math.Max(0, Math.Min(100, usage));
        }

        _prevIdle = idle;
        _prevTotal = total;
        
        _lastCpuPercent = SmoothingAlpha * usage + (1 - SmoothingAlpha) * _lastCpuPercent;
        _lastCpuPercent = Math.Round(_lastCpuPercent, 2);

        return _lastCpuPercent;
    }

    private bool TrySample(out ulong idleOut, out ulong totalOut)
    {
        idleOut = 0;
        totalOut = 0;
        if (!GetSystemTimes(out var idle, out var kernel, out var user))
            return false;

        var idleTicks = FileTimeToUInt64(idle);
        var kernelTicks = FileTimeToUInt64(kernel);
        var userTicks = FileTimeToUInt64(user);
        var totalTicks = kernelTicks + userTicks;

        idleOut = idleTicks;
        totalOut = totalTicks;
        return true;
    }

    private static ulong FileTimeToUInt64(FILETIME ft)
    {
        return ((ulong)ft.dwHighDateTime << 32) | ft.dwLowDateTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    // ReSharper disable once InconsistentNaming
    private struct FILETIME
    {
        public uint dwLowDateTime;
        public uint dwHighDateTime;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetSystemTimes(out FILETIME idleTime, out FILETIME kernelTime, out FILETIME userTime);
}