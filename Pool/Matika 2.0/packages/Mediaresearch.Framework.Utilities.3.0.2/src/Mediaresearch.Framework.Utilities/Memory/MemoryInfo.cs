using System;
using System.Runtime.InteropServices;

namespace Mediaresearch.Framework.Utilities.Memory
{
    public class MemoryInfo
    {
        private PERFORMANCE_INFORMATION m_information;

        public MemoryInfo()
        {
            m_information = new PERFORMANCE_INFORMATION();
            uint size = (uint)Marshal.SizeOf(typeof(PERFORMANCE_INFORMATION));
            GetPerformanceInfo(out m_information, size);
        }


        public long GetAvailableMemoryInBytes()
        {
            return (long)m_information.PhysicalAvailable * (long)m_information.PageSize;
        }

        public long GetAvailableMemoryInKB()
        {
            return GetAvailableMemoryInBytes()/1024;
        }

        public long GetAvailableMemoryInMB()
        {
            return GetAvailableMemoryInKB() / 1024;
        }

        public long GetAvailableMemoryInGB()
        {
            return GetAvailableMemoryInMB() / 1024;
        }

        public long GetTotalMemoryInBytes()
        {
            return (long)m_information.PhysicalTotal * (long)m_information.PageSize;
        }

        public long GetTotalMemoryInKB()
        {
            return GetTotalMemoryInBytes()/1024;
        }

        public long GetTotalMemoryInMB()
        {
            return GetTotalMemoryInKB()/1024;
        }

        public long GetTotalMemoryInGB()
        {
            return GetTotalMemoryInMB() / 1024;
        }

        [DllImport("psapi.dll", SetLastError = true)]
        static extern bool GetPerformanceInfo(out PERFORMANCE_INFORMATION pPerformanceInformation, uint cb);
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct PERFORMANCE_INFORMATION
    {
        uint cb;
        public UIntPtr CommitTotal;
        public UIntPtr CommitLimit;
        public UIntPtr CommitPeak;
        public UIntPtr PhysicalTotal;
        public UIntPtr PhysicalAvailable;
        public UIntPtr SystemCache;
        public UIntPtr KernelTotal;
        public UIntPtr KernelPaged;
        public UIntPtr KernelNonpaged;
        public UIntPtr PageSize;
        public uint HandleCount;
        public uint ProcessCount;
        public uint ThreadCount;
    }
}