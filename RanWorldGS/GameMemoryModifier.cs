// PSGG.64 Exfarm, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// GameMemoryModifier
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

public class GameMemoryModifier
{
    [Flags]
    public enum ProcessAccessFlags : uint
    {
        All = 0x1F0FFFu
    }

    private string targetProcessName;

    private int targetBaseAddress;

    private byte[] newBytes;

    private volatile bool shouldStop;

    public GameMemoryModifier(string processName, int baseAddress, byte[] bytesToWrite)
    {
        targetProcessName = processName;
        targetBaseAddress = baseAddress;
        newBytes = bytesToWrite;
        shouldStop = false;
    }

    public void MonitorAndModifyMemory()
    {
        int num = 0;
        while (!shouldStop)
        {
            Process[] processesByName = Process.GetProcessesByName(targetProcessName);
            if (processesByName.Length == 0)
            {
                num = 0;
            }
            Process[] array = processesByName;
            for (int i = 0; i < array.Length; i++)
            {
                int id = array[i].Id;
                if (id == num)
                {
                    continue;
                }
                num = id;
                Console.WriteLine($"Found {targetProcessName} with PID: {id}");
                IntPtr intPtr = OpenProcess(ProcessAccessFlags.All, bInheritHandle: false, id);
                if (intPtr != IntPtr.Zero)
                {
                    for (int j = 0; j < newBytes.Length; j++)
                    {
                        WriteProcessMemory(intPtr, (IntPtr)(targetBaseAddress + j), new byte[1] { newBytes[j] }, 1u, out var _);
                    }
                    CloseHandle(intPtr);
                }
            }
            Thread.Sleep(50);
        }
    }

    public void RequestStop()
    {
        shouldStop = true;
    }

    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CloseHandle(IntPtr hObject);
}
