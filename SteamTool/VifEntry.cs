using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteamTool
{
    // Valve Init File
    class VifEntry
    {
        public VifEntry next = null;
        public VifEntry first = null;
        public string name = string.Empty;
        public string value = string.Empty;

        public static VifEntry Scan(ref string content)
        {
            VifToken token = new VifToken();
            Stack<VifEntry> scopes = new Stack<VifEntry>();
            VifEntry root = null;
            VifEntry prev = null;
            VifEntry parent = null;

            while (true)
            {
                // Get the first token
                int match = token.Match(ref content, token.eos);
                //System.Diagnostics.Debug.WriteLine(string.Format("Name  {0,3}   {1,3}   {2,3}   {3}", token.Length(), token.bos, token.eos, token.ToString(ref content)));
                if (match == -1)
                    break; // end of file
                if (match < 0)
                    throw new Exception("File format error: Malformed file.");

                // Test for ending curly brace.
                if (token.token_type == AcfType.AcfCEnd)
                {
                    if (scopes.Count() == 0)
                        throw new Exception("File format error: Unblanaced curly brace.");
                    prev = scopes.Pop();
                    parent = (scopes.Count() > 0) ? scopes.Peek() : null;
                    continue;
                }

                // Get the name of the entry
                if (token.token_type != AcfType.AcfQstr)
                    throw new Exception("File format error: Expected a quoted string as a name.");
                VifEntry entry = new VifEntry();
                entry.name = token.ToString(ref content);

                // Insert the entry into the graph
                if (root == null)
                    root = entry;
                if (parent != null && parent.first == null)
                    parent.first = entry;
                else if (prev != null)
                    prev.next = entry;
                prev = entry;

                // Get the value of the entry, the second token.
                if (token.Match(ref content, token.eos) < 0)
                    throw new Exception(string.Format("File format error: Expected a value for name {0}", entry.name));
                //System.Diagnostics.Debug.WriteLine(string.Format("Value {0,3}   {1,3}   {2,3}   {3}", token.Length(), token.bos, token.eos, token.ToString(ref content)));
                if (token.token_type == AcfType.AcfCBegin)
                {
                    scopes.Push(entry);
                    parent = entry;
                    continue;
                }
                if (token.token_type == AcfType.AcfQstr)
                {
                    entry.value = token.ToString(ref content);
                    continue;
                }
                throw new Exception(string.Format("File format error: Malformed value for name {0}", entry.name));
            }
            return root;
        } // method

        public string Get(string name)
        {
            for (VifEntry entry = first; entry != null; entry = entry.next)
            {
                if (entry.name == name)
                    return entry.value;
            }
            return string.Empty;
        } // method

        public string Get(string name1, string name2)
        {
            if (name != name1)
                return string.Empty;
            return Get(name2);
        } // method

        public void ToStringBuilder(StringBuilder buf, int indent = 0)
        {
            string tindent = String.Empty;
            if (indent > 0)
                tindent = new String(' ', indent);
            VifEntry entry = this;
            while (entry != null)
            {
                if (entry.first != null)
                {
                    buf.WriteLine("{0}{1}", tindent, entry.name);
                    entry.first.ToStringBuilder(buf, indent + 3);
                }
                else
                    buf.WriteLine("{0}{1} {2}", tindent, entry.name, entry.value);
                entry = entry.next;
            }
        } // method

        override public String ToString()
        {
            StringBuilder buf = new StringBuilder();
            ToStringBuilder(buf);
            return buf.ToString();
        }
    } // class
} // namespace
