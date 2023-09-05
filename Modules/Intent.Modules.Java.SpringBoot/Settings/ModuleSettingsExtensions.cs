using System;
using Intent.Configuration;
using Intent.Engine;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Settings.ModuleSettingsExtensions", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Settings
{
    public static class ModuleSettingsExtensions
    {
        public static SpringBoot GetSpringBoot(this IApplicationSettingsProvider settings)
        {
            return new SpringBoot(settings.GetGroup("eced68d4-93cf-41c3-890b-ca665fa21953"));
        }
    }

    public class SpringBoot : IGroupSettings
    {
        private readonly IGroupSettings _groupSettings;

        public SpringBoot(IGroupSettings groupSettings)
        {
            _groupSettings = groupSettings;
        }

        public string Id => _groupSettings.Id;

        public string Title
        {
            get => _groupSettings.Title;
            set => _groupSettings.Title = value;
        }

        public ISetting GetSetting(string settingId)
        {
            return _groupSettings.GetSetting(settingId);
        }
        public TargetVersionOptions TargetVersion() => new TargetVersionOptions(_groupSettings.GetSetting("2c7289d0-0261-4040-95ad-e7aba770753c")?.Value);

        public class TargetVersionOptions
        {
            public readonly string Value;

            public TargetVersionOptions(string value)
            {
                Value = value;
            }

            public TargetVersionOptionsEnum AsEnum()
            {
                return Value switch
                {
                    "V2_7_5" => TargetVersionOptionsEnum.V2_7_5,
                    "V3_1_3" => TargetVersionOptionsEnum.V3_1_3,
                    _ => throw new ArgumentOutOfRangeException(nameof(Value), $"{Value} is out of range")
                };
            }

            public bool IsV2_7_5()
            {
                return Value == "V2_7_5";
            }

            public bool IsV3_1_3()
            {
                return Value == "V3_1_3";
            }

            [IntentIgnore]
            public override string ToString()
            {
                return Value.RemovePrefix("V").Replace("_", ".");
            }
        }

        public enum TargetVersionOptionsEnum
        {
            V2_7_5,
            V3_1_3,
        }
    }
}