using System.Text;

namespace TravelPortal.Web.Utils;

public static class PinyinHelper
{
    /// <summary>
    /// 获取汉字首字母（大写）
    /// </summary>
    public static string GetFirstLetter(string str)
    {
        if (string.IsNullOrEmpty(str)) return "#";
        
        char firstChar = str[0];
        if (firstChar >= 'a' && firstChar <= 'z' || firstChar >= 'A' && firstChar <= 'Z')
        {
            return firstChar.ToString().ToUpper();
        }

        // 简单汉字首字母快速计算逻辑（基于编码范围）
        // 如果需要更高精度，建议引入第三方库如 PinYinConverter
        byte[] array = Encoding.Default.GetBytes(new char[] { firstChar });
        if (array.Length < 2) return "#";

        int i = array[0] * 256 + array[1];
        if (i < 0xB0A1) return "#";
        if (i < 0xB0C5) return "A";
        if (i < 0xB2C1) return "B";
        if (i < 0xB4EE) return "C";
        if (i < 0xB6EA) return "D";
        if (i < 0xB7A2) return "E";
        if (i < 0xB8C1) return "F";
        if (i < 0xB9FE) return "G";
        if (i < 0xBBF7) return "H";
        if (i < 0xBFA6) return "J";
        if (i < 0xC0AC) return "K";
        if (i < 0xC2E8) return "L";
        if (i < 0xC4C3) return "M";
        if (i < 0xC5B6) return "N";
        if (i < 0xC5BE) return "O";
        if (i < 0xC6DA) return "P";
        if (i < 0xC8BB) return "Q";
        if (i < 0xC8F6) return "R";
        if (i < 0xCBFA) return "S";
        if (i < 0xCDDA) return "T";
        if (i < 0xCEF4) return "W";
        if (i < 0xD1B9) return "X";
        if (i < 0xD4D1) return "Y";
        if (i < 0xD7FA) return "Z";
        
        return "#";
    }
}
