using System;

namespace SteamTool
{
    public enum AcfType { AcfNone, AcfQstr, AcfCBegin, AcfCEnd };

    // Valve Init File
    public class VifToken
    {
        public AcfType token_type = AcfType.AcfNone;
        public int bos = 0;
        public int eos = 0;

        AcfType GetAcfType(char chr)
        {
            if (chr == '{')
                return AcfType.AcfCBegin;
            if (chr == '}')
                return AcfType.AcfCEnd;
            if (chr == '\"')
                return AcfType.AcfQstr;
            return AcfType.AcfNone;
        } // method

        // Recognices a quoted string or a curly bracket at the specified offset.
        // On success it sets offset_begin and offset_end to point to the matched text.
        // On success it returns offset_end, otherwise it returns a negative number, -1 on end of file and -2 on error.
        public int Match(ref string txt, int offset)
        {
            const char quote = '\"';
            int len = txt.Length;

            // Skip leading whitespace
            while(offset < len && char.IsWhiteSpace(txt[offset]))
                ++offset;

            // End of file ?
            if (offset >= len)
                return -1;

            // Determine the type of the token.
            bos = offset;
            token_type = GetAcfType(txt[bos]);

            // Is it curly braces ?
            if (token_type == AcfType.AcfCBegin || token_type == AcfType.AcfCEnd)
            {
                eos = offset + 1;
                return eos;
            }
            // Is it a quoted string ?
            if (token_type == AcfType.AcfQstr)
            {
                if (++offset >= len)
                    return -2;
                offset = txt.IndexOf(quote, offset);
                if (offset < 0)
                    return -2;
                eos = offset + 1;
                return eos;
            }
            // Fail
            return -3;
        } // method

        public int Length()
        {
            if (eos < 1)
                return 0;
            return (eos - bos);
        } // method

        public string ToString(ref string content)
        {
            if (token_type == AcfType.AcfNone)
                return string.Empty;
            int len = Length();
            if (len < 1)
                return string.Empty;
            if (token_type == AcfType.AcfQstr && len < 3)
                return string.Empty;
            if (token_type == AcfType.AcfQstr)
                return content.Substring(bos + 1, len - 2);
            return content.Substring(bos, len);
        } // method
    } // class
} // namespace
