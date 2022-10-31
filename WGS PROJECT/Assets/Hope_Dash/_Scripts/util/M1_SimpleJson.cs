/* * * * *
 * A simple JSON Parser / builder
 * ------------------------------
 * 
 * It mainly has been written as a simple JSON parser. It can build a JSON string
 * from the node-tree, or generate a node tree from any valid JSON string.
 * 
 * Written by Bunny83 
 * 2012-06-09
 * 
 * Changelog now external. See Changelog.txt
 * 
 * The MIT License (MIT)
 * 
 * Copyright (c) 2012-2019 Markus Göbel (Bunny83)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 * * * * */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace M1_SimpleJSON
{
    public enum JSONNodeType
    {
        Array = 1,
        Object = 2,
        String = 3,
        Number = 4,
        NullValue = 5,
        Boolean = 6,
        None = 7,
        Custom = 0xFF,
    }
    public enum JSONTextMode
    {
        Compact,
        Indent
    }

    public abstract partial class M1_JSONNode
    {
        #region Enumerators
        public struct Enumerator
        {
            private enum Type { None, Array, Object }
            private Type type;
            private Dictionary<string, M1_JSONNode>.Enumerator m_Object;
            private List<M1_JSONNode>.Enumerator m_Array;
            public bool IsValid { get { return type != Type.None; } }
            public Enumerator(List<M1_JSONNode>.Enumerator aArrayEnum)
            {
                type = Type.Array;
                m_Object = default(Dictionary<string, M1_JSONNode>.Enumerator);
                m_Array = aArrayEnum;
            }
            public Enumerator(Dictionary<string, M1_JSONNode>.Enumerator aDictEnum)
            {
                type = Type.Object;
                m_Object = aDictEnum;
                m_Array = default(List<M1_JSONNode>.Enumerator);
            }
            public KeyValuePair<string, M1_JSONNode> Current
            {
                get
                {
                    if (type == Type.Array)
                        return new KeyValuePair<string, M1_JSONNode>(string.Empty, m_Array.Current);
                    else if (type == Type.Object)
                        return m_Object.Current;
                    return new KeyValuePair<string, M1_JSONNode>(string.Empty, null);
                }
            }
            public bool MoveNext()
            {
                if (type == Type.Array)
                    return m_Array.MoveNext();
                else if (type == Type.Object)
                    return m_Object.MoveNext();
                return false;
            }
        }
        public struct ValueEnumerator
        {
            private Enumerator m_Enumerator;
            public ValueEnumerator(List<M1_JSONNode>.Enumerator aArrayEnum) : this(new Enumerator(aArrayEnum)) { }
            public ValueEnumerator(Dictionary<string, M1_JSONNode>.Enumerator aDictEnum) : this(new Enumerator(aDictEnum)) { }
            public ValueEnumerator(Enumerator aEnumerator) { m_Enumerator = aEnumerator; }
            public M1_JSONNode Current { get { return m_Enumerator.Current.Value; } }
            public bool MoveNext() { return m_Enumerator.MoveNext(); }
            public ValueEnumerator GetEnumerator() { return this; }
        }
        public struct KeyEnumerator
        {
            private Enumerator m_Enumerator;
            public KeyEnumerator(List<M1_JSONNode>.Enumerator aArrayEnum) : this(new Enumerator(aArrayEnum)) { }
            public KeyEnumerator(Dictionary<string, M1_JSONNode>.Enumerator aDictEnum) : this(new Enumerator(aDictEnum)) { }
            public KeyEnumerator(Enumerator aEnumerator) { m_Enumerator = aEnumerator; }
            public string Current { get { return m_Enumerator.Current.Key; } }
            public bool MoveNext() { return m_Enumerator.MoveNext(); }
            public KeyEnumerator GetEnumerator() { return this; }
        }

        public class LinqEnumerator : IEnumerator<KeyValuePair<string, M1_JSONNode>>, IEnumerable<KeyValuePair<string, M1_JSONNode>>
        {
            private M1_JSONNode m_Node;
            private Enumerator m_Enumerator;
            internal LinqEnumerator(M1_JSONNode aNode)
            {
                m_Node = aNode;
                if (m_Node != null)
                    m_Enumerator = m_Node.GetEnumerator();
            }
            public KeyValuePair<string, M1_JSONNode> Current { get { return m_Enumerator.Current; } }
            object IEnumerator.Current { get { return m_Enumerator.Current; } }
            public bool MoveNext() { return m_Enumerator.MoveNext(); }

            public void Dispose()
            {
                m_Node = null;
                m_Enumerator = new Enumerator();
            }

            public IEnumerator<KeyValuePair<string, M1_JSONNode>> GetEnumerator()
            {
                return new LinqEnumerator(m_Node);
            }

            public void Reset()
            {
                if (m_Node != null)
                    m_Enumerator = m_Node.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new LinqEnumerator(m_Node);
            }
        }

        #endregion Enumerators

        #region common interface

        public static bool forceASCII = false; // Use Unicode by default
        public static bool longAsString = false; // lazy creator creates a JSONString instead of JSONNumber
        public static bool allowLineComments = true; // allow "//"-style comments at the end of a line

        public abstract JSONNodeType Tag { get; }

        public virtual M1_JSONNode this[int aIndex] { get { return null; } set { } }

        public virtual M1_JSONNode this[string aKey] { get { return null; } set { } }

        public virtual string Value { get { return ""; } set { } }

        public virtual int Count { get { return 0; } }

        public virtual bool IsNumber { get { return false; } }
        public virtual bool IsString { get { return false; } }
        public virtual bool IsBoolean { get { return false; } }
        public virtual bool IsNull { get { return false; } }
        public virtual bool IsArray { get { return false; } }
        public virtual bool IsObject { get { return false; } }

        public virtual bool Inline { get { return false; } set { } }

        public virtual void Add(string aKey, M1_JSONNode aItem)
        {
        }
        public virtual void Add(M1_JSONNode aItem)
        {
            Add("", aItem);
        }

        public virtual M1_JSONNode Remove(string aKey)
        {
            return null;
        }

        public virtual M1_JSONNode Remove(int aIndex)
        {
            return null;
        }

        public virtual M1_JSONNode Remove(M1_JSONNode aNode)
        {
            return aNode;
        }
        public virtual void Clear() { }

        public virtual M1_JSONNode Clone()
        {
            return null;
        }

        public virtual IEnumerable<M1_JSONNode> Children
        {
            get
            {
                yield break;
            }
        }

        public IEnumerable<M1_JSONNode> DeepChildren
        {
            get
            {
                foreach (var C in Children)
                    foreach (var D in C.DeepChildren)
                        yield return D;
            }
        }

        public virtual bool HasKey(string aKey)
        {
            return false;
        }

        public virtual M1_JSONNode GetValueOrDefault(string aKey, M1_JSONNode aDefault)
        {
            return aDefault;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            WriteToStringBuilder(sb, 0, 0, JSONTextMode.Compact);
            return sb.ToString();
        }

        public virtual string ToString(int aIndent)
        {
            StringBuilder sb = new StringBuilder();
            WriteToStringBuilder(sb, 0, aIndent, JSONTextMode.Indent);
            return sb.ToString();
        }
        internal abstract void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode);

        public abstract Enumerator GetEnumerator();
        public IEnumerable<KeyValuePair<string, M1_JSONNode>> Linq { get { return new LinqEnumerator(this); } }
        public KeyEnumerator Keys { get { return new KeyEnumerator(GetEnumerator()); } }
        public ValueEnumerator Values { get { return new ValueEnumerator(GetEnumerator()); } }

        #endregion common interface

        #region typecasting properties


        public virtual double AsDouble
        {
            get
            {
                double v = 0.0;
                if (double.TryParse(Value, NumberStyles.Float, CultureInfo.InvariantCulture, out v))
                    return v;
                return 0.0;
            }
            set
            {
                Value = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        public virtual int AsInt
        {
            get { return (int)AsDouble; }
            set { AsDouble = value; }
        }

        public virtual float AsFloat
        {
            get { return (float)AsDouble; }
            set { AsDouble = value; }
        }

        public virtual bool AsBool
        {
            get
            {
                bool v = false;
                if (bool.TryParse(Value, out v))
                    return v;
                return !string.IsNullOrEmpty(Value);
            }
            set
            {
                Value = (value) ? "true" : "false";
            }
        }

        public virtual long AsLong
        {
            get
            {
                long val = 0;
                if (long.TryParse(Value, out val))
                    return val;
                return 0L;
            }
            set
            {
                Value = value.ToString();
            }
        }

        public virtual ulong AsULong
        {
            get
            {
                ulong val = 0;
                if (ulong.TryParse(Value, out val))
                    return val;
                return 0;
            }
            set
            {
                Value = value.ToString();
            }
        }

        public virtual M1JsonArray AsArray
        {
            get
            {
                return this as M1JsonArray;
            }
        }

        public virtual M1JsonObject AsObject
        {
            get
            {
                return this as M1JsonObject;
            }
        }


        #endregion typecasting properties

        #region operators

        public static implicit operator M1_JSONNode(string s)
        {
            return (s == null) ? (M1_JSONNode)M1JsonNull.CreateOrGet() : new M1JsonString(s);
        }
        public static implicit operator string(M1_JSONNode d)
        {
            return (d == null) ? null : d.Value;
        }

        public static implicit operator M1_JSONNode(double n)
        {
            return new M1JsonNumber(n);
        }
        public static implicit operator double(M1_JSONNode d)
        {
            return (d == null) ? 0 : d.AsDouble;
        }

        public static implicit operator M1_JSONNode(float n)
        {
            return new M1JsonNumber(n);
        }
        public static implicit operator float(M1_JSONNode d)
        {
            return (d == null) ? 0 : d.AsFloat;
        }

        public static implicit operator M1_JSONNode(int n)
        {
            return new M1JsonNumber(n);
        }
        public static implicit operator int(M1_JSONNode d)
        {
            return (d == null) ? 0 : d.AsInt;
        }

        public static implicit operator M1_JSONNode(long n)
        {
            if (longAsString)
                return new M1JsonString(n.ToString());
            return new M1JsonNumber(n);
        }
        public static implicit operator long(M1_JSONNode d)
        {
            return (d == null) ? 0L : d.AsLong;
        }

        public static implicit operator M1_JSONNode(ulong n)
        {
            if (longAsString)
                return new M1JsonString(n.ToString());
            return new M1JsonNumber(n);
        }
        public static implicit operator ulong(M1_JSONNode d)
        {
            return (d == null) ? 0 : d.AsULong;
        }

        public static implicit operator M1_JSONNode(bool b)
        {
            return new M1JsonBool(b);
        }
        public static implicit operator bool(M1_JSONNode d)
        {
            return (d == null) ? false : d.AsBool;
        }

        public static implicit operator M1_JSONNode(KeyValuePair<string, M1_JSONNode> aKeyValue)
        {
            return aKeyValue.Value;
        }

        public static bool operator ==(M1_JSONNode a, object b)
        {
            if (ReferenceEquals(a, b))
                return true;
            bool aIsNull = a is M1JsonNull || ReferenceEquals(a, null) || a is M1JsonLazyCreator;
            bool bIsNull = b is M1JsonNull || ReferenceEquals(b, null) || b is M1JsonLazyCreator;
            if (aIsNull && bIsNull)
                return true;
            return !aIsNull && a.Equals(b);
        }

        public static bool operator !=(M1_JSONNode a, object b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion operators

        [ThreadStatic]
        private static StringBuilder m_EscapeBuilder;
        internal static StringBuilder EscapeBuilder
        {
            get
            {
                if (m_EscapeBuilder == null)
                    m_EscapeBuilder = new StringBuilder();
                return m_EscapeBuilder;
            }
        }
        internal static string Escape(string aText)
        {
            var sb = EscapeBuilder;
            sb.Length = 0;
            if (sb.Capacity < aText.Length + aText.Length / 10)
                sb.Capacity = aText.Length + aText.Length / 10;
            foreach (char c in aText)
            {
                switch (c)
                {
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    default:
                        if (c < ' ' || (forceASCII && c > 127))
                        {
                            ushort val = c;
                            sb.Append("\\u").Append(val.ToString("X4"));
                        }
                        else
                            sb.Append(c);
                        break;
                }
            }
            string result = sb.ToString();
            sb.Length = 0;
            return result;
        }

        private static M1_JSONNode ParseElement(string token, bool quoted)
        {
            if (quoted)
                return token;
            if (token.Length <= 5)
            {
                string tmp = token.ToLower();
                if (tmp == "false" || tmp == "true")
                    return tmp == "true";
                if (tmp == "null")
                    return M1JsonNull.CreateOrGet();
            }
            double val;
            if (double.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out val))
                return val;
            else
                return token;
        }

        public static M1_JSONNode Parse(string aJSON)
        {
            Stack<M1_JSONNode> stack = new Stack<M1_JSONNode>();
            M1_JSONNode ctx = null;
            int i = 0;
            StringBuilder Token = new StringBuilder();
            string TokenName = "";
            bool QuoteMode = false;
            bool TokenIsQuoted = false;
            bool HasNewlineChar = false;
            while (i < aJSON.Length)
            {
                switch (aJSON[i])
                {
                    case '{':
                        if (QuoteMode)
                        {
                            Token.Append(aJSON[i]);
                            break;
                        }
                        stack.Push(new M1JsonObject());
                        if (ctx != null)
                        {
                            ctx.Add(TokenName, stack.Peek());
                        }
                        TokenName = "";
                        Token.Length = 0;
                        ctx = stack.Peek();
                        HasNewlineChar = false;
                        break;

                    case '[':
                        if (QuoteMode)
                        {
                            Token.Append(aJSON[i]);
                            break;
                        }

                        stack.Push(new M1JsonArray());
                        if (ctx != null)
                        {
                            ctx.Add(TokenName, stack.Peek());
                        }
                        TokenName = "";
                        Token.Length = 0;
                        ctx = stack.Peek();
                        HasNewlineChar = false;
                        break;

                    case '}':
                    case ']':
                        if (QuoteMode)
                        {

                            Token.Append(aJSON[i]);
                            break;
                        }
                        if (stack.Count == 0)
                            throw new Exception("JSON Parse: Too many closing brackets");

                        stack.Pop();
                        if (Token.Length > 0 || TokenIsQuoted)
                            ctx.Add(TokenName, ParseElement(Token.ToString(), TokenIsQuoted));
                        if (ctx != null)
                            ctx.Inline = !HasNewlineChar;
                        TokenIsQuoted = false;
                        TokenName = "";
                        Token.Length = 0;
                        if (stack.Count > 0)
                            ctx = stack.Peek();
                        break;

                    case ':':
                        if (QuoteMode)
                        {
                            Token.Append(aJSON[i]);
                            break;
                        }
                        TokenName = Token.ToString();
                        Token.Length = 0;
                        TokenIsQuoted = false;
                        break;

                    case '"':
                        QuoteMode ^= true;
                        TokenIsQuoted |= QuoteMode;
                        break;

                    case ',':
                        if (QuoteMode)
                        {
                            Token.Append(aJSON[i]);
                            break;
                        }
                        if (Token.Length > 0 || TokenIsQuoted)
                            ctx.Add(TokenName, ParseElement(Token.ToString(), TokenIsQuoted));
                        TokenIsQuoted = false;
                        TokenName = "";
                        Token.Length = 0;
                        TokenIsQuoted = false;
                        break;

                    case '\r':
                    case '\n':
                        HasNewlineChar = true;
                        break;

                    case ' ':
                    case '\t':
                        if (QuoteMode)
                            Token.Append(aJSON[i]);
                        break;

                    case '\\':
                        ++i;
                        if (QuoteMode)
                        {
                            char C = aJSON[i];
                            switch (C)
                            {
                                case 't':
                                    Token.Append('\t');
                                    break;
                                case 'r':
                                    Token.Append('\r');
                                    break;
                                case 'n':
                                    Token.Append('\n');
                                    break;
                                case 'b':
                                    Token.Append('\b');
                                    break;
                                case 'f':
                                    Token.Append('\f');
                                    break;
                                case 'u':
                                    {
                                        string s = aJSON.Substring(i + 1, 4);
                                        Token.Append((char)int.Parse(
                                            s,
                                            System.Globalization.NumberStyles.AllowHexSpecifier));
                                        i += 4;
                                        break;
                                    }
                                default:
                                    Token.Append(C);
                                    break;
                            }
                        }
                        break;
                    case '/':
                        if (allowLineComments && !QuoteMode && i + 1 < aJSON.Length && aJSON[i + 1] == '/')
                        {
                            while (++i < aJSON.Length && aJSON[i] != '\n' && aJSON[i] != '\r') ;
                            break;
                        }
                        Token.Append(aJSON[i]);
                        break;
                    case '\uFEFF': // remove / ignore BOM (Byte Order Mark)
                        break;

                    default:
                        Token.Append(aJSON[i]);
                        break;
                }
                ++i;
            }
            if (QuoteMode)
            {
                throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
            }
            if (ctx == null)
                return ParseElement(Token.ToString(), TokenIsQuoted);
            return ctx;
        }

    }
    // End of JSONNode

    public partial class M1JsonArray : M1_JSONNode
    {
        private List<M1_JSONNode> m_List = new List<M1_JSONNode>();
        private bool inline = false;
        public override bool Inline
        {
            get { return inline; }
            set { inline = value; }
        }

        public override JSONNodeType Tag { get { return JSONNodeType.Array; } }
        public override bool IsArray { get { return true; } }
        public override Enumerator GetEnumerator() { return new Enumerator(m_List.GetEnumerator()); }

        public override M1_JSONNode this[int aIndex]
        {
            get
            {
                if (aIndex < 0 || aIndex >= m_List.Count)
                    return new M1JsonLazyCreator(this);
                return m_List[aIndex];
            }
            set
            {
                if (value == null)
                    value = M1JsonNull.CreateOrGet();
                if (aIndex < 0 || aIndex >= m_List.Count)
                    m_List.Add(value);
                else
                    m_List[aIndex] = value;
            }
        }

        public override M1_JSONNode this[string aKey]
        {
            get { return new M1JsonLazyCreator(this); }
            set
            {
                if (value == null)
                    value = M1JsonNull.CreateOrGet();
                m_List.Add(value);
            }
        }

        public override int Count
        {
            get { return m_List.Count; }
        }

        public override void Add(string aKey, M1_JSONNode aItem)
        {
            if (aItem == null)
                aItem = M1JsonNull.CreateOrGet();
            m_List.Add(aItem);
        }

        public override M1_JSONNode Remove(int aIndex)
        {
            if (aIndex < 0 || aIndex >= m_List.Count)
                return null;
            M1_JSONNode tmp = m_List[aIndex];
            m_List.RemoveAt(aIndex);
            return tmp;
        }

        public override M1_JSONNode Remove(M1_JSONNode aNode)
        {
            m_List.Remove(aNode);
            return aNode;
        }

        public override void Clear()
        {
            m_List.Clear();
        }

        public override M1_JSONNode Clone()
        {
            var node = new M1JsonArray();
            node.m_List.Capacity = m_List.Capacity;
            foreach (var n in m_List)
            {
                if (n != null)
                    node.Add(n.Clone());
                else
                    node.Add(null);
            }
            return node;
        }

        public override IEnumerable<M1_JSONNode> Children
        {
            get
            {
                foreach (M1_JSONNode N in m_List)
                    yield return N;
            }
        }


        internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
        {
            aSB.Append('[');
            int count = m_List.Count;
            if (inline)
                aMode = JSONTextMode.Compact;
            for (int i = 0; i < count; i++)
            {
                if (i > 0)
                    aSB.Append(',');
                if (aMode == JSONTextMode.Indent)
                    aSB.AppendLine();

                if (aMode == JSONTextMode.Indent)
                    aSB.Append(' ', aIndent + aIndentInc);
                m_List[i].WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
            }
            if (aMode == JSONTextMode.Indent)
                aSB.AppendLine().Append(' ', aIndent);
            aSB.Append(']');
        }
    }
    // End of JSONArray

    public partial class M1JsonObject : M1_JSONNode
    {
        private Dictionary<string, M1_JSONNode> m_Dict = new Dictionary<string, M1_JSONNode>();

        private bool inline = false;
        public override bool Inline
        {
            get { return inline; }
            set { inline = value; }
        }

        public override JSONNodeType Tag { get { return JSONNodeType.Object; } }
        public override bool IsObject { get { return true; } }

        public override Enumerator GetEnumerator() { return new Enumerator(m_Dict.GetEnumerator()); }


        public override M1_JSONNode this[string aKey]
        {
            get
            {
                if (m_Dict.ContainsKey(aKey))
                    return m_Dict[aKey];
                else
                    return new M1JsonLazyCreator(this, aKey);
            }
            set
            {
                if (value == null)
                    value = M1JsonNull.CreateOrGet();
                if (m_Dict.ContainsKey(aKey))
                    m_Dict[aKey] = value;
                else
                    m_Dict.Add(aKey, value);
            }
        }

        public override M1_JSONNode this[int aIndex]
        {
            get
            {
                if (aIndex < 0 || aIndex >= m_Dict.Count)
                    return null;
                return m_Dict.ElementAt(aIndex).Value;
            }
            set
            {
                if (value == null)
                    value = M1JsonNull.CreateOrGet();
                if (aIndex < 0 || aIndex >= m_Dict.Count)
                    return;
                string key = m_Dict.ElementAt(aIndex).Key;
                m_Dict[key] = value;
            }
        }

        public override int Count
        {
            get { return m_Dict.Count; }
        }

        public override void Add(string aKey, M1_JSONNode aItem)
        {
            if (aItem == null)
                aItem = M1JsonNull.CreateOrGet();

            if (aKey != null)
            {
                if (m_Dict.ContainsKey(aKey))
                    m_Dict[aKey] = aItem;
                else
                    m_Dict.Add(aKey, aItem);
            }
            else
                m_Dict.Add(Guid.NewGuid().ToString(), aItem);
        }

        public override M1_JSONNode Remove(string aKey)
        {
            if (!m_Dict.ContainsKey(aKey))
                return null;
            M1_JSONNode tmp = m_Dict[aKey];
            m_Dict.Remove(aKey);
            return tmp;
        }

        public override M1_JSONNode Remove(int aIndex)
        {
            if (aIndex < 0 || aIndex >= m_Dict.Count)
                return null;
            var item = m_Dict.ElementAt(aIndex);
            m_Dict.Remove(item.Key);
            return item.Value;
        }

        public override M1_JSONNode Remove(M1_JSONNode aNode)
        {
            try
            {
                var item = m_Dict.Where(k => k.Value == aNode).First();
                m_Dict.Remove(item.Key);
                return aNode;
            }
            catch
            {
                return null;
            }
        }

        public override void Clear()
        {
            m_Dict.Clear();
        }

        public override M1_JSONNode Clone()
        {
            var node = new M1JsonObject();
            foreach (var n in m_Dict)
            {
                node.Add(n.Key, n.Value.Clone());
            }
            return node;
        }

        public override bool HasKey(string aKey)
        {
            return m_Dict.ContainsKey(aKey);
        }

        public override M1_JSONNode GetValueOrDefault(string aKey, M1_JSONNode aDefault)
        {
            M1_JSONNode res;
            if (m_Dict.TryGetValue(aKey, out res))
                return res;
            return aDefault;
        }

        public override IEnumerable<M1_JSONNode> Children
        {
            get
            {
                foreach (KeyValuePair<string, M1_JSONNode> N in m_Dict)
                    yield return N.Value;
            }
        }

        internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
        {
            aSB.Append('{');
            bool first = true;
            if (inline)
                aMode = JSONTextMode.Compact;
            foreach (var k in m_Dict)
            {
                if (!first)
                    aSB.Append(',');
                first = false;
                if (aMode == JSONTextMode.Indent)
                    aSB.AppendLine();
                if (aMode == JSONTextMode.Indent)
                    aSB.Append(' ', aIndent + aIndentInc);
                aSB.Append('\"').Append(Escape(k.Key)).Append('\"');
                if (aMode == JSONTextMode.Compact)
                    aSB.Append(':');
                else
                    aSB.Append(" : ");
                k.Value.WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
            }
            if (aMode == JSONTextMode.Indent)
                aSB.AppendLine().Append(' ', aIndent);
            aSB.Append('}');
        }

    }
    // End of JSONObject

    public partial class M1JsonString : M1_JSONNode
    {
        private string m_Data;

        public override JSONNodeType Tag { get { return JSONNodeType.String; } }
        public override bool IsString { get { return true; } }

        public override Enumerator GetEnumerator() { return new Enumerator(); }


        public override string Value
        {
            get { return m_Data; }
            set
            {
                m_Data = value;
            }
        }

        public M1JsonString(string aData)
        {
            m_Data = aData;
        }
        public override M1_JSONNode Clone()
        {
            return new M1JsonString(m_Data);
        }

        internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
        {
            aSB.Append('\"').Append(Escape(m_Data)).Append('\"');
        }
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
                return true;
            string s = obj as string;
            if (s != null)
                return m_Data == s;
            M1JsonString s2 = obj as M1JsonString;
            if (s2 != null)
                return m_Data == s2.m_Data;
            return false;
        }
        public override int GetHashCode()
        {
            return m_Data.GetHashCode();
        }
        public override void Clear()
        {
            m_Data = "";
        }
    }
    // End of JSONString

    public partial class M1JsonNumber : M1_JSONNode
    {
        private double m_Data;

        public override JSONNodeType Tag { get { return JSONNodeType.Number; } }
        public override bool IsNumber { get { return true; } }
        public override Enumerator GetEnumerator() { return new Enumerator(); }

        public override string Value
        {
            get { return m_Data.ToString(CultureInfo.InvariantCulture); }
            set
            {
                double v;
                if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out v))
                    m_Data = v;
            }
        }

        public override double AsDouble
        {
            get { return m_Data; }
            set { m_Data = value; }
        }
        public override long AsLong
        {
            get { return (long)m_Data; }
            set { m_Data = value; }
        }
        public override ulong AsULong
        {
            get { return (ulong)m_Data; }
            set { m_Data = value; }
        }

        public M1JsonNumber(double aData)
        {
            m_Data = aData;
        }

        public M1JsonNumber(string aData)
        {
            Value = aData;
        }

        public override M1_JSONNode Clone()
        {
            return new M1JsonNumber(m_Data);
        }

        internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
        {
            aSB.Append(Value);
        }
        private static bool IsNumeric(object value)
        {
            return value is int || value is uint
                || value is float || value is double
                || value is decimal
                || value is long || value is ulong
                || value is short || value is ushort
                || value is sbyte || value is byte;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (base.Equals(obj))
                return true;
            M1JsonNumber s2 = obj as M1JsonNumber;
            if (s2 != null)
                return m_Data == s2.m_Data;
            if (IsNumeric(obj))
                return Convert.ToDouble(obj) == m_Data;
            return false;
        }
        public override int GetHashCode()
        {
            return m_Data.GetHashCode();
        }
        public override void Clear()
        {
            m_Data = 0;
        }
    }
    // End of JSONNumber

    public partial class M1JsonBool : M1_JSONNode
    {
        private bool m_Data;

        public override JSONNodeType Tag { get { return JSONNodeType.Boolean; } }
        public override bool IsBoolean { get { return true; } }
        public override Enumerator GetEnumerator() { return new Enumerator(); }

        public override string Value
        {
            get { return m_Data.ToString(); }
            set
            {
                bool v;
                if (bool.TryParse(value, out v))
                    m_Data = v;
            }
        }
        public override bool AsBool
        {
            get { return m_Data; }
            set { m_Data = value; }
        }

        public M1JsonBool(bool aData)
        {
            m_Data = aData;
        }

        public M1JsonBool(string aData)
        {
            Value = aData;
        }

        public override M1_JSONNode Clone()
        {
            return new M1JsonBool(m_Data);
        }

        internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
        {
            aSB.Append((m_Data) ? "true" : "false");
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is bool)
                return m_Data == (bool)obj;
            return false;
        }
        public override int GetHashCode()
        {
            return m_Data.GetHashCode();
        }
        public override void Clear()
        {
            m_Data = false;
        }
    }
    // End of JSONBool

    public partial class M1JsonNull : M1_JSONNode
    {
        static M1JsonNull m_StaticInstance = new M1JsonNull();
        public static bool reuseSameInstance = true;
        public static M1JsonNull CreateOrGet()
        {
            if (reuseSameInstance)
                return m_StaticInstance;
            return new M1JsonNull();
        }
        private M1JsonNull() { }

        public override JSONNodeType Tag { get { return JSONNodeType.NullValue; } }
        public override bool IsNull { get { return true; } }
        public override Enumerator GetEnumerator() { return new Enumerator(); }

        public override string Value
        {
            get { return "null"; }
            set { }
        }
        public override bool AsBool
        {
            get { return false; }
            set { }
        }

        public override M1_JSONNode Clone()
        {
            return CreateOrGet();
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;
            return (obj is M1JsonNull);
        }
        public override int GetHashCode()
        {
            return 0;
        }

        internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
        {
            aSB.Append("null");
        }
    }
    // End of JSONNull

    internal partial class M1JsonLazyCreator : M1_JSONNode
    {
        private M1_JSONNode m_Node = null;
        private string m_Key = null;
        public override JSONNodeType Tag { get { return JSONNodeType.None; } }
        public override Enumerator GetEnumerator() { return new Enumerator(); }

        public M1JsonLazyCreator(M1_JSONNode aNode)
        {
            m_Node = aNode;
            m_Key = null;
        }

        public M1JsonLazyCreator(M1_JSONNode aNode, string aKey)
        {
            m_Node = aNode;
            m_Key = aKey;
        }

        private T Set<T>(T aVal) where T : M1_JSONNode
        {
            if (m_Key == null)
                m_Node.Add(aVal);
            else
                m_Node.Add(m_Key, aVal);
            m_Node = null; // Be GC friendly.
            return aVal;
        }

        public override M1_JSONNode this[int aIndex]
        {
            get { return new M1JsonLazyCreator(this); }
            set { Set(new M1JsonArray()).Add(value); }
        }

        public override M1_JSONNode this[string aKey]
        {
            get { return new M1JsonLazyCreator(this, aKey); }
            set { Set(new M1JsonObject()).Add(aKey, value); }
        }

        public override void Add(M1_JSONNode aItem)
        {
            Set(new M1JsonArray()).Add(aItem);
        }

        public override void Add(string aKey, M1_JSONNode aItem)
        {
            Set(new M1JsonObject()).Add(aKey, aItem);
        }

        public static bool operator ==(M1JsonLazyCreator a, object b)
        {
            if (b == null)
                return true;
            return System.Object.ReferenceEquals(a, b);
        }

        public static bool operator !=(M1JsonLazyCreator a, object b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return true;
            return System.Object.ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override int AsInt
        {
            get { Set(new M1JsonNumber(0)); return 0; }
            set { Set(new M1JsonNumber(value)); }
        }

        public override float AsFloat
        {
            get { Set(new M1JsonNumber(0.0f)); return 0.0f; }
            set { Set(new M1JsonNumber(value)); }
        }

        public override double AsDouble
        {
            get { Set(new M1JsonNumber(0.0)); return 0.0; }
            set { Set(new M1JsonNumber(value)); }
        }

        public override long AsLong
        {
            get
            {
                if (longAsString)
                    Set(new M1JsonString("0"));
                else
                    Set(new M1JsonNumber(0.0));
                return 0L;
            }
            set
            {
                if (longAsString)
                    Set(new M1JsonString(value.ToString()));
                else
                    Set(new M1JsonNumber(value));
            }
        }

        public override ulong AsULong
        {
            get
            {
                if (longAsString)
                    Set(new M1JsonString("0"));
                else
                    Set(new M1JsonNumber(0.0));
                return 0L;
            }
            set
            {
                if (longAsString)
                    Set(new M1JsonString(value.ToString()));
                else
                    Set(new M1JsonNumber(value));
            }
        }

        public override bool AsBool
        {
            get { Set(new M1JsonBool(false)); return false; }
            set { Set(new M1JsonBool(value)); }
        }

        public override M1JsonArray AsArray
        {
            get { return Set(new M1JsonArray()); }
        }

        public override M1JsonObject AsObject
        {
            get { return Set(new M1JsonObject()); }
        }
        internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
        {
            aSB.Append("null");
        }
    }
    // End of JSONLazyCreator

    public static class M1_JSON
    {
        public static M1_JSONNode Parse(string aJSON)
        {
            return M1_JSONNode.Parse(aJSON);
        }
    }
}