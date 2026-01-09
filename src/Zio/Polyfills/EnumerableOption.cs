// ReSharper disable CheckNamespace

namespace System.IO
{

#if !NET5_0_OR_GREATER && NETSTANDARD2_0


    /// <summary>Specifies the type of wildcard matching to use.</summary>
    public enum MatchType
    {
        /// <summary>
        ///   <para>Matches using '*' and '?' wildcards.</para>
        ///   <para>
        ///     <c>*</c> matches from zero to any amount of characters. <c>?</c> matches exactly one character. <c>*.*</c> matches any name with a period in it (with <see cref="F:System.IO.MatchType.Win32" />, this would match all items).</para>
        /// </summary>
        Simple,

        /// <summary>
        ///   <para>Match using Win32 DOS style matching semantics.</para>
        ///   <para>'*', '?', '&lt;', '&gt;', and '"' are all considered wildcards. Matches in a traditional DOS <c>/</c> Windows command prompt way. <c>*.*</c> matches all files. <c>?</c> matches collapse to periods. <c>file.??t</c> will match <c>file.t</c>, <c>file.at</c>, and <c>file.txt</c>.</para>
        /// </summary>
        Win32,
    }

    /// <summary>Specifies the type of character casing to match.</summary>
    public enum MatchCasing
    {
        /// <summary>Matches using the default casing for the given platform.</summary>
        PlatformDefault,

        /// <summary>Matches respecting character casing.</summary>
        CaseSensitive,

        /// <summary>Matches ignoring character casing.</summary>
        CaseInsensitive,
    }

#endif


#if !NET5_0_OR_GREATER && NETSTANDARD2_0
//using System.IO;
//using System.Security;


    /// <summary>Provides file and directory enumeration options.</summary>
    public class EnumerationOptions
    {
        private int _maxRecursionDepth;

        internal const int DefaultMaxRecursionDepth = int.MaxValue;

        /// <summary>
        /// For internal use. These are the options we want to use if calling the existing Directory/File APIs where you don't
        /// explicitly specify EnumerationOptions.
        /// </summary>
        internal static EnumerationOptions Compatible { get; } =
            new EnumerationOptions { MatchType = MatchType.Win32, AttributesToSkip = 0, IgnoreInaccessible = false };

        private static EnumerationOptions CompatibleRecursive { get; } =
            new EnumerationOptions { RecurseSubdirectories = true, MatchType = MatchType.Win32, AttributesToSkip = 0, IgnoreInaccessible = false };

        /// <summary>
        /// Internal singleton for default options.
        /// </summary>
        internal static EnumerationOptions Default { get; } = new EnumerationOptions();

        /// <summary>Initializes a new instance of the <see cref="EnumerationOptions" /> class with the recommended default options.</summary>
        public EnumerationOptions()
        {
            IgnoreInaccessible = true;
            AttributesToSkip = FileAttributes.Hidden | FileAttributes.System;
            MaxRecursionDepth = DefaultMaxRecursionDepth;
        }

        /// <summary>
        /// Converts SearchOptions to FindOptions. Throws if undefined SearchOption.
        /// </summary>
        internal static EnumerationOptions FromSearchOption(SearchOption searchOption)
        {
            if ((searchOption != SearchOption.TopDirectoryOnly) && (searchOption != SearchOption.AllDirectories))
                throw new ArgumentOutOfRangeException(nameof(searchOption), $"Argument Out Of Range {searchOption}");

            return searchOption == SearchOption.AllDirectories ? CompatibleRecursive : Compatible;
        }

        /// <summary>Gets or sets a value that indicates whether to recurse into subdirectories while enumerating. The default is <see langword="false" />.</summary>
        /// <value><see langword="true" /> to recurse into subdirectories; otherwise, <see langword="false" />.</value>
        public bool RecurseSubdirectories { get; set; }

        /// <summary>Gets or sets a value that indicates whether to skip files or directories when access is denied (for example, <see cref="UnauthorizedAccessException" /> or <see cref="SecurityException" />). The default is <see langword="true" />.</summary>
        /// <value><see langword="true" /> to skip innacessible files or directories; otherwise, <see langword="false" />.</value>
        public bool IgnoreInaccessible { get; set; }

