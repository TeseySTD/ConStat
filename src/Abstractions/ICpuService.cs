namespace ConStat.Abstractions;

public interface ICpuService
{
    string GetCpuName();
    double GetCpuUsage();
}