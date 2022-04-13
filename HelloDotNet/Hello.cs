using System;
using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Server;

namespace HelloDotNet
{
    class Hello
    {
        // Set SdkKey to your LaunchDarkly SDK key.
        public const string SdkKey = "sdk-9fc18935-1821-4bb9-8439-995d926638cd";

        // Set FeatureFlagKey to the feature flag key you want to evaluate.
        public const string FeatureFlagKey = "x-lab-feature-flag";

        private static void ShowMessage(string s) {
            Console.WriteLine("*** " + s);
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            if (string.IsNullOrEmpty(SdkKey))
            {
                ShowMessage("Please edit Hello.cs to set SdkKey to your LaunchDarkly SDK key first");
                Environment.Exit(1);
            }

            var ldConfig = Configuration.Default(SdkKey);
            

            var client = new LdClient(ldConfig);

            if (client.Initialized)
            {
                ShowMessage("SDK successfully initialized!");
            }
            else
            {
                ShowMessage("SDK failed to initialize");
                Environment.Exit(1);
            }

            // Set up the user properties. This user should appear on your LaunchDarkly users dashboard
            // soon after you run the demo.
            var user = User.Builder("Sandy-user-key")
                .FirstName("Sandy")
                .LastName("Stevenson")
                .Country("Australia")
                .IPAddress("127.0.0.1")
                .Email("sstevenson@launchdarkly.com")
                .AsPrivateAttribute()
                .Build();

            var flagValue = client.BoolVariation(FeatureFlagKey, user, false);

            ShowMessage(string.Format("Feature flag '{0}' is {1} for this user",
                FeatureFlagKey, flagValue));

            // Here we ensure that the SDK shuts down cleanly and has a chance to deliver analytics
            // events to LaunchDarkly before the program exits. If analytics events are not delivered,
            // the user properties and flag usage statistics will not appear on your dashboard. In a
            // normal long-running application, the SDK would continue running and events would be
            // delivered automatically in the background.
            client.Dispose();
        }
    }
}
