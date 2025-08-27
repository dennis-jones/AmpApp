namespace Zamp.Shared.Extensions;

public static class LongExtensions
{
    public static string? ToFormattedFileSize(this long? bytes)
    {
        if (bytes == null) { return null; }
        var kb = (long)((bytes + 1023) / 1024);
        return $"{kb:#,0}";
    }

    // This is fancy (1B, 2.2KB, 4.3MB, 13.8GB, etc) but, in a list, it is difficult to determine
    // the relative sized of files
    //public static string? ToFormattedFileSize(this long? bytes)
    //{
    //    if (bytes == null) { return null; }

    //    var unit = 1024;
    //    if (bytes < unit) { return $"{bytes} B"; }

    //    var exp = (int)(Math.Log((double)bytes) / Math.Log(unit));
    //    return $"{bytes / Math.Pow(unit, exp):F1} {("KMGTPE")[exp - 1]}B";
    //}
}

