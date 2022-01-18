namespace Authoritative
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using ZenLib;

    /// <summary>
    /// Domain Name as list of labels.
    /// </summary>
    public struct DomainName : IEquatable<DomainName>
    {
        // Rajdeep Comments: IEquatable<DomainName> is an Interface
        //                   For details, check this https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/types/interfaces
        
        
        /// <summary>
        /// The value for the domain represented as a list.
        /// For example, bar.foo.com is represented as [com, foo, bar].
        /// '*' is represented using (byte)1.
        /// </summary>
        public IList<byte> Value { get; set; }
        
        // Rajdeep Comments: Above function creates an Ilist<byte> with the name "value", which
        //                   is the domain name in reverse order. I understand it is a list of
        //                   of integers. (Is there some mapping from integers to names?)

        /// <summary>
        /// Create a Zen domain name from a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The domain name.</returns>
        public static Zen<DomainName> Create(Zen<IList<byte>> value)
        {
            return Language.Create<DomainName>(("Value", value));
        }
        
        // Rajdeep comments: Okay, so I pass an IList<byte> value to the above function
        //                   which calls Value to create a new object of type DomainName.

        /// <summary>
        /// Convert the domain name to a string format.
        /// </summary>
        /// <returns>The string.</returns>
        [ExcludeFromCodeCoverage]
        //Rajdeep Comments: Not exactly sure, but I understand that it has something to do 
        //                  with excluding the following code during unit tests?
        public override string ToString()
        {
            return string.Join(".", Value);
        }

        /// <summary>
        ///     Equality for domain names.
        /// </summary>
        /// <param name="other">The other domain name.</param>
        /// <returns>True or false.</returns>
        public bool Equals(DomainName other)
        {
            return Value.SequenceEqual(other.Value);
        }
        
        //Doubts: Both ExcludeFromCodeCoverage and IEquatable have am Equals method.
        //        Which one is being defined above? Or am I missing something?
    }

    /// <summary>
    /// Domain name Zen extension methods.
    /// </summary>
    public static class DomainNameExtensions
    {
        /// <summary>
        /// Gets the list of labels for the domain name.
        /// </summary>
        /// <param name="dn">The domain name.</param>
        /// <returns>The list of labels.</returns>
        public static Zen<IList<byte>> GetValue(this Zen<DomainName> dn) => dn.GetField<DomainName, IList<byte>>("Value");

        /// <summary>
        /// Checks if a domain name is a wildcard domain name.
        /// </summary>
        /// <param name="dn">The domain name.</param>
        /// <returns>A boolean.</returns>
        public static Zen<bool> IsWildcardDomain(this Zen<DomainName> dn) => dn.GetValue().At(dn.GetValue().Length() - 1).Value() == 1;
    }
}
