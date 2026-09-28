using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

public static class NativeEntryIcon
{
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern IntPtr BeginUpdateResource(string file, bool deleteExisting);
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool UpdateResource(IntPtr update, IntPtr type, IntPtr name, ushort language, byte[] data, uint size);
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool EndUpdateResource(IntPtr update, bool discard);

    public static void Apply(string executable, string icon)
    {
        byte[] bytes = File.ReadAllBytes(icon);
        IntPtr update = BeginUpdateResource(executable, false);
        if (update == IntPtr.Zero) throw new Win32Exception();
        bool committed = false;
        try
        {
            using (var input = new BinaryReader(new MemoryStream(bytes)))
            using (var group = new MemoryStream())
            using (var writer = new BinaryWriter(group))
            {
                if (input.ReadUInt16() != 0 || input.ReadUInt16() != 1) throw new InvalidDataException("Invalid icon header.");
                ushort count = input.ReadUInt16();
                writer.Write((ushort)0); writer.Write((ushort)1); writer.Write(count);
                for (ushort i = 0; i < count; i++)
                {
                    writer.Write(input.ReadBytes(8));
                    uint size = input.ReadUInt32(), offset = input.ReadUInt32();
                    writer.Write(size); writer.Write((ushort)(i + 1));
                    if ((ulong)offset + size > (ulong)bytes.Length) throw new InvalidDataException("Invalid icon image.");
                    byte[] image = new byte[checked((int)size)];
                    Buffer.BlockCopy(bytes, checked((int)offset), image, 0, image.Length);
                    if (!UpdateResource(update, new IntPtr(3), new IntPtr(i + 1), 0, image, size)) throw new Win32Exception();
                }
                byte[] resource = group.ToArray();
                if (!UpdateResource(update, new IntPtr(14), new IntPtr(1), 0, resource, (uint)resource.Length)) throw new Win32Exception();
            }
            if (!EndUpdateResource(update, false)) throw new Win32Exception();
            committed = true;
        }
        finally { if (!committed) EndUpdateResource(update, true); }
    }
}
