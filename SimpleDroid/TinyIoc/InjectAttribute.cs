using System;

namespace TinyIoC
{   
    internal class InjectAttribute : Attribute
    {
        /// <summary>
        /// Type to resolve
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Named export / registation , optional
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// If true PropertyName will be used as Name
        /// </summary>
        public bool IsNamed { get; set; }

        /// <summary>
        /// May Not reolve
        /// </summary>
        public bool Optional { get; set; }
    }
}