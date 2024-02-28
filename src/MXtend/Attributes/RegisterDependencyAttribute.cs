using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXtend.Attributes
{
    /// <summary>
    /// Add this attribute to the classes you wish to be automatically registered in DI container.
    /// </summary>
    public class RegisterDependencyAttribute : Attribute
    {
        private Type? _interface;

        /// <summary>
        /// If null - will try to automatically resolve interface, only if inherits one interface.<br />
        /// If not null - will use this parameter to register a resolve type.
        /// </summary>
        public Type? Interface
        {
            get => _interface;
            set
            {
                if (value == null)
                {
                    _interface = value;
                    return;
                }

                if (!value.IsInterface)
                    throw new ArgumentException("Interface cannot be not an interface.");

                _interface = value;
            }
        }
        public DependencyType Type { get; set; } = DependencyType.Transient;
    }

    public enum DependencyType
    {
        Transient,
        Singleton,
    }
}
