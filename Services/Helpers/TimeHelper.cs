namespace ProvaPub.Services.Helpers
{
    public static class TimeHelper
    {
        public static TimeZoneInfo SaoPauloTimeZone =>
            TryFind("America/Sao_Paulo") ?? TryFind("E. South America Standard Time")
            ?? TimeZoneInfo.Utc;

        private static TimeZoneInfo? TryFind(string id)
        {
            try { return TimeZoneInfo.FindSystemTimeZoneById(id); }
            catch { return null; }
        }

        public static (DateTimeOffset StartUtc, DateTimeOffset EndUtc) GetMonthBoundsUtc(DateTimeOffset nowLocal, TimeZoneInfo tz)
        {
            var local = TimeZoneInfo.ConvertTime(nowLocal, tz);
            var startLocal = new DateTime(local.Year, local.Month, 1, 0, 0, 0, DateTimeKind.Unspecified);
            var start = new DateTimeOffset(startLocal, local.Offset);
            var end = start.AddMonths(1);
            return (start.ToUniversalTime(), end.ToUniversalTime());
        }
    }
}
