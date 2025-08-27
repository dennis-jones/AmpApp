namespace Zamp.Shared.Extensions;

public static class DateTimeExtensions
{
    public static DateTime? InTimeZone(this DateTime? dateTime, TimeZoneInfo targetTimeZoneInfo)
        => dateTime?.InTimeZone(targetTimeZoneInfo);

    public static DateTime InTimeZone(this DateTime dateTime, TimeZoneInfo targetTimeZoneInfo)
        => dateTime.InTimeZone(DateTimeKind.Utc, targetTimeZoneInfo);

    public static DateTime? InTimeZone(this DateTime? dateTime, DateTimeKind sourceDateTimeKind, TimeZoneInfo targetTimeZoneInfo)
        => dateTime?.InTimeZone(sourceDateTimeKind, targetTimeZoneInfo);

    public static DateTime InTimeZone(this DateTime dateTime, DateTimeKind sourceDateTimeKind, TimeZoneInfo targetTimeZoneInfo)
        => TimeZoneInfo.ConvertTime(DateTime.SpecifyKind(dateTime, sourceDateTimeKind), targetTimeZoneInfo);

    public static string? FormatDate(this DateTime? value) => value?.FormatDate();
    public static string FormatDate(this DateTime value) => value.ToString("MMM d, yyyy").Replace(".", "");

    public static string? FormatTime(this DateTime? value) => value?.FormatTime();

    // public static string FormatTime(this DateTime value) => value.ToString("h:mm tt");
    public static string FormatTime(this DateTime value) => value.ToString("H:mm");
    public static string? FormatDateTime(this DateTime? value) => value?.FormatDateTime();
    public static string FormatDateTime(this DateTime value) => value.ToString("MMM d, yyyy H:mm").Replace(".", "");

    public static DateTime? DateOnly(this DateTime? value) => value?.DateOnly();
    public static DateTime DateOnly(this DateTime value) => value.Date;

    public static DateTime? TimeOnly(this DateTime? value) => value?.TimeOnly();
    public static DateTime TimeOnly(this DateTime value) => new(value.TimeOfDay.Ticks);

    public static long? TimeTicks(this DateTime? value) => value?.TimeOfDay.Ticks;
}