        /// <summary>Gets or sets the suggested buffer size, in bytes. The default is 0 (no suggestion).</summary>
        /// <value>The buffer size.</value>
        /// <remarks>Not all platforms use user allocated buffers, and some require either fixed buffers or a buffer that has enough space to return a full result.
        /// One scenario where this option is useful is with remote share enumeration on Windows. Having a large buffer may result in better performance as more results can be batched over the wire (for example, over a network share).
        /// A "large" buffer, for example, would be 16K. Typical is 4K.
        /// The suggested buffer size will not be used if it has no meaning for the native APIs on the current platform or if it would be too small for getting at least a single result.</remarks>
        public int BufferSize { get; set; }

        /// <summary>Gets or sets the attributes to skip. The default is <c>FileAttributes.Hidden | FileAttributes.System</c>.</summary>
        /// <value>The attributes to skip.</value>
        public FileAttributes AttributesToSkip { get; set; }

        /// <summary>Gets or sets the match type.</summary>
        /// <value>One of the enumeration values that indicates the match type.</value>
        /// <remarks>For APIs that allow specifying a match expression, this property allows you to specify how to interpret the match expression.
        /// The default is simple matching where '*' is always 0 or more characters and '?' is a single character.</remarks>
        public MatchType MatchType { get; set; }

        /// <summary>Gets or sets the case matching behavior.</summary>
        /// <value>One of the enumeration values that indicates the case matching behavior.</value>
        /// <remarks>For APIs that allow specifying a match expression, this property allows you to specify the case matching behavior.
        /// The default is to match platform defaults, which are gleaned from the case sensitivity of the temporary folder.</remarks>
        public MatchCasing MatchCasing { get; set; }

        /// <summary>Gets or sets a value that indicates the maximum directory depth to recurse while enumerating, when <see cref="RecurseSubdirectories" /> is set to <see langword="true" />.</summary>
        /// <value>A number that represents the maximum directory depth to recurse while enumerating. The default value is <see cref="int.MaxValue" />.</value>
        /// <remarks>If <see cref="MaxRecursionDepth" /> is set to zero, enumeration returns the contents of the initial directory.</remarks>
        public int MaxRecursionDepth
        {
            get => _maxRecursionDepth;
            set
            {
                //ArgumentOutOfRangeException.ThrowIfNegative(value);
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));

                _maxRecursionDepth = value;
            }
        }

        /// <summary>Gets or sets a value that indicates whether to return the special directory entries "." and "..".</summary>
        /// <value><see langword="true" /> to return the special directory entries "." and ".."; otherwise, <see langword="false" />.</value>
        public bool ReturnSpecialDirectories { get; set; }
    }
#endif

    public static class EnumerationOptionsUtils
    {

        /// <summary>
        /// Converts SearchOptions to FindOptions. Throws if undefined SearchOption.
        /// </summary>
        public static EnumerationOptions FromSearchOption(SearchOption searchOption)
        {
            if ((searchOption != SearchOption.TopDirectoryOnly) && (searchOption != SearchOption.AllDirectories))
                throw new ArgumentOutOfRangeException(nameof(searchOption), $"Argument Out Of Range {searchOption}");

            return searchOption == SearchOption.AllDirectories ? CompatibleSafeRecursive : CompatibleSafe;
        }
        
        public static EnumerationOptions Default { get; } = new EnumerationOptions();
        
        // public static EnumerationOptions GetOrDefault(EnumerationOptions? enumerationOptions, EnumerationOptions defaultValue)
        // {
        //     return enumerationOptions ?? defaultValue;
        // }
        
        /// <summary>
        /// Converts SearchOptions to FindOptions. Throws if undefined SearchOption.
        /// </summary>
        public static EnumerationOptions GetOrDefault(EnumerationOptions? enumerationOptions)
        {
            return enumerationOptions ?? new EnumerationOptions();
        }
        
        /// <summary>
        /// Converts SearchOptions to FindOptions. Throws if undefined SearchOption.
        /// </summary>
        public static SearchOption ToSearchOption(EnumerationOptions enumerationOptions)
        {
            return enumerationOptions.RecurseSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        }
        
        /// <summary>
        /// For internal use. These are the options we want to use if calling the existing Directory/File APIs where you don't
        /// explicitly specify EnumerationOptions.
        /// </summary>
        private static EnumerationOptions CompatibleSafe { get; } =
            new EnumerationOptions { MatchType = MatchType.Win32, AttributesToSkip = 0, IgnoreInaccessible = true };

        private static EnumerationOptions CompatibleSafeRecursive { get; } =
            new EnumerationOptions { RecurseSubdirectories = true, MatchType = MatchType.Win32, AttributesToSkip = 0, IgnoreInaccessible = true };
    }

}