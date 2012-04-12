using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Dialer.Communication.Common;

namespace Dialer.Communication.Messages
{
    public class GetScreen : DialerMessage
    {
        private static readonly
            Regex _fieldRegex = new Regex(
                      "(?<Type>F)\\,\\s*(?<XPos>[0-9]+)\\,\\s*(?<YPos>[0-9]+)\\,\\s" +
                      "*(?<Length>[0-9]+)\\,\\s*\\\"(?<Name>\\w+)\\:(?<Cursor>[10])" +
                      "\\:(?<Locked>[10])\\:(?<Format>[CNDT$])\\:(?<PossibleValues>" +
                      ".*)\\:(?<Required>[01])\\:(?<Dots>[01])\\\"",
                      RegexOptions.CultureInvariant);
        private static readonly
            Regex _labelRegex = new Regex(
                      "(?<Type>L)\\,\\s*(?<XPos>[0-9]+)\\,\\s*(?<YPos>[0-9]+)\\,\\s" +
                      "*(?<Length>[0-9]+)\\,\\s*\\\"(?<Name>.*)\\\"",
                      RegexOptions.CultureInvariant);

        public Screen Screen { get; set; }

        internal override void Parse(List<string> segments)
        {
            Screen = new Screen { Name = segments.FirstOrDefault(), Fields = new List<Field>() };

            for (int i = 1; i < segments.Count; i++)
            {
                Field field = BuildField(segments[i]);
                if (field != null)
                    Screen.Fields.Add(field);
            }
        }

        private Field BuildField(string rawField)
        {
            Match fieldMatch = _fieldRegex.Match(rawField);
            if (fieldMatch.Success)
            {
                DataField field = new DataField
                {
                    Name = fieldMatch.Groups["Name"].Value,
                    X = Convert.ToInt32(fieldMatch.Groups["XPos"].Value),
                    Y = Convert.ToInt32(fieldMatch.Groups["YPos"].Value),
                    Width = Convert.ToInt32(fieldMatch.Groups["Length"].Value),
                    Type = fieldMatch.Groups["Format"].Value
                };
                if (fieldMatch.Groups["PossibleValues"].Success && !string.IsNullOrEmpty(fieldMatch.Groups["PossibleValues"].Value))
                    field.PossibleValues = new List<string>(fieldMatch.Groups["PossibleValues"].Value.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries));
                return field;
            }
            else
            {
                Match labelMatch = _labelRegex.Match(rawField);
                if (labelMatch.Success)
                {
                    LabelField field = new LabelField
                    {
                        Name = labelMatch.Groups["Name"].Value,
                        X = Convert.ToInt32(labelMatch.Groups["XPos"].Value),
                        Y = Convert.ToInt32(labelMatch.Groups["YPos"].Value),
                        Width = Convert.ToInt32(labelMatch.Groups["Length"].Value)
                    };
                    return field;
                }
            }
            return null;
        }
    }
}
