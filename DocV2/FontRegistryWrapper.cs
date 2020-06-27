using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocV2
{
    class FontRegistryWrapper
    {
        public static Dictionary<string,string> fontKV;

        public static void ReadRegistry()
        {
            fontKV = new Dictionary<string, string>();
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\Fonts"))
            {
                foreach (string name in key.GetValueNames())
                {
                    if (IsSupportKorean(key.GetValue(name).ToString()))
                    {
                        string tmpName = name.Split(new string[]{" &"," (TrueType)"},StringSplitOptions.None)[0];
                        fontKV.Add(tmpName, key.GetValue(name).ToString());
                    }
                }
            }
        }
        public static bool IsSupportKorean(String fontName)
        {
            try
            {
                var families = System.Windows.Media.Fonts.GetFontFamilies(@"C:\Windows\Fonts\" + fontName);
                foreach (System.Windows.Media.FontFamily family in families)
                {
                    var typefaces = family.GetTypefaces();
                    foreach (System.Windows.Media.Typeface typeface in typefaces)
                    {
                        System.Windows.Media.GlyphTypeface glyph;
                        typeface.TryGetGlyphTypeface(out glyph);
                        IDictionary<int, ushort> characterMap = glyph.CharacterToGlyphMap;
                        if (characterMap.ContainsKey(Char.Parse("가")))
                            return true;
                        else
                            return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        public static Font GetITextSharpFont(string name, string size)
        {
            BaseFont baseFont;
            if (fontKV.ContainsKey(name))
            {
                name = fontKV[name];
            }
            else
            {
                name = fontKV.ToList()[0].Value;
            }
            try { baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\" + name, BaseFont.IDENTITY_H, BaseFont.EMBEDDED); }
            catch { baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\" + name + ",1", BaseFont.IDENTITY_H, BaseFont.EMBEDDED); }

            return new Font(baseFont, float.Parse(size) / 9 * 11);
        }
    }
}
