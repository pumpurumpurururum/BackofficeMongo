using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using BaseCms.Common.Typograph.Interfaces;

namespace BaseCms.Common.Typograph
{
    public class Typograph : ITypograph
    {
        public string ProcessText(string text, Type typographHelperType = null)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            if ((typographHelperType != null) && typeof(ITypographHelper).IsAssignableFrom(typographHelperType))
            {
                var helper = InstanceCreator.CreateInstance<ITypographHelper>(typographHelperType);
                if (helper != null)
                {
                    text = helper.HandleText(text);
                }
            }

            var protectedFromTypographRegExps = new[]
                {
                    @"href=\""[\w\d\sа-яА-Я\\\-.:/=%+&;?,]+\""", @"src=\""[\w\d\sа-яА-Я\\\-.:/=%+&;?,]+\""",
                    @"class=\""[\w-\s]+\""", @"style=\""[\w-\s:;]+\""",
                    @"width=\""[\w\d]+\""", @"height=\""[\w\d]+\""",
                    @"frameborder=\""[\d]+\""", @"scrolling=\""[\w\d]+\""",
                    @"marginwidth=\""[\w\d]+\""", @"marginheight=\""[\w\d]+\"""
                };

            var protectedFromTypographValues = new Dictionary<string, string>();
            var protectedIndex = 1;
            foreach (var regExp in protectedFromTypographRegExps)
            {
                foreach (var val in Regex.Matches(text, regExp))
                {
                    var key = $"<prVal{protectedIndex}>";
                    var value = val.ToString();
                    protectedFromTypographValues.Add(key, value);
                    text = text.Replace(value, key);
                    protectedIndex++;
                }
            }

            text += "$";
            const string clearLastVoidParagraphsPattern = @"((<p>(&nbsp;|\s)</p>\n*\r*)+)\$";
            text = Regex.Replace(text, clearLastVoidParagraphsPattern, "$");
            text = text.Substring(0, text.Length - 1);

            var path = HostingEnvironment.MapPath("~/BaseCms/Common/Typograph");

            // загрузим регулярные выражения
            var preexprs = new StreamReader(Path.Combine(path, "preexprs.txt"), Encoding.Default);
            string line;
            int divider;
            while ((line = preexprs.ReadLine()) != null)
            {
                if ((line.TrimStart().Length > 0) && (line.TrimStart()[0] == '#'))
                    continue;

                divider = line.IndexOf("\x0009", StringComparison.Ordinal);
                if (divider <= 0) continue;

                var pattern = line.Substring(0, divider);

                if (!Regex.IsMatch(text, pattern)) continue;

                var replaceValue = line.Substring(divider + 1);

                if (replaceValue.EndsWith("()"))
                {
                    try
                    {
                        var mi = typeof(Typograph).GetMethod(replaceValue.TrimEnd('(', ')'),
                                                              BindingFlags.NonPublic | BindingFlags.Instance);

                        foreach (var m in from object match in Regex.Matches(text, pattern) select match as Match)
                        {
                            if (m != null)
                            {
                                var groupCollection = m.Groups;
                                replaceValue = mi.Invoke(this, new object[] { groupCollection }).ToString();
                                text = text.Replace(m.Value, replaceValue);
                            }
                            
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
                else text = Regex.Replace(text, pattern, replaceValue);
            }
            preexprs.Close();

            // теперь обработаем обычные замены
            var exprs = new StreamReader(Path.Combine(path, "exprs.txt"), Encoding.Default);
            while ((line = exprs.ReadLine()) != null)
            {
                divider = line.IndexOf("\x0009", StringComparison.Ordinal);
                if (divider > 0)
                {
                    text = text.Replace(line.Substring(0, divider), line.Substring(divider + 1));
                }
            }
            exprs.Close();

            text = protectedFromTypographValues.Aggregate(text, (current, prVal) => current.Replace(prVal.Key, prVal.Value));

            return text;
        }

        #region Сокращения

        private string m_sm_m2(GroupCollection m)
        {
            return
                $"{m[1]}{m[2]}<nbsp/>{m[4]}{((m[5].Value == "3" || m[5].Value == "2") ? "<sup" + m[5] + "/>" : String.Empty)}{m[6]}";
        }

        private string ps_pps(GroupCollection m)
        {
            return
                $"{m[1]}{noWrapSpan(m[2].Value.Trim() + (string.IsNullOrEmpty(m[3].Value.Trim()) ? string.Empty : m[3].Value) + "<nbsp/>" + m[4])}{m[5]}";
        }

        private string itd(GroupCollection m)
        {
            return m[1].Value + noWrapSpan("и т. д.");
        }

        private string itp(GroupCollection m)
        {
            return m[1].Value + noWrapSpan("и т. п.");
        }

        private string vtch(GroupCollection m)
        {
            return m[1].Value + noWrapSpan("в т. ч.");
        }

        private string te(GroupCollection m)
        {
            return m[1].Value + noWrapSpan(m[2].Value + ". е.");
        }

        private string ue(GroupCollection m)
        {
            return m[1].Value + (checkIsNullOrEmpty(m[4], "<nbsp/>" + m[4].Value + (m[4].Value == "тыс" ? "." : "")) + "<nbsp/>" + (Regex.IsMatch(m[7].Value, "у[\\.]? ?е[\\.]?") ? "у.е." : m[7].Value));
        }

        private string gost1(GroupCollection m)
        {
            return m[1].Value + noWrapSpan("ГОСТ " + m[3].Value + checkIsNullOrEmpty(m[6], "<ndash/>" + m[6].Value) + checkIsNullOrEmpty(m[7], "<mdash/>" + m[7].Value));
        }

        private string gost2(GroupCollection m)
        {
            return m[1].Value + "ГОСТ " + m[3].Value + "<ndash/>" + m[5].Value;
        }

        #endregion

        #region Дефисы и тире

        private string defis(GroupCollection m)
        {
            return checkNbsp(m[1]) + m[2].Value + "-" + m[4].Value + checkNbsp(m[5]);
        }

        #endregion

        #region Даты и дни

        private string period(GroupCollection m)
        {
            return m[1].Value + m[2].Value +
                   (int.Parse(m[3].Value) >= int.Parse(m[5].Value)
                        ? String.Format("{0}{1}{2}", m[3], m[4], m[5])
                        : String.Format("{0}<mdash/>{1}", m[3], m[5])) + checkIsNullOrEmpty(m[6], "<nbsp/>гг.");
        }

        private string daysInterval(GroupCollection m)
        {
            return m[1].Value + noWrapSpan(m[2].Value + "<mdash/>" + m[4].Value + "<nbsp/>" + m[6].Value) + m[7].Value;
        }

        private string year1(GroupCollection m)
        {
            return m[1].Value + noWrapSpan(m[2].Value + " г.") + (m[5].Value == "." ? String.Empty : " ");
        }

        private string year2(GroupCollection m)
        {
            return m[1].Value + noWrapSpan(m[2].Value) + m[4].Value;
        }

        #endregion

        #region Прочее

        private string supSmall(GroupCollection m)
        {
            return String.Format("<sup><small>{0}</small></sup>{1}", m[3], m[4]);
        }

        private string centuries(GroupCollection m)
        {
            return m[1].Value + noWrapSpan(m[2].Value + "<mdash/>" + m[4] + " вв.");
        }

        private string timeDiapason(GroupCollection m)
        {
            return m[1].Value + noWrapSpan(m[2].Value + "<mdash/>" + m[4]) + m[5].Value;
        }

        #endregion

        #region Неразрывные конструкции

        private string prepConj(GroupCollection m)
        {
            return String.Format("{0}{1}<nbsp/>{2}{3}", m[1], m[3].Value.Trim(),
                                 String.IsNullOrEmpty(m[4].Value) ? String.Empty : m[4].Value.Trim() + "<nbsp/>", m[5]);
        }

        private string phone(GroupCollection m)
        {
            return m[1].Value +
                   ((m[1].Value == ">" || m[11].Value == "<")
                        ? m[2].Value + " " + m[4].Value + " " + m[6].Value + "-" + m[8].Value + "-" + m[10].Value
                        : noWrapSpan(m[2].Value + " " + m[4].Value + " " + m[6].Value + "-" + m[8].Value + "-" +
                                     m[10].Value)) +
                   m[11].Value;
        }

        private string initials1(GroupCollection m)
        {
            return m[1].Value +
                   noWrapSpan(m[2].Value + ". " + m[4].Value + ". " + m[8].Value) +
                   m[9].Value;
        }

        private readonly string[] _nonChangableInitials = new[] { "РФ", "ЖК" };

        private string initials2(GroupCollection m)
        {
            var initials = m[4].Value + m[6].Value;
            if (_nonChangableInitials.Contains(initials)) return m[0].Value;

            return m[1].Value +
                   noWrapSpan(m[2].Value + " " + m[4].Value + ". " + m[6].Value + ".") +
                   m[7].Value;
        }

        private string particle(GroupCollection m)
        {
            return "<nbsp/>" + m[2].Value + checkNbsp(m[3]);
        }

        private string minus(GroupCollection m)
        {
            return String.Format("{0}<minus/>{1}{2}{3}{4}{5}", m[1], m[3], m[4], m[5],
                                 (m[6].Value == "+" ? m[6].Value : "</minus>"), m[7]);
        }

        private string subIndex(GroupCollection m)
        {
            return m[1].Value + String.Format("<sub><small>{0}</small></sub>{1}", m[2], m[3]);
        }

        private string supIndex(GroupCollection m)
        {
            return m[1].Value + String.Format("<sup><small>{0}</small></sup>{1}", m[2], m[3]);
        }

        private string triads(GroupCollection m)
        {
            return m[3].Value == "-" ? m[0].Value : m[1].Value.Replace(" ", "<thinsp/>") + m[3].Value;
        }

        #endregion

        #region Оптическое выравнивание

        private string opticAlignQuote1(GroupCollection m)
        {
            return m[1].Value + String.Format("<span style=\"margin-right:0.44em;\">{0}</span>", m[2]) +
                   String.Format("<span style=\"margin-left:-0.44em;\">{0}</span>", m[3]);
        }

        private string opticAlignQuote2(GroupCollection m)
        {
            return m[1].Value + String.Format("<span style=\"margin-left:-0.44em;\">{0}</span>", m[2]);
        }

        private string opticAlignBracket1(GroupCollection m)
        {
            return String.Format("<span style=\"margin-right:0.3em;\">{0}</span>", m[1]) +
                   "<span style=\"margin-left:-0.3em;\">(</span>";
        }

        private string opticAlignBracket2(GroupCollection m)
        {
            return m[1].Value + "<span style=\"margin-left:-0.3em;\">(</span>";
        }

        private string opticAlignBracket3(GroupCollection m)
        {
            return m[1].Value + "<span style=\"margin-right:-0.2em;\">,</span>" + "<span style=\"margin-left:0.2em;\"> </span>";
        }

        #endregion

        #region Кавычки

        private string openQuote(GroupCollection m)
        {
            return String.Format("{0}<quot1>{1}", m[1], m[3]);
        }

        private string closeQuote(GroupCollection m)
        {
            return m[1].Value + String.Concat(Enumerable.Repeat("</quot1>", m[2].Value.Count(c => c == '"'))) + m[4].Value;
        }

        private string closeQuoteSpec1(GroupCollection m)
        {
            return m[1].Value + String.Concat(Enumerable.Repeat("</quot1>", m[2].Value.Count(c => c == '"') + substringCount(m[2].Value, "&laquo;"))) + m[4].Value + m[5].Value;
        }

        private string closeQuoteSpec2(GroupCollection m)
        {
            return m[1].Value + m[2].Value + String.Concat(Enumerable.Repeat("</quot1>", m[3].Value.Count(c => c == '"') + substringCount(m[3].Value, "&laquo;"))) + m[5].Value + m[6].Value;
        }

        private string openQuoteSpec(GroupCollection m)
        {
            return String.Format("{0}<quot1>{1}", m[1], m[4]);
        }

        #endregion

        #region Расстановка и удаление пробелов

        private string spaceAfterDot(GroupCollection m)
        {
            return m[1].Value + m[2].Value + "." + (checkIsDomain(m[3].Value) ? "" : " ") + m[3].Value;
        }

        #endregion

        #region Специальные символы

        private string reg(GroupCollection m)
        {
            return String.Format("{0}<regd/>{1}", m[1], m[2]);
        }

        private string fahrenheit(GroupCollection m)
        {
            return noWrapSpan(m[1].Value + " <deg/>F") + m[2].Value;
        }

        #endregion

        private string noWrapSpan(string innerHtml)
        {
            return $"<span style=\"word-spacing:nowrap;\">{innerHtml}</span>";
        }

        private string checkNbsp(Group beingCheckedGroup)
        {
            return (beingCheckedGroup.Value == "&nbsp;" ? " " : beingCheckedGroup.Value);
        }

        private string checkIsNullOrEmpty(Group beingCheckedGroup, string valueIfNotNullOrEmpty, string valueIfNullOrEmpty = "")
        {
            return String.IsNullOrEmpty(beingCheckedGroup.Value) ? valueIfNullOrEmpty : valueIfNotNullOrEmpty;
        }

        private int substringCount(string src, string target)
        {
            return src.Select((c, i) => src.Substring(i)).Count(sub => sub.StartsWith(target));
        }

        private readonly string[] _domains = { "ru", "ру", "com", "ком", "org", "орг", "уа", "ua" };

        private bool checkIsDomain(string src)
        {
            if (String.IsNullOrEmpty(src)) return false;
            return _domains.Contains(src.ToLower());
        }

        private string divideNumberIntoTriads(string text)
        {
            const string pattern = @"[\s?!>()-+*=#№$]\d{4,}";
            foreach (var match in Regex.Matches(text, pattern))
            {
                var m = match as Match;

                if (m != null)
                {
                    var number = m.Value;

                    var sb = new StringBuilder(number.Length + number.Length / 3);
                    for (var i = number.Length - 1; i >= 0; i--)
                    {
                        var symbol = number[i];
                        sb.Append(symbol);
                        if (!char.IsDigit(symbol)) break;
                        if ((number.Length - i) % 3 == 0) sb.Append(' ');
                    }

                    text = text.Replace(number, string.Concat(sb.ToString().Reverse()));
                }
            }

            return text;
        }
    }
}
