#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Intent.Modules.Java.Maven.Utils;

/// <summary>
/// A port to C# of <see href="https://github.com/apache/maven/blob/master/maven-artifact/src/main/java/org/apache/maven/artifact/versioning/ComparableVersion.java"/>
/// which is a version comparison of Maven versions as described at <see href="https://cwiki.apache.org/confluence/display/MAVENOLD/Versioning"/>.
/// </summary>
/// <remarks>
/// This is a somewhat "dumb" conversion of the .java file hence the unidiomatic C#.
/// </remarks>
public class ComparableVersion : IComparable<ComparableVersion>, IComparable, IEquatable<ComparableVersion>
{
    public bool Equals(ComparableVersion? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _items.Equals(other._items);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ComparableVersion)obj);
    }

    public override int GetHashCode()
    {
        return _items.GetHashCode();
    }

    public static bool operator ==(ComparableVersion? left, ComparableVersion? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ComparableVersion? left, ComparableVersion? right)
    {
        return !Equals(left, right);
    }

    public int CompareTo(object? obj)
    {
        if (ReferenceEquals(null, obj)) return 1;
        if (ReferenceEquals(this, obj)) return 0;
        return obj is ComparableVersion other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(ComparableVersion)}");
    }

    public static bool operator <(ComparableVersion? left, ComparableVersion? right)
    {
        return Comparer<ComparableVersion>.Default.Compare(left, right) < 0;
    }

    public static bool operator >(ComparableVersion? left, ComparableVersion? right)
    {
        return Comparer<ComparableVersion>.Default.Compare(left, right) > 0;
    }

    public static bool operator <=(ComparableVersion? left, ComparableVersion? right)
    {
        return Comparer<ComparableVersion>.Default.Compare(left, right) <= 0;
    }

    public static bool operator >=(ComparableVersion? left, ComparableVersion? right)
    {
        return Comparer<ComparableVersion>.Default.Compare(left, right) >= 0;
    }

    private const int MaxIntItemLength = 9;

    private const int MaxLongItemLength = 18;

    private readonly string _value;

    private readonly ListItem _items;

    public static ComparableVersion Parse(string version)
    {
        return new ComparableVersion(version);
    }

    public static bool TryParse(string version, out ComparableVersion? result)
    {
        try
        {
            result = Parse(version);
            return true;
        }
        catch (Exception )
        {
            result = default;
            return false;
        }
    }

    private ComparableVersion(string version)
    {
        _value = version;
        _items = new ListItem();

        version = version.ToLowerInvariant();

        var list = _items;

        var stack = new Stack<IItem>();
        stack.Push(list);

        var isDigit = false;

        var startIndex = 0;

        for (var i = 0; i < version.Length; i++)
        {
            var c = version[i];

            if (c == '.')
            {
                list.Add(i == startIndex ? IntItem.Zero : ParseItem(isDigit, version.JavaSubstring(startIndex, i)));

                startIndex = i + 1;
            }
            else if (c == '-')
            {
                list.Add(i == startIndex ? IntItem.Zero : ParseItem(isDigit, version.JavaSubstring(startIndex, i)));

                startIndex = i + 1;

                list.Add(list = new ListItem());
                stack.Push(list);
            }
            else if (char.IsDigit(c))
            {
                if (!isDigit && i > startIndex)
                {
                    list.Add(new StringItem(version.JavaSubstring(startIndex, i), true));
                    startIndex = i;

                    list.Add(list = new ListItem());
                    stack.Push(list);
                }

                isDigit = true;
            }
            else
            {
                if (isDigit && i > startIndex)
                {
                    list.Add(ParseItem(true, version.JavaSubstring(startIndex, i)));
                    startIndex = i;

                    list.Add(list = new ListItem());
                    stack.Push(list);
                }

                isDigit = false;
            }
        }

        if (version.Length > startIndex)
        {
            list.Add(ParseItem(isDigit, version.JavaSubstring(startIndex)));
        }

        while (stack.Count != 0)
        {
            list = (ListItem)stack.Pop();
            list.Normalize();
        }
    }

    private static IItem ParseItem(bool isDigit, string buf)
    {
        if (!isDigit)
        {
            return new StringItem(buf, false);
        }

        buf = StripLeadingZeroes(buf);
        if (buf.Length <= MaxIntItemLength)
        {
            // lower than 2^31
            return new IntItem(buf);
        }

        if (buf.Length <= MaxLongItemLength)
        {
            // lower than 2^63
            return new LongItem(buf);
        }

        return new BigIntegerItem(buf);
    }

    private static string StripLeadingZeroes(string? buf)
    {
        if (string.IsNullOrEmpty(buf))
        {
            return "0";
        }
        for (var i = 0; i < buf.Length; ++i)
        {
            var c = buf[i];
            if (c != '0')
            {
                return buf.JavaSubstring(i);
            }
        }
        return buf;
    }

    public int CompareTo(ComparableVersion? o)
    {
        return _items.CompareTo(o?._items);
    }

    public override string ToString()
    {
        return _value;
    }

    public string GetCanonical()
    {
        return _items.ToString();
    }

    // The below were in IItem
    private const int IntItemType = 3;
    private const int LongItemType = 4;
    private const int BigIntegerItemType = 0;
    private const int StringItemType = 1;
    private const int ListItemType = 2;

    private interface IItem
    {
        int CompareTo(IItem? item);

        int GetItemType();

        bool IsNull();
    }

    /// <summary>
    /// Represents a numeric item in the version item list that can be represented with an int.
    /// </summary>
    private class IntItem : IItem, IEquatable<IntItem>, IComparable<IItem>
    {
        public bool Equals(IntItem? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _value == other._value;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((IntItem)obj);
        }

        public override int GetHashCode()
        {
            return _value;
        }

        public static bool operator ==(IntItem? left, IntItem? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(IntItem? left, IntItem? right)
        {
            return !Equals(left, right);
        }

        private readonly int _value;

        public static readonly IntItem Zero = new();

        private IntItem()
        {
            _value = 0;
        }

        public IntItem(string str)
        {
            _value = int.Parse(str);
        }

        public int GetItemType()
        {
            return IntItemType;
        }

        public bool IsNull()
        {
            return _value == 0;
        }

        public int CompareTo(IItem? item)
        {
            if (item == null)
            {
                return (_value == 0) ? 0 : 1; // 1.0 == 1, 1.1 > 1
            }

            switch (item.GetItemType())
            {
                case IntItemType:
                    var itemValue = ((IntItem)item)._value;
                    return _value.CompareTo(itemValue);
                case LongItemType:
                case BigIntegerItemType:
                    return -1;

                case StringItemType:
                    return 1; // 1.1 > 1-sp

                case ListItemType:
                    return 1; // 1.1 > 1-1

                default:
                    throw new InvalidOperationException("invalid item: " + item.GetType());
            }
        }

        public override string ToString()
        {
            return _value.ToString(CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Represents a numeric item in the version item list that can be represented with a long.
    /// </summary>
    private class LongItem : IItem, IEquatable<LongItem>, IComparable<IItem>
    {
        public bool Equals(LongItem? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _value == other._value;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((LongItem)obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(LongItem? left, LongItem? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LongItem? left, LongItem? right)
        {
            return !Equals(left, right);
        }

        private readonly long _value;

        public LongItem(string str)
        {
            _value = long.Parse(str);
        }

        public int GetItemType()
        {
            return LongItemType;
        }

        public bool IsNull()
        {
            return _value == 0;
        }

        public int CompareTo(IItem? item)
        {
            if (item == null)
            {
                return (_value == 0) ? 0 : 1; // 1.0 == 1, 1.1 > 1
            }

            switch (item.GetItemType())
            {
                case IntItemType:
                    return 1;
                case LongItemType:
                    var itemValue = ((LongItem)item)._value;
                    return _value.CompareTo(itemValue);
                case BigIntegerItemType:
                    return -1;

                case StringItemType:
                    return 1; // 1.1 > 1-sp

                case ListItemType:
                    return 1; // 1.1 > 1-1

                default:
                    throw new InvalidOperationException("invalid item: " + item.GetType());
            }
        }

        public override string ToString()
        {
            return _value.ToString(CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Represents a numeric item in the version item list.
    /// </summary>
    private class BigIntegerItem : IItem, IEquatable<BigIntegerItem>, IComparable<IItem>
    {
        public bool Equals(BigIntegerItem? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _value.Equals(other._value);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BigIntegerItem)obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(BigIntegerItem? left, BigIntegerItem? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BigIntegerItem? left, BigIntegerItem? right)
        {
            return !Equals(left, right);
        }

        private readonly BigInteger _value;

        public BigIntegerItem(string str)
        {
            _value = BigInteger.Parse(str);
        }

        public int GetItemType()
        {
            return BigIntegerItemType;
        }

        public bool IsNull()
        {
            return _value.IsZero;
        }

        public int CompareTo(IItem? item)
        {
            if (item == null)
            {
                return _value.IsZero ? 0 : 1; // 1.0 == 1, 1.1 > 1
            }

            switch (item.GetItemType())
            {
                case IntItemType:
                case LongItemType:
                    return 1;

                case BigIntegerItemType:
                    return _value.CompareTo(((BigIntegerItem)item)._value);

                case StringItemType:
                    return 1; // 1.1 > 1-sp

                case ListItemType:
                    return 1; // 1.1 > 1-1

                default:
                    throw new InvalidOperationException("invalid item: " + item.GetType());
            }
        }

        public override string ToString()
        {
            return _value.ToString(CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Represents a version list item. This class is used both for the global item list and for sub-lists (which start
    /// with '-(number)' in the version specification).
    /// </summary>
    private class ListItem : List<IItem>, IItem
    {
        public bool Equals(ListItem? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (other.Count != Count)
            {
                return false;
            }

            return other
                .Zip(this)
                .All(item => item.First.Equals(item.Second));
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ListItem)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();

            foreach (var item in this)
            {
                hashCode.Add(item.GetHashCode());
            }

            return hashCode.ToHashCode();
        }

        public static bool operator ==(ListItem? left, ListItem? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ListItem? left, ListItem right)
        {
            return !Equals(left, right);
        }

        public int GetItemType()
        {
            return ListItemType;
        }

        public bool IsNull()
        {
            return (Count == 0);
        }

        public void Normalize()
        {
            for (var i = Count - 1; i >= 0; i--)
            {
                var lastItem = this[i];

                if (lastItem.IsNull())
                {
                    // remove null trailing items: 0, "", empty list
                    RemoveAt(i);
                }
                else if (lastItem is not ListItem)
                {
                    break;
                }
            }
        }

        public int CompareTo(IItem? item)
        {
            if (item == null)
            {
                if (Count == 0)
                {
                    return 0; // 1-0 = 1- (normalize) = 1
                }
                // Compare the entire list of items with null - not just the first one, MNG-6964
                foreach (var i in this)
                {
                    var result = i.CompareTo(null);
                    if (result != 0)
                    {
                        return result;
                    }
                }
                return 0;
            }
            switch (item.GetItemType())
            {
                case IntItemType:
                case LongItemType:
                case BigIntegerItemType:
                    return -1; // 1-1 < 1.0.x

                case StringItemType:
                    return 1; // 1-1 > 1-sp

                case ListItemType:
                    {
                        using var left = GetEnumerator();
                        using var right = ((ListItem)item).GetEnumerator();

                        var leftHasNext = left.MoveNext();
                        var rightHasNext = right.MoveNext();

                        while (leftHasNext || rightHasNext)
                        {
                            var l = leftHasNext ? left.Current : null;
                            var r = rightHasNext ? right.Current : null;

                            // if this is shorter, then invert the compare and mul with -1
                            var result = l?.CompareTo(r) ?? (r == null ? 0 : -1 * r.CompareTo(l));

                            if (result != 0)
                            {
                                return result;
                            }

                            leftHasNext = left.MoveNext();
                            rightHasNext = right.MoveNext();

                        }

                        return 0;
                    }

                default:
                    throw new InvalidOperationException("invalid item: " + item.GetType());
            }
        }

        public override string ToString()
        {
            var buffer = new StringBuilder();
            foreach (var item in this)
            {
                if (buffer.Length > 0)
                {
                    buffer.Append((item is ListItem) ? '-' : '.');
                }
                buffer.Append(item);
            }
            return buffer.ToString();
        }

        /// <summary>
        /// Return the contents in the same format that is used when you call toString() on a List.
        /// </summary>
        public string ToListString()
        {
            var buffer = new StringBuilder();
            buffer.Append("[");
            foreach (var item in this)
            {
                if (buffer.Length > 1)
                {
                    buffer.Append(", ");
                }
                if (item is ListItem listItem)
                {
                    buffer.Append(listItem.ToListString());
                }
                else
                {
                    buffer.Append(item);
                }
            }
            buffer.Append("]");
            return buffer.ToString();
        }
    }

    /// <summary>
    /// Represents a string in the version item list, usually a qualifier.
    /// </summary>
    private class StringItem : IItem, IEquatable<StringItem>, IComparable<IItem>
    {
        public bool Equals(StringItem? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _value == other._value;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((StringItem)obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(StringItem? left, StringItem? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(StringItem? left, StringItem? right)
        {
            return !Equals(left, right);
        }

        private static readonly string[] Qualifiers = { "alpha", "beta", "milestone", "rc", "snapshot", "", "sp" };

        private static readonly Dictionary<string, string> Aliases = new()
        {
            ["ga"] = string.Empty,
            ["final"] = string.Empty,
            ["release"] = string.Empty,
            ["cr"] = "rc"
        };

        /// <summary>
        /// A comparable value for the empty-string qualifier. This one is used to determine if a given qualifier makes
        /// the version older than one without a qualifier, or more recent.
        /// </summary>
        private static readonly string ReleaseVersionIndex = Array.IndexOf(Qualifiers, "").ToString(CultureInfo.InvariantCulture);

        private readonly string _value;

        public StringItem(string value, bool followedByDigit)
        {
            if (followedByDigit && value.Length == 1)
            {
                // a1 = alpha-1, b1 = beta-1, m1 = milestone-1
                switch (value[0])
                {
                    case 'a':
                        value = "alpha";
                        break;
                    case 'b':
                        value = "beta";
                        break;
                    case 'm':
                        value = "milestone";
                        break;
                }
            }
            _value = Aliases.TryGetValue(value, out var aliasValue)
                ? aliasValue
                : value;
        }

        public int GetItemType()
        {
            return StringItemType;
        }

        public bool IsNull()
        {
            return string.Compare(ComparableQualifier(_value), ReleaseVersionIndex, StringComparison.Ordinal) == 0;
        }

        /// <summary>
        /// Returns a comparable value for a qualifier.
        ///
        /// <para>This method takes into account the ordering of known qualifiers then unknown qualifiers with lexical
        /// ordering.</para>
        ///
        /// <para>just returning an Integer with the index here is faster, but requires a lot of if/then/else to check for -1
        /// or QUALIFIERS.size and then resort to lexical ordering. Most comparisons are decided by the first character,
        /// so this is still fast. If more characters are needed then it requires a lexical sort anyway. </para>
        /// </summary>
        /// <returns>an equivalent value that can be used with lexical comparison</returns>
        private static string ComparableQualifier(string qualifier)
        {
            var i = Array.IndexOf(Qualifiers, qualifier);

            return i == -1 ? Qualifiers.Length + "-" + qualifier : i.ToString();
        }

        public int CompareTo(IItem? item)
        {
            if (item == null)
            {
                // 1-rc < 1, 1-ga > 1
                return string.Compare(ComparableQualifier(_value), ReleaseVersionIndex, StringComparison.Ordinal);
            }
            switch (item.GetItemType())
            {
                case IntItemType:
                case LongItemType:
                case BigIntegerItemType:
                    return -1; // 1.any < 1.1 ?

                case StringItemType:
                    return string.Compare(ComparableQualifier(_value), ComparableQualifier(((StringItem)item)._value), StringComparison.Ordinal);

                case ListItemType:
                    return -1; // 1.any < 1-1

                default:
                    throw new InvalidOperationException("invalid item: " + item.GetType());
            }
        }

        public override string ToString()
        {
            return _value;
        }
    }

    /// <summary>
    /// Main to test version parsing and comparison.
    /// <para>
    /// To check how "1.2.7" compares to "1.2-SNAPSHOT", for example, you can issue
    /// </para>
    /// <c>java -jar ${maven.repo.local}/org/apache/maven/maven-artifact/${maven.version}/maven-artifact-${maven.version}.jar "1.2.7" "1.2-SNAPSHOT"</c>
    /// command to command line. Result of given command will be something like this:
    /// <code>
    /// Display parameters as parsed by Maven (in canonical form) and comparison result:
    /// 1. 1.2.7 == 1.2.7
    ///    1.2.7 &gt; 1.2-SNAPSHOT
    /// 2. 1.2-SNAPSHOT == 1.2-snapshot
    /// </code>
    /// </summary>
    /// <param name="args">the version strings to parse and compare. You can pass arbitrary number of version strings and always
    /// two adjacent will be compared</param>
#pragma warning disable CS7022
    public static void Main(params string[] args)
#pragma warning restore CS7022
    {
        Console.WriteLine("Display parameters as parsed by Maven (in canonical form and as a list of tokens) and"
                                + " comparison result:");
        if (args.Length == 0)
        {
            return;
        }

        ComparableVersion? prev = null;
        var i = 1;
        foreach (var version in args)
        {
            var c = new ComparableVersion(version);

            if (prev != null)
            {
                var compare = prev.CompareTo(c);
                Console.WriteLine("   " + prev + ' '
                                   + ((compare == 0) ? "==" : ((compare < 0) ? "<" : ">")) + ' ' + version);
            }

            Console.WriteLine((i++) + ". " + version + " -> " + c.GetCanonical()
                               + "; tokens: " + c._items.ToListString());

            prev = c;
        }
    }
}