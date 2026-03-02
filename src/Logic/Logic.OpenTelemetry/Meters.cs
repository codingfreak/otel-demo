namespace codingfreaks.OtelDemo.Logic.OpenTelemetry
{
    using System.Diagnostics;
    using System.Diagnostics.Metrics;

    public static class Meters
    {
        #region methods

        public static void Init(string otelServiceName)
        {
            ActivitySource = new ActivitySource(otelServiceName);
            PeopleMeter = new Meter(otelServiceName, "1.0");
            PeopleCounter = PeopleMeter.CreateCounter<int>(
                "people.creations",
                description: "Is counted up when a new person is created.");
        }

        #endregion

        #region properties

        public static Meter? PeopleMeter { get; private set; }

        public static Counter<int>? PeopleCounter { get; private set; }

        public static ActivitySource? ActivitySource { get; private set; }

        #endregion
    }
}