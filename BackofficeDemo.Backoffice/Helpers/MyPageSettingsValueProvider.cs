using System;
using System.Collections.Generic;
using BaseCms.Common.Validation.Attributes;
using BaseCms.MetadataValueProviders.Interfaces;
using BaseCms.Views.Detail.Attributes;

namespace BackofficeDemo.Backoffice.Helpers
{
    public class MyPageSettingsValueProvider : IPageSettingsValueProvider
    {
        private readonly Dictionary<string, Type> _dictionary;

        public MyPageSettingsValueProvider()
        {
            _dictionary = new Dictionary<string, Type>
                {
                    {"1", typeof (Template1Metadata)},
                    {"2", typeof (Template2Metadata)}
                };
        }

        public Type GetValue(string key)
        {
            if (key == null) return null;
            return _dictionary.ContainsKey(key) ? _dictionary[key] : null;
        }
    }

    public class Template2Metadata
    {
        [DetailMetadata(Display = "Нижний текст", Template = "Html", Order = 1)]
        public string BottomText { get; set; }

        [DetailMetadata(Display = "Мультилайн текст", Template = "MultilineText", Order = 3)]
        public string NewText { get; set; }

        [DetailMetadata(Display = "Сумма", Order = 2)]
        [Required]
        public int Summa { get; set; }
    }

    public class Template1Metadata
    {
        [DetailMetadata(Display = "Верхний текст", Template = "Html", Order = 2)]
        public string TopText { get; set; }

        [DetailMetadata(Display = "Сумма", Order = 1)]
        [Required]
        public int Sum { get; set; }
    }
}
