namespace Utilities
{
public static class DateTimeExtensions
{
    public static DateTime RoundToTicks(this DateTime target, long ticks) => new((target.Ticks + ticks / 2) / ticks * ticks, target.Kind);
    public static DateTime RoundUpToTicks(this DateTime target, long ticks) => new((target.Ticks + ticks - 1) / ticks * ticks, target.Kind);
    public static DateTime RoundDownToTicks(this DateTime target, long ticks) => new(target.Ticks / ticks * ticks, target.Kind);

    public static DateTime Round(this DateTime target, TimeSpan round) => RoundToTicks(target, round.Ticks);
    public static DateTime RoundUp(this DateTime target, TimeSpan round) => RoundUpToTicks(target, round.Ticks);
    public static DateTime RoundDown(this DateTime target, TimeSpan round) => RoundDownToTicks(target, round.Ticks);

    public static DateTime RoundToMinutes(this DateTime target, int minutes = 1) => RoundToTicks(target, minutes * TimeSpan.TicksPerMinute);
    public static DateTime RoundUpToMinutes(this DateTime target, int minutes = 1) => RoundUpToTicks(target, minutes * TimeSpan.TicksPerMinute);
    public static DateTime RoundDownToMinutes(this DateTime target, int minutes = 1) => RoundDownToTicks(target, minutes * TimeSpan.TicksPerMinute);

    public static DateTime RoundToHours(this DateTime target, int hours = 1) => RoundToTicks(target, hours * TimeSpan.TicksPerHour);
    public static DateTime RoundUpToHours(this DateTime target, int hours = 1) => RoundUpToTicks(target, hours * TimeSpan.TicksPerHour);
    public static DateTime RoundDownToHours(this DateTime target, int hours = 1) => RoundDownToTicks(target, hours * TimeSpan.TicksPerHour);

    public static DateTime RoundToDays(this DateTime target, int days = 1) => RoundToTicks(target, days * TimeSpan.TicksPerDay);
    public static DateTime RoundUpToDays(this DateTime target, int days = 1) => RoundUpToTicks(target, days * TimeSpan.TicksPerDay);
    public static DateTime RoundDownToDays(this DateTime target, int days = 1) => RoundDownToTicks(target, days * TimeSpan.TicksPerDay);
}

public static class TimeOnlyExtensions
{
    public static TimeOnly RoundToTicks(this TimeOnly target, long ticks) => new((target.Ticks + ticks / 2) / ticks * ticks);
    public static TimeOnly RoundUpToTicks(this TimeOnly target, long ticks) => new((target.Ticks + ticks - 1) / ticks * ticks);
    public static TimeOnly RoundDownToTicks(this TimeOnly target, long ticks) => new(target.Ticks / ticks * ticks);

    public static TimeOnly RoundToMinutes(this TimeOnly target, int minutes = 1) => RoundToTicks(target, minutes * TimeSpan.TicksPerMinute);
    public static TimeOnly RoundUpToMinutes(this TimeOnly target, int minutes = 1) => RoundUpToTicks(target, minutes * TimeSpan.TicksPerMinute);
    public static TimeOnly RoundDownToMinutes(this TimeOnly target, int minutes = 1) => RoundDownToTicks(target, minutes * TimeSpan.TicksPerMinute);

    public static TimeOnly RoundToHours(this TimeOnly target, int hours = 1) => RoundToTicks(target, hours * TimeSpan.TicksPerHour);
    public static TimeOnly RoundUpToHours(this TimeOnly target, int hours = 1) => RoundUpToTicks(target, hours * TimeSpan.TicksPerHour);
    public static TimeOnly RoundDownToHours(this TimeOnly target, int hours = 1) => RoundDownToTicks(target, hours * TimeSpan.TicksPerHour);

    public static TimeOnly RoundToDays(this TimeOnly target, int days = 1) => RoundToTicks(target, days * TimeSpan.TicksPerDay);
    public static TimeOnly RoundUpToDays(this TimeOnly target, int days = 1) => RoundUpToTicks(target, days * TimeSpan.TicksPerDay);
    public static TimeOnly RoundDownToDays(this TimeOnly target, int days = 1) => RoundDownToTicks(target, days * TimeSpan.TicksPerDay);
}
